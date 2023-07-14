using Repository;
using Model;
using Model.Requests;

namespace Services
{
    public class PurchaseOrderService
    {
        private readonly DbContextAccess dbContextAccess;
        public ProductPurchaseOrderService productPurchaseOrderService;

        public PurchaseOrderService() { }
        public PurchaseOrderService(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
            productPurchaseOrderService = new ProductPurchaseOrderService(dbContextAccess);
        }

        public PurchaseOrder InsertPurchaseOrder(PurchaseOrderRequest purchaseOrderRequest)
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
            dbContextAccess.PurchaseOrders.Add(purchaseOrder);
            dbContextAccess.SaveChanges();
            purchaseOrderRequest.ProductsPurchased.ForEach(data =>
            {
                data.PurchaseOrderId = purchaseOrder.Id;
                productPurchaseOrderService.InsertProductPurchaseOrder(data);
            });
            purchaseOrder.Total = GetTotalAmount(purchaseOrder.Id);
            dbContextAccess.PurchaseOrders.Update(purchaseOrder);
            dbContextAccess.SaveChanges();

            return purchaseOrder;
        }

       public List<PurchaseOrderWithProductDetails> GetPurchasedOrders()
        {
            List<PurchaseOrderWithProductDetails> purchaseOrderWithProductDetails = new List<PurchaseOrderWithProductDetails>();
            purchaseOrderWithProductDetails = dbContextAccess.PurchaseOrders.Where(purchase => purchase.IsActive).ToList().Select(purchaseOrder => new PurchaseOrderWithProductDetails
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
            return dbContextAccess.VendorDetails.FirstOrDefault(vendor => vendor.Id == purchaseProduct.VendorId);
        }

        public PurchaseOrder DeletePurchaseOrder(Guid PurchaseOrderId)
        {
            var purchaseOrder = dbContextAccess.PurchaseOrders.FirstOrDefault(p => p.Id == PurchaseOrderId);
            if(purchaseOrder != null)
            {
                purchaseOrder.IsActive = false;
                dbContextAccess.PurchaseOrders.Update(purchaseOrder);
                dbContextAccess.SaveChanges();
            }
            return purchaseOrder;
        }

        public PurchaseOrder updatePurchaseOrder(Guid id, PurchaseOrderRequest purchaseOrderRequest)
        {
            PurchaseOrder purchaseOrder = dbContextAccess.PurchaseOrders.FirstOrDefault(e => e.Id == id);
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
                dbContextAccess.SaveChanges();
            }
            return purchaseOrder;
        }
        
        public PurchaseOrder UpdateStatus(Guid id, int approverId)
        {
            var purchase = dbContextAccess.PurchaseOrders.FirstOrDefault(x => x.Id == id && x.IsActive);
            if(purchase != null)
            {
                purchase.Status = "Approved";
                purchase.ApprovedBy = approverId;
                purchase.ApprovedDateTime = DateTime.Now.ToString();
                dbContextAccess.PurchaseOrders.Update(purchase); 
                dbContextAccess.SaveChanges(); 
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

        public decimal GetCurrentYearExpense()
        {
            decimal total = 0;
            var currentYear = DateTime.Now.Year;
            var purchaseOrders = dbContextAccess.PurchaseOrders.ToList();
            purchaseOrders.ForEach(x =>
            {
                if (x.Status == "Approved" && x.IsActive && DateTime.Parse(x.ApprovedDateTime).Year == currentYear)
                {
                    total += (decimal)x.Total;
                }
            });
            return total;
        }

        public List<decimal> GetListOfExpensesForMonth()
        {
            List<decimal> list = new List<decimal>();
            var purchaseOrders = dbContextAccess.PurchaseOrders.ToList();
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

        public GetVendorsWithExpense GetAllExpensesByVendor()
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

        public int GetCountOfAllPendingPurchaseOrders()
        {
            return dbContextAccess.PurchaseOrders.Where(x => x.IsActive && x.Status == "Pending").Count();
        }
    }
}