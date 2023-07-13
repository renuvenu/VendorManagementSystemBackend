using Model.Requests;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Services
{
    public class ProductPurchaseOrderService
    {
        public readonly DbContextAccess dbContextAccess;

        public ProductPurchaseOrderService(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
        }

        public ProductPurchaseOrderService() { }

        public void InsertProductPurchaseOrder(InsertProductPurchaseRequest insertProductPurchaseRequest)
        {

            if (insertProductPurchaseRequest != null)
            {
                ProductPurchaseOrder productpurchasedetail = new ProductPurchaseOrder();

                productpurchasedetail.Id = new Guid();
                productpurchasedetail.VendorId = insertProductPurchaseRequest.VendorId;
                productpurchasedetail.PurchaseOrderId = insertProductPurchaseRequest.PurchaseOrderId;
                productpurchasedetail.ProductId = insertProductPurchaseRequest.ProductId;
                productpurchasedetail.Quantity = insertProductPurchaseRequest.Quantity;
                productpurchasedetail.IsActive = true;
                dbContextAccess.productpurchaseorder.Add(productpurchasedetail);
                dbContextAccess.SaveChanges();
            }
        }


        public void DeleteProductPurchased(Guid productpurchaseid)
        {
            var productPurchaseOrder = dbContextAccess.productpurchaseorder.FirstOrDefault(x => x.Id == productpurchaseid);
            if (productPurchaseOrder != null)
            {
                productPurchaseOrder.IsActive = false;
                dbContextAccess.productpurchaseorder.Update(productPurchaseOrder); 
                dbContextAccess.SaveChanges();
            }
        }
    }
}
