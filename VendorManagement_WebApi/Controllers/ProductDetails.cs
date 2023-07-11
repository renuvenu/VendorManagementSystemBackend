using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Requests;
using Repository;

namespace VendorManagement_WebApi.Controllers
{
    public class ProductDetails : Controller
    {
        [ApiController]
        [Route("api/[Controller]")]
        public class ProductDetailsController : Controller
        {
            private readonly DbContextAccess productdetailDBContext1;

            public ProductDetailsController(DbContextAccess productdetailDBContext1)
            {
                this.productdetailDBContext1 = productdetailDBContext1;
            }
            [HttpGet]

            public async Task<IActionResult> GetProductDetail()
            {
                return Ok(await productdetailDBContext1.ProductDetails.ToListAsync());
            }

            [HttpPost]
            public async Task<IActionResult> InsertProductDetail(InsertProductDetailRequest insertProductDetailRequest)
            {

                if (insertProductDetailRequest != null)
                {
                    ProductDetail productdetail = new ProductDetail();

                    productdetail.Id = new Guid();
                    productdetail.VendorId = insertProductDetailRequest.VendorId;
                    productdetail.ProductName = insertProductDetailRequest.ProductName;
                    productdetail.Price = insertProductDetailRequest.Price;
                    productdetail.ProductDescription = insertProductDetailRequest.ProductDescription;


                    await productdetailDBContext1.ProductDetails.AddAsync(productdetail);
                    await productdetailDBContext1.SaveChangesAsync();

                    return Ok(productdetail);
                }
                else
                {
                    return BadRequest("Product Detail is not available");
                }
            }

            [HttpPut]

            [Route("{Id:guid}")]
            public async Task<IActionResult> UpdatePersonDetail([FromRoute] Guid Id, UpdateProductDetailRequest updateProductDetailRequest)
            {


                if (updateProductDetailRequest != null)
                {
                    var productdetailResult = await productdetailDBContext1.ProductDetails.FirstOrDefaultAsync(x => x.Id.Equals(Id));

                    if (productdetailResult != null)
                    {

                        productdetailResult.ProductName = updateProductDetailRequest.ProductName;
                        productdetailResult.Price = updateProductDetailRequest.Price;
                        productdetailResult.ProductDescription = updateProductDetailRequest.ProductDescription;

                        productdetailDBContext1.ProductDetails.Update(productdetailResult);

                        await productdetailDBContext1.SaveChangesAsync();
                    }

                    return Ok(productdetailResult);
                }
                else
                {
                    return BadRequest("Product is not available");
                }
            }

            [HttpGet]

            [Route("{Id}")]
            public async Task<IActionResult> GetElementById([FromRoute] Guid Id)
            {
                return await Task.FromResult<IActionResult>(Ok(productdetailDBContext1.ProductDetails.Where(x => x.Id.Equals(Id))));
            }

            [HttpDelete]
            [Route("{Id:guid}")]
            public async Task<IActionResult> DeletePersonDetail([FromRoute] Guid Id)
            {
                var productdetailResult = productdetailDBContext1.ProductDetails.FirstOrDefault(x => x.Id.Equals(Id));

                if (productdetailResult != null)
                {
                    productdetailDBContext1.ProductDetails.Remove(productdetailResult);
                    await productdetailDBContext1.SaveChangesAsync();
                }

                return Ok(productdetailResult);
            }


        }
    }
}
