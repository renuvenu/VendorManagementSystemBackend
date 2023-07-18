using Repository;
using Model;
using Model.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MailKit;
using Microsoft.Extensions.Options;

namespace Services
{
    public class PurchaseOrderService
    {
        private readonly DbContextAccess dbContextAccess;
        public ProductPurchaseOrderService productPurchaseOrderService;
        public MailService mailService;
        public PurchaseOrderService() { }
        public PurchaseOrderService(DbContextAccess dbContextAccess, IOptions<MailSettings> mailSettings)
        {
            this.dbContextAccess = dbContextAccess;
            productPurchaseOrderService = new ProductPurchaseOrderService(dbContextAccess);
            this.mailService = new MailService(mailSettings);
        }

        public async Task<ActionResult<PurchaseOrder>> InsertPurchaseOrder(PurchaseOrderRequest purchaseOrderRequest)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder.Id = new Guid();
            purchaseOrder.CreatedBy = purchaseOrderRequest.CreatedBy;
            purchaseOrder.BillingAddress = purchaseOrderRequest.BillingAddress;
            purchaseOrder.BillingAddressCity = purchaseOrderRequest.BillingAddressCity;
            purchaseOrder.BillingAddressState = purchaseOrderRequest.BillingAddressState;
            purchaseOrder.BillingAddressCountry = purchaseOrderRequest.BillingAddressCountry;
            purchaseOrder.BillingAddressZipcode = purchaseOrderRequest.BillingAddressZipcode;
            purchaseOrder.ShippingAddress = purchaseOrderRequest.ShippingAddress;
            purchaseOrder.ShippingAddressCity = purchaseOrderRequest.ShippingAddressCity;
            purchaseOrder.ShippingAddressState = purchaseOrderRequest.ShippingAddressState;
            purchaseOrder.ShippingAddressCountry = purchaseOrderRequest.ShippingAddressCountry;
            purchaseOrder.ShippingAddressZipcode = purchaseOrderRequest.ShippingAddressZipcode;
            purchaseOrder.TermsAndConditions = purchaseOrderRequest.TermsAndConditions;
            purchaseOrder.Description = purchaseOrderRequest.Description;
            purchaseOrder.Status = "Pending";
            purchaseOrder.IsActive = true;
            purchaseOrder.Total = 0;
            await dbContextAccess.PurchaseOrders.AddAsync(purchaseOrder);
            await dbContextAccess.SaveChangesAsync();
            purchaseOrderRequest.ProductsPurchased.ForEach(data =>
            {
                data.PurchaseOrderId = purchaseOrder.Id;
                productPurchaseOrderService.InsertProductPurchaseOrder(data);
            });
            purchaseOrder.Total = GetTotalAmount(purchaseOrder.Id);
            dbContextAccess.PurchaseOrders.Update(purchaseOrder);
            await dbContextAccess.SaveChangesAsync();
            User createdByUser = dbContextAccess.Users.Find(purchaseOrder.CreatedBy);
            var approvers = dbContextAccess.Users.Where(x => x.IsActive && (x.Role.Name == "Approver" || x.Role.Name == "Admin")).ToList();
            approvers.ForEach(user =>
            {
                MailRequest mailRequest = new MailRequest();
                mailRequest.ToEmail = user.Email;
                mailRequest.Subject = "Regarding purchase order approval request";
                mailRequest.Body = $"Kindly review and approve the purchase order created by {createdByUser.Name}({createdByUser.Id})";
                mailService.SendEmailAsync(mailRequest);

            });

            return purchaseOrder;
        }

