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
        public PurchaseOrderService purchaseOrderService;

        public PurchaseOrderController(DbContextAccess dbContextAccess)
        {
            purchaseOrderService = new PurchaseOrderService(dbContextAccess);
        }

        [HttpPost]
        public async Task<IActionResult> addPurchaseOrder(PurchaseOrderRequest purchaseOrderRequest)
        {
            if(purchaseOrderRequest != null && purchaseOrderRequest.ProductsPurchased.Count() > 0)
            {
                return Ok( purchaseOrderService.InsertPurchaseOrder(purchaseOrderRequest));
            } else { return BadRequest("Invalid Purchase Order requested"); }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetPurchasedOrders()
        //{
        //    return Ok(purchaseOrderService.GetPurchaseOrder());

        //}

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePurchaseOrder([FromRoute] Guid id)
        {
            var purchaseOrder = purchaseOrderService.DeletePurchaseOrder(id);
            if (purchaseOrder != null)
            {
                return Ok(purchaseOrder);
            }
            return NotFound("Purchase order not found");
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePurchaseOrder([FromRoute] Guid id,PurchaseOrderRequest purchaseOrderRequest)
        {
            if(purchaseOrderRequest != null && purchaseOrderRequest.ProductsPurchased.Count() > 0)
            {
                return Ok(purchaseOrderService.updatePurchaseOrder(id,purchaseOrderRequest));
            }
            return BadRequest("Invalid purchase order");
        }

        [HttpPut]
        [Route("/status/{id:guid}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id)
        {
            var purchaseOrder = purchaseOrderService.UpdateStatus(id);
            if (purchaseOrder != null)
            {
                return Ok(purchaseOrder);
            }
            return NotFound("Purchase order not found");
        }
    }
}