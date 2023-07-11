using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Requests;
using Repository;

namespace VendorManagement_WebApi.Controllers
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
                return Ok(await productdetailDBContext1.productDetails.ToListAsync());
            }

            [HttpPost]
            public async void InsertProductDetail(InsertProductDetailRequest insertProductDetailRequest)
            {

                if (insertProductDetailRequest != null)
                {
                    ProductDetail productdetail = new ProductDetail();

                    productdetail.Id = new Guid();
                    productdetail.VendorId = insertProductDetailRequest.VendorId;
                    productdetail.ProductName = insertProductDetailRequest.ProductName;
                    productdetail.Price = insertProductDetailRequest.Price;
                    productdetail.ProductDescription = insertProductDetailRequest.ProductDescription;


                    await productdetailDBContext1.productDetails.AddAsync(productdetail);
                    await productdetailDBContext1.SaveChangesAsync();

                    //return Ok(productdetail);
                }
            }

            [HttpPut]

            [Route("{Id:guid}")]
            public async Task<IActionResult> UpdatePersonDetail([FromRoute] Guid Id, UpdateProductDetailRequest updateProductDetailRequest)
            {


                if (updateProductDetailRequest != null)
                {
                    var productdetailResult = await productdetailDBContext1.productDetails.FirstOrDefaultAsync(x => x.Id.Equals(Id));

                    if (productdetailResult != null)
                    {

                        productdetailResult.ProductName = updateProductDetailRequest.ProductName;
                        productdetailResult.Price = updateProductDetailRequest.Price;
                        productdetailResult.ProductDescription = updateProductDetailRequest.ProductDescription;

                        productdetailDBContext1.productDetails.Update(productdetailResult);

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
                return await Task.FromResult<IActionResult>(Ok(productdetailDBContext1.productDetails.Where(x => x.Id.Equals(Id))));
            }

            [HttpDelete]
            [Route("{Id:guid}")]
            public async Task<IActionResult> DeletePersonDetail([FromRoute] Guid Id)
            {
                var productdetailResult = productdetailDBContext1.productDetails.FirstOrDefault(x => x.Id.Equals(Id));

                if (productdetailResult != null)
                {
                    productdetailDBContext1.productDetails.Remove(productdetailResult);
                    await productdetailDBContext1.SaveChangesAsync();
                }

                return Ok(productdetailResult);
            }


        }
    }