       public async Task<ActionResult<List<PurchaseOrderWithProdDetailsWithUserName>>> GetPurchasedOrders()
        {
            List<PurchaseOrderWithProdDetailsWithUserName> purchaseOrderWithProductDetails = new List<PurchaseOrderWithProdDetailsWithUserName>();
            purchaseOrderWithProductDetails =dbContextAccess.PurchaseOrders.Where(purchase => purchase.IsActive).ToList().Select(purchaseOrder => new PurchaseOrderWithProdDetailsWithUserName
            {
                PurchaseOrderWithUsersName = new PurchaseOrderWithUsersName
                {
                    Id = purchaseOrder.Id,
                    CreatedBy = new UserForPO
                    {
                        Id = purchaseOrder.CreatedBy,
                        Name = dbContextAccess.Users.Find(purchaseOrder.CreatedBy).Name
                    },
                    TrackingNumber = purchaseOrder.TrackingNumber,
                    OrderDateTime = purchaseOrder.OrderDateTime,
                    DueDate = purchaseOrder.DueDate,
                    ApprovedBy = purchaseOrder.ApprovedBy > 0? new UserForPO
                    {
                        Id = purchaseOrder.ApprovedBy,
                        Name = dbContextAccess.Users.Find(purchaseOrder.ApprovedBy).Name
                    } : null,
                    ApprovedDateTime = purchaseOrder.ApprovedDateTime,
                    BillingAddress = purchaseOrder.BillingAddress,
                    BillingAddressCity = purchaseOrder.BillingAddressCity,
                    BillingAddressCountry = purchaseOrder.BillingAddressCountry,
                    BillingAddressState = purchaseOrder.BillingAddressState,
                    BillingAddressZipcode = purchaseOrder.BillingAddressZipcode,
                    ShippingAddress = purchaseOrder.ShippingAddress,
                    ShippingAddressCity = purchaseOrder.ShippingAddressCity,
                    ShippingAddressCountry = purchaseOrder.ShippingAddressCountry,
                    ShippingAddressState = purchaseOrder.ShippingAddressState,
                    ShippingAddressZipcode = purchaseOrder.ShippingAddressZipcode,
                    TermsAndConditions = purchaseOrder.TermsAndConditions,
                    Description = purchaseOrder.Description,
                    Status = purchaseOrder.Status,
                    Total = purchaseOrder.Total,
                    IsActive = purchaseOrder.IsActive,
                },
                PurchaseProducts = dbContextAccess.productpurchaseorder.Where(prod => prod.PurchaseOrderId == purchaseOrder.Id && prod.IsActive).ToList().Select(purchase => new PurchaseProductDetails
                {
                    Quantity = purchase.Quantity,
                    Price = dbContextAccess.productDetails.Find(purchase.ProductId).Price,
                    ProductDescription = dbContextAccess.productDetails.Find(purchase.ProductId).ProductDescription,
                    ProductName = dbContextAccess.productDetails.Find(purchase.ProductId).ProductName,
                    ProductId = purchase.Id
                }).ToList(),
                VendorForPurchaseOrder = dbContextAccess.productpurchaseorder.Where(e => e.PurchaseOrderId == purchaseOrder.Id).ToList().Select(purchase => new VendorForPurchaseOrder
                {
                    Id = purchase.Id,
                    VendorName = dbContextAccess.VendorDetails.Find(purchase.VendorId).VendorName,
                    VendorType = dbContextAccess.VendorDetails.Find(purchase.VendorId).VendorType
                }).ToList().Last(),
            }).ToList();
            return purchaseOrderWithProductDetails;
        }


        public async Task<ActionResult<List<PurchaseOrderWithProductDetails>>> GetAllPendingPurchaseOrders()
        {
            List<PurchaseOrderWithProductDetails> purchaseOrderWithProductDetails = new List<PurchaseOrderWithProductDetails>();
            purchaseOrderWithProductDetails = dbContextAccess.PurchaseOrders.Where(purchase => purchase.IsActive && purchase.Status == "Pending").ToList().Select(purchaseOrder => new PurchaseOrderWithProductDetails
            {
                PurchaseOrders = purchaseOrder,
                PurchaseProducts = dbContextAccess.productpurchaseorder.Where(prod => prod.PurchaseOrderId == purchaseOrder.Id && prod.IsActive).ToList().Select(purchase => new PurchaseProductDetails
                {
                    Quantity = purchase.Quantity,
                    Price = dbContextAccess.productDetails.Find(purchase.ProductId).Price,
                    ProductDescription = dbContextAccess.productDetails.Find(purchase.ProductId).ProductDescription,
                    ProductName = dbContextAccess.productDetails.Find(purchase.ProductId).ProductName,
                    ProductId = purchase.Id
                }).ToList(),
                VendorForPurchaseOrder = dbContextAccess.productpurchaseorder.Where(e => e.PurchaseOrderId == purchaseOrder.Id).ToList().Select(purchase => new VendorForPurchaseOrder
                {
                    Id = purchase.Id,
                    VendorName = dbContextAccess.VendorDetails.Find(purchase.VendorId).VendorName,
                    VendorType = dbContextAccess.VendorDetails.Find(purchase.VendorId).VendorType
                }).ToList().Last(),
            }).ToList();
            return purchaseOrderWithProductDetails;
        }

        public async Task<ActionResult<List<PurchaseOrderWithProductDetails>>> GetAllPendingRequestsOfaUser(int id)
        {
            List<PurchaseOrderWithProductDetails> purchaseOrderWithProductDetails = new List<PurchaseOrderWithProductDetails>();
            purchaseOrderWithProductDetails = dbContextAccess.PurchaseOrders.Where(purchase => purchase.IsActive && purchase.Status == "Pending" && purchase.CreatedBy == id).ToList().Select(purchaseOrder => new PurchaseOrderWithProductDetails
            {
                PurchaseOrders = purchaseOrder,
                PurchaseProducts = dbContextAccess.productpurchaseorder.Where(prod => prod.PurchaseOrderId == purchaseOrder.Id && prod.IsActive).ToList().Select(purchase => new PurchaseProductDetails
                {
                    Quantity = purchase.Quantity,
                    Price = dbContextAccess.productDetails.Find(purchase.ProductId).Price,
                    ProductDescription = dbContextAccess.productDetails.Find(purchase.ProductId).ProductDescription,
                    ProductName = dbContextAccess.productDetails.Find(purchase.ProductId).ProductName,
                    ProductId = purchase.Id
                }).ToList(),
                VendorForPurchaseOrder = dbContextAccess.productpurchaseorder.Where(e => e.PurchaseOrderId == purchaseOrder.Id).ToList().Select(purchase => new VendorForPurchaseOrder
                {
                    Id = purchase.Id,
                    VendorName = dbContextAccess.VendorDetails.Find(purchase.VendorId).VendorName,
                    VendorType = dbContextAccess.VendorDetails.Find(purchase.VendorId).VendorType
                }).ToList().Last(),
            }).ToList();
            return purchaseOrderWithProductDetails;
        }

        public decimal GetTotalAmount(Guid purchaseOrderId)
        {
            decimal totalAmount = 0;
            List<ProductPurchaseOrder> productPurchaseOrders = dbContextAccess.productpurchaseorder.Where(prod => prod.PurchaseOrderId == purchaseOrderId).ToList();
            productPurchaseOrders.ForEach(data =>
            {
                var productData = dbContextAccess.productDetails.FirstOrDefault(e => e.Id == data.ProductId);
                if (productData != null)
                {
                    totalAmount += (decimal)(data.Quantity * productData.Price);
                }
            });
            return totalAmount;
        }
        public VendorDetails GetVendorDetails(Guid PurchaseOrderId)
        {
            var purchaseProduct = dbContextAccess.productpurchaseorder.FirstOrDefault(p => p.PurchaseOrderId == PurchaseOrderId);
            if (purchaseProduct != null)
            {
                return dbContextAccess.VendorDetails.FirstOrDefault(vendor => vendor.Id == purchaseProduct.VendorId);
            }
            else
            {
                return null;
            }
            }

