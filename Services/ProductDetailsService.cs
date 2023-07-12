using Microsoft.AspNetCore.Mvc;
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
    public class ProductDetailsService
    {



        private readonly DbContextAccess productdetailDBContext1;

        

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


                await productdetailDBContext1.productDetails.AddAsync(productdetail);
                await productdetailDBContext1.SaveChangesAsync();

                return Ok(productdetail);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
