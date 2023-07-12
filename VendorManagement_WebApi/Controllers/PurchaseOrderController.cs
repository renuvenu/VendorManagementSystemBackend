using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Requests;
using Repository;
using Microsoft.Identity.Client.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Services;

namespace VendorManagement_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseOrderController : Controller
    {
        private readonly DbContextAccess dbContextAccess;

        public PurchaseOrderController(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
        }

        [HttpPost]
        public async Task<IActionResult> addPurchaseOrder(PurchaseOrderRequest purchaseOrderRequest)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder.Id = new Guid();
            purchaseOrder.UserId = purchaseOrderRequest.UserId;
            purchaseOrder.BillingAddress = purchaseOrderRequest.BillingAddress;
            purchaseOrder.BillingAddressCity = purchaseOrderRequest.BillingAddressCity;
            purchaseOrder.BillingAddressState = purchaseOrderRequest.BillingAddressState;
            purchaseOrder.BillingAddressCountry = purchaseOrderRequest.BillingAddressCountry;
            purchaseOrder.BillingAddressZipcode =   purchaseOrderRequest.BillingAddressZipcode;
            purchaseOrder.ShippingAddress = purchaseOrderRequest.ShippingAddress;
            purchaseOrder.ShippingAddressCity = purchaseOrderRequest.ShippingAddressCity;
            purchaseOrder.ShippingAddressState = purchaseOrderRequest.ShippingAddressState;
            purchaseOrder.ShippingAddressCountry = purchaseOrderRequest.ShippingAddressCountry;
            purchaseOrder.ShippingAddressZipcode = purchaseOrderRequest.ShippingAddressZipcode;
            purchaseOrder.TermsAndConditions = purchaseOrderRequest.TermsAndConditions;
            purchaseOrder.Description = purchaseOrderRequest.Description;
            purchaseOrder.Status = "Pending";
            purchaseOrder.Total = 0;
            await dbContextAccess.PurchaseOrders.AddAsync(purchaseOrder);
            await dbContextAccess.SaveChangesAsync();
            ProductPurchaseOrderController productPurchaseOrderController = new ProductPurchaseOrderController(dbContextAccess);
            purchaseOrderRequest.ProductsPurchased.ForEach(data =>
            {
                data.PurchaseOrderId = purchaseOrder.Id;
                productPurchaseOrderController.InsertProductPurchaseOrder(data);
            });
            return Ok(purchaseOrder);
        }

        [HttpGet]
        public async Task<IActionResult> getProductOrderDetails()
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>(await dbContextAccess.PurchaseOrders.ToListAsync());
            var res = from purchaseorder in purchaseOrders
                      join ppo in dbContextAccess.productpurchaseorder on purchaseorder.Id equals ppo.PurchaseOrderId into productpurchasedetails
                      //from vendor in dbcontextaccess.vendordetails where vendor.id == matchedproduct.elementat(0).vendorid
                      select new
                      {
                          purchaseorder,
                          productpurchasedetails
                      };

            var res2 = res.ToList();
            return Ok(res);
            //purchaseOrders.ForEach(async purchaseOrder =>
            //{
            //    //List<ProductPurchaseOrder> productPurchaseOrders = new List<ProductPurchaseOrder>(await dbContextAccess.productpurchaseorder.Where(data => data.PurchaseOrderId == purchaseOrder.Id).ToListAsync());
            //    //productPurchaseOrders.ForEach(orders => )
            //    //List<ProductPurchaseOrder> productPurchaseOrder = dbContextAccess.productpurchaseorder.AllAsync(data => data.PurchaseOrderId == purchaseOrder.Id);
            //    var result = from productPurchaseOrder in productPurchaseOrders join ppo in dbContextAccess.productpurchaseorder on productPurchaseOrder.ProductId equals ppo.ProductId
            //});
            //return Ok(await dbContextAccess.PurchaseOrders.ToListAsync());
        }
    }
}