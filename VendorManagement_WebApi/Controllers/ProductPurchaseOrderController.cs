using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Requests;
using Model;
using Repository;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace VendorManagement_WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductPurchaseOrderController : Controller
    {
        public ProductPurchaseOrderService productPurchaseOrderService;

        public ProductPurchaseOrderController(DbContextAccess dbContextAccess)
        {
            productPurchaseOrderService = new ProductPurchaseOrderService(dbContextAccess);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Approver,User")]
        public void InsertProductPurchaseOrder(InsertProductPurchaseRequest insertProductPurchaseRequest)
        {
               productPurchaseOrderService.InsertProductPurchaseOrder(insertProductPurchaseRequest);
        }
    }
}