        public async Task<ActionResult<PurchaseOrder>> DeletePurchaseOrder(Guid PurchaseOrderId)
        {
            var purchaseOrder = await dbContextAccess.PurchaseOrders.FirstOrDefaultAsync(p => p.Id == PurchaseOrderId);
            if(purchaseOrder != null)
            {
                purchaseOrder.IsActive = false;
                dbContextAccess.PurchaseOrders.Update(purchaseOrder);
                await dbContextAccess.SaveChangesAsync();
            }
            return purchaseOrder;
        }

        public async Task<ActionResult<PurchaseOrder>> updatePurchaseOrder(Guid id, PurchaseOrderRequest purchaseOrderRequest)
        {
            PurchaseOrder purchaseOrder = await dbContextAccess.PurchaseOrders.FirstOrDefaultAsync(e => e.Id == id);
            if(purchaseOrder != null)
            {
                purchaseOrder.Id = purchaseOrder.Id;
                purchaseOrder.CreatedBy = purchaseOrderRequest.CreatedBy;
                purchaseOrder.BillingAddress = purchaseOrderRequest.BillingAddress;
                purchaseOrder.BillingAddressCity = purchaseOrderRequest.BillingAddressCity;
                purchaseOrder.BillingAddressState = purchaseOrderRequest.BillingAddressState;
                purchaseOrder.BillingAddressCountry = purchaseOrderRequest.BillingAddressCountry;
                purchaseOrder.BillingAddressZipcode = purchaseOrderRequest.BillingAddressZipcode;
                purchaseOrder.ShippingAddress = purchaseOrderRequest.ShippingAddress;
                purchaseOrder.ShippingAddressCity = purchaseOrderRequest.ShippingAddressCity;
                purchaseOrder.ShippingAddressState = purchaseOrderRequest.ShippingAddressState;
                purchaseOrder.ShippingAddressCountry = purchaseOrderRequest.ShippingAddressCountry;
                purchaseOrder.ShippingAddressZipcode = purchaseOrderRequest.ShippingAddressZipcode;
                purchaseOrder.TermsAndConditions = purchaseOrderRequest.TermsAndConditions;
                purchaseOrder.Description = purchaseOrderRequest.Description;
                purchaseOrder.Status = "Pending";
                purchaseOrder.IsActive = true;
                List<Guid> purchaseOrderIds = new List<Guid>();
                dbContextAccess.productpurchaseorder.ToList().ForEach(purchaseOrder =>
                {
                    if(purchaseOrder.PurchaseOrderId == id)
                    {
                        purchaseOrderIds.Add((Guid)purchaseOrder.Id);
                    }
                });
                purchaseOrderRequest.ProductsPurchased.ForEach(data =>
                {
                    var productPurchase = dbContextAccess.productpurchaseorder.FirstOrDefault(x => x.ProductId == data.ProductId && x.PurchaseOrderId == id);
                    if(productPurchase != null)
                    {
                        purchaseOrderIds.Remove(productPurchase.Id);
                        productPurchase.Quantity = data.Quantity;
                        productPurchase.VendorId  = data.VendorId;
                        dbContextAccess.productpurchaseorder.Update(productPurchase);
                        dbContextAccess.SaveChanges();
                    } else
                    {
                        data.PurchaseOrderId = purchaseOrder.Id;
                        productPurchaseOrderService.InsertProductPurchaseOrder(data);
                    }
                    
                });
                purchaseOrderIds.ForEach(id =>
                {
                    productPurchaseOrderService.DeleteProductPurchased(id);
                });
                purchaseOrder.Total = GetTotalAmount(id);
                dbContextAccess.PurchaseOrders.Update(purchaseOrder);
                await dbContextAccess.SaveChangesAsync();
            }
            return purchaseOrder;
        }
        
        public async Task<ActionResult<PurchaseOrder>> UpdateStatus(Guid id, int approverId)
        {
            var purchase = await dbContextAccess.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            if(purchase != null)
            {
                purchase.Status = "Approved";
                purchase.ApprovedBy = approverId;
                purchase.ApprovedDateTime = DateTime.Now.ToString();
                purchase.TrackingNumber = Guid.NewGuid().ToString();
                dbContextAccess.PurchaseOrders.Update(purchase); 
               await dbContextAccess.SaveChangesAsync();
                User user = dbContextAccess.Users.Find(purchase.CreatedBy);
                User approver = dbContextAccess.Users.Find(approverId);
                MailRequest mailRequest = new MailRequest();
                mailRequest.ToEmail = user.Email;
                mailRequest.Subject = "Regarding purchase order approval";
                mailRequest.Body = $"Your purchase order request is approved by {approver.Name})";
                mailService.SendEmailAsync(mailRequest);
            }
            return purchase;
        }

        public decimal GetCurrentMonthExpense()
        {
            decimal total = 0;
            var currentMonth = DateTime.Now.Month;
            var purchaseOrders = dbContextAccess.PurchaseOrders.ToList();
            purchaseOrders.ForEach(x =>
            {
                if(x.Status == "Approved" && x.IsActive && DateTime.Parse(x.ApprovedDateTime).Month == currentMonth)
                {
                    total += (decimal)x.Total;
                }
            });
            return total;
        }

        public async Task<ActionResult<decimal>> GetCurrentYearExpense()
        {
            decimal total = 0;
            var currentYear = DateTime.Now.Year;
            var purchaseOrders =await dbContextAccess.PurchaseOrders.ToListAsync();
            purchaseOrders.ForEach(x =>
            {
                if (x.Status == "Approved" && x.IsActive && DateTime.Parse(x.ApprovedDateTime).Year == currentYear)
                {
                    total += (decimal)x.Total;
                }
            });
            return total;
        }

        public async Task<ActionResult<List<decimal>>> GetListOfExpensesForMonth()
        {
            List<decimal> list = new List<decimal>();
            var purchaseOrders =await dbContextAccess.PurchaseOrders.ToListAsync();
            for (int i = 1; i <= 12;i++)
            {
                decimal total = 0; 
                purchaseOrders.ForEach(x =>
                {
                    if (x.Status == "Approved" && x.IsActive && DateTime.Parse(x.ApprovedDateTime).Month == i)
                    {
                        total += (decimal)x.Total;
                    }
                });
                list.Add(total);
            }
            return list;
        }

        public async Task<ActionResult<GetVendorsWithExpense>> GetAllExpensesByVendor()
        {
            List<string> vendors = new List<string>();
            List<Guid> vendorsIds = new List<Guid>();
            List<decimal> expenses = new List<decimal>();
            dbContextAccess.VendorDetails.ToList().ForEach(x =>
            {
                if (x.IsActive)
                {
                    vendors.Add(x.VendorName);
                    vendorsIds.Add(x.Id);
                }
            });
            vendorsIds.ForEach(id =>
            {
                decimal total = 0;
                var productPurchaseOrders = dbContextAccess.productpurchaseorder.Where(x => x.VendorId == id && x.IsActive).ToList();
                HashSet<Guid> guids = new HashSet<Guid>();
                productPurchaseOrders.ForEach(x =>
                {
                    guids.Add((Guid)x.PurchaseOrderId);
                });
                guids.ToList().ForEach(x =>
                {
                    var purchaseOrder = dbContextAccess.PurchaseOrders.Find(x);
                    if(purchaseOrder != null && purchaseOrder.IsActive && purchaseOrder.Status == "Approved")
                    {
                        total += (decimal)purchaseOrder.Total;
                    }
                });
                expenses.Add(total);

            });

            return new GetVendorsWithExpense
            {
                vendors = vendors,
                expenses = expenses
            };
        }

        public async Task<ActionResult<int>> GetCountOfAllPendingPurchaseOrders()
        {
            return await dbContextAccess.PurchaseOrders.Where(x => x.IsActive && x.Status == "Pending").CountAsync();
        }
    }
}