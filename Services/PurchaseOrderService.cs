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
            purchaseOrder.UserId = purchaseOrderRequest.UserId;
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

        //public  List<PurchaseOrderWithProductDetails> GetPurchaseOrder()
        //{
        //    return PurchaseOrderWithProductDetails();
        //}

        public  decimal GetTotalAmount(Guid purchaseOrderId)
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

        //public List<PurchaseProductDetails> GetAllPurchaseProducts(Guid PurchaseOrderId)
        //{
        //    List<PurchaseProductDetails> purchaseProductDetailsList = new List<PurchaseProductDetails>();
        //    var purchaseProducts = dbContextAccess.productpurchaseorder.Where(p => p.PurchaseOrderId == PurchaseOrderId).ToList();
        //    purchaseProducts.ForEach(data =>
        //    {
        //        var product = dbContextAccess.productDetails.FirstOrDefault(product => product.Id == data.ProductId);
        //        PurchaseProductDetails purchaseProductDetails = new PurchaseProductDetails();
        //        purchaseProductDetails.ProductId = product.Id;
        //        purchaseProductDetails.ProductName = product.ProductName;
        //        purchaseProductDetails.ProductDescription = product.ProductDescription;
        //        purchaseProductDetails.Price = product.Price;
        //        purchaseProductDetails.Quantity = data.Quantity;
        //        purchaseProductDetailsList.Add(purchaseProductDetails);
        //    });
        //    return purchaseProductDetailsList;

        //}

        //public List<PurchaseOrderWithProductDetails>  PurchaseOrderWithProductDetails()
        //{
        //    List<PurchaseOrderWithProductDetails> purchaseOrderWithProductDetails = new List<PurchaseOrderWithProductDetails>();
        //    purchaseOrderWithProductDetails = dbContextAccess.PurchaseOrders.Select(purchase => new PurchaseOrderWithProductDetails
        //    {
        //        PurchaseOrders = purchase,
        //        PurchaseProducts = GetAllPurchaseProducts(purchase.Id),
        //        VendorDetails = GetVendorDetails(purchase.Id)
        //    }).ToList();
        //    return purchaseOrderWithProductDetails;
        //}


        //public VendorDetails GetVendorDetails(Guid PurchaseOrderId)
        //{
        //    var purchaseProduct = dbContextAccess.productpurchaseorder.FirstOrDefault(p => p.PurchaseOrderId == PurchaseOrderId);
        //    return dbContextAccess.VendorDetails.FirstOrDefault(vendor => vendor.Id == purchaseProduct.VendorId);
        //}

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
                purchaseOrder.UserId = purchaseOrderRequest.UserId;
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
        
        public PurchaseOrder UpdateStatus(Guid id)
        {
            var purchase = dbContextAccess.PurchaseOrders.FirstOrDefault(x => x.Id == id && x.IsActive);
            if(purchase != null)
            {
                purchase.Status = "Approved";
                dbContextAccess.PurchaseOrders.Update(purchase); 
                dbContextAccess.SaveChanges();
            }
            return purchase;
        }
    }
}