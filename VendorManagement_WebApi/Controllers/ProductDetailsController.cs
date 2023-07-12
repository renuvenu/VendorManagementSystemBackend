using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Requests;
using Repository;
using Services;

namespace VendorManagement_WebApi.Controllers
{
    
        [ApiController]
        [Route("api/[Controller]")]
        public class ProductDetailsController : Controller
        {
            public ProductDetailsService productDetailsService;

            public ProductDetailsController()
            {
                productDetailsService = new ProductDetailsService();
            }
        //[HttpGet]

        //public async Task<IActionResult> GetProductDetail()
        //{
        //    return Ok(await productdetailDBContext1.productDetails.ToListAsync());
        //}

        //[HttpPost]
        //public async Task<IActionResult> InsertProductDetail(InsertProductDetailRequest insertProductDetailRequest)
        //{

        //    if (insertProductDetailRequest != null)
        //    {
        //        return Ok(productDetailsService.InsertProductDetail(insertProductDetailRequest));
        //        }
        //    else
        //    {
        //        return BadRequest("Invalid product details");
        //    }
        //}

        //[HttpPut]


        //public async Task<IActionResult> UpdateProductDetail(Guid Id, InsertProductDetailRequest updateProductDetailRequest)
        //{


        //    if (updateProductDetailRequest != null)
        //    {
        //        var productdetailResult = await productdetailDBContext1.productDetails.FirstOrDefaultAsync(x => x.VendorId.Equals(Id));

        //        if (productdetailResult != null)
        //        {

        //            productdetailResult.ProductName = updateProductDetailRequest.ProductName;
        //            productdetailResult.Price = updateProductDetailRequest.Price;
        //            productdetailResult.ProductDescription = updateProductDetailRequest.ProductDescription;

        //            productdetailDBContext1.productDetails.Update(productdetailResult);

        //            await productdetailDBContext1.SaveChangesAsync();
        //        }

        //        return Ok(productdetailResult);
        //    }
        //    else
        //    {
        //        return BadRequest("Product is not available");
        //    }
        //}

        //[HttpGet]

        //[Route("{Id}")]
        //public async Task<IActionResult> GetElementById([FromRoute] Guid Id)
        //{
        //    return await Task.FromResult<IActionResult>(Ok(productdetailDBContext1.productDetails.Where(x => x.Id.Equals(Id))));
        //}

        //[HttpDelete]
        //[Route("{Id:guid}")]
        //public async Task<IActionResult> DeletePersonDetail([FromRoute] Guid Id)
        //{
        //    var productdetailResult = productdetailDBContext1.productDetails.FirstOrDefault(x => x.Id.Equals(Id));

        //    if (productdetailResult != null)
        //    {
        //        productdetailDBContext1.productDetails.Remove(productdetailResult);
        //        await productdetailDBContext1.SaveChangesAsync();
        //    }

        //    return Ok(productdetailResult);
        //}


    }
    }

