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
                var purchaseOrder = await purchaseOrderService.InsertPurchaseOrder(purchaseOrderRequest);
                return Ok(purchaseOrder.Value);
            } else { return BadRequest("Invalid Purchase Order requested"); }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchasedOrders()
        {
            var purchaseOrders = await purchaseOrderService.GetPurchasedOrders();
            return Ok(purchaseOrders.Value);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePurchaseOrder([FromRoute] Guid id)
        {
            var purchaseOrder =await purchaseOrderService.DeletePurchaseOrder(id);
            if (purchaseOrder.Value != null && purchaseOrder.Value.Id == id)
            {
                return Ok(purchaseOrder.Value);
            }
            return NotFound("Purchase order not found");
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePurchaseOrder([FromRoute] Guid id,PurchaseOrderRequest purchaseOrderRequest)
        {
            if(purchaseOrderRequest != null && purchaseOrderRequest.ProductsPurchased.Count() > 0)
            {
                var purchaseOrder = await purchaseOrderService.updatePurchaseOrder(id, purchaseOrderRequest);
                return Ok(purchaseOrder.Value);
            }
            return BadRequest("Invalid purchase order");
        }

        [HttpPut]
        [Route("/status/{id:guid}/{approverId:int}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromRoute] int approverId)
        {
            var purchaseOrder = await purchaseOrderService.UpdateStatus(id, approverId);
            if (purchaseOrder.Value != null && purchaseOrder.Value.Id == id)
            {
                return Ok(purchaseOrder.Value);
            }
            return NotFound("Purchase order not found");
        }

        [HttpGet]
        [Route("get/currentMonth/expense")]
        public async Task<IActionResult> GetCurrentMonthExpense()
        {
            return Ok(purchaseOrderService.GetCurrentMonthExpense());
        }

        [HttpGet]
        [Route("get/currentYear/expense")]
        public async Task<IActionResult> GetCurrentYearExpense()
        {
            var count = await purchaseOrderService.GetCurrentYearExpense();
            return Ok(count.Value);
        }

        [HttpGet]
        [Route("get/currentYear/list-expenses")]
        public async Task<IActionResult> GetListOfExpensesForMonth()
        {
            var list = await purchaseOrderService.GetListOfExpensesForMonth();
            return Ok(list.Value);
        }

        [HttpGet]
        [Route("get/all-expenses/vendors")]
        public async Task<IActionResult> GetAllExpensesByVendor()
        {
            var list = await purchaseOrderService.GetAllExpensesByVendor();
            return Ok(list.Value);
        }

        [HttpGet]
        [Route("get/count/pending/purchase-orders")]
        public async Task<IActionResult> GetCountOfAllPendingPurchaseOrders()
        {
            var count = await purchaseOrderService.GetCountOfAllPendingPurchaseOrders();
            return Ok(count.Value);
        }

        [HttpGet]
        [Route("get/all/pending/purchase-orders")]
        public async Task<IActionResult> GetAllPendingPurchaseOrders()
        {
            var pendingPurchases = await purchaseOrderService.GetAllPendingPurchaseOrders();
            return Ok(pendingPurchases.Value);
        }

    }
}