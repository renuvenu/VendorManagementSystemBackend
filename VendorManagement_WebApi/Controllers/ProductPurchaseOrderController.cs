﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Requests;
using Model;
using Repository;

namespace VendorManagement_WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductPurchaseOrderController : Controller
    {
        private readonly DbContextAccess productpurchasedetailDBContext1;

        public ProductPurchaseOrderController(DbContextAccess productpurchasedetailDBContext1)
        {
            this.productpurchasedetailDBContext1 = productpurchasedetailDBContext1;
        }

        [HttpGet]

        public async Task<IActionResult> GetProductPurchaseDetail()
        {
            return Ok(await productpurchasedetailDBContext1.productpurchaseorder.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> InsertProductPurchaseOrder(InsertProductPurchaseRequest insertProductPurchaseRequest)
        {

            if (insertProductPurchaseRequest != null)
            {
                ProductPurchaseOrder productpurchasedetail = new ProductPurchaseOrder();

                productpurchasedetail.Id = new Guid();
                productpurchasedetail.VendorId= insertProductPurchaseRequest.VendorId;
                productpurchasedetail.PurchaseOrderId = insertProductPurchaseRequest.PurchaseOrderId;
                productpurchasedetail.ProductId = insertProductPurchaseRequest.ProductId;
                productpurchasedetail.Quantity = insertProductPurchaseRequest.Quantity;

                await productpurchasedetailDBContext1.productpurchaseorder.AddAsync(productpurchasedetail);
                await productpurchasedetailDBContext1.SaveChangesAsync();

                return Ok(productpurchasedetail);
            }
            else
            {
                return BadRequest("Product Detail is not available");
            }
        }
        [HttpGet]

        [Route("{Id}")]
        public async Task<IActionResult> GetElementById([FromRoute] Guid Id)
        {
            return await Task.FromResult<IActionResult>(Ok(productpurchasedetailDBContext1.productpurchaseorder.Where(x => x.Id.Equals(Id))));
        }
    }
}
