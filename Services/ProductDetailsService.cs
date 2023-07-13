
using Model.Requests;
using Model;
using Repository;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class ProductDetailsService
    {

        private readonly DbContextAccess dbContextAccess;
        public ProductDetailsService() { }
        
        public ProductDetailsService(DbContextAccess dbContextAccess) { 
            this.dbContextAccess = dbContextAccess;
        }

        public void InsertProductDetail(InsertProductDetailRequest insertProductDetailRequest)
        {
                ProductDetail productdetail = new ProductDetail();

                productdetail.Id = new Guid();
                productdetail.VendorId = (Guid)insertProductDetailRequest.VendorId;
                productdetail.ProductName = insertProductDetailRequest.ProductName;
                productdetail.Price = insertProductDetailRequest.Price;
                productdetail.ProductDescription = insertProductDetailRequest.ProductDescription;
                productdetail.IsActive = true;

                dbContextAccess.productDetails.Add(productdetail);
                dbContextAccess.SaveChanges();
        }

        public void UpdateProductDetail(Guid vendorId,UpdateProductDetailRequest updateProductDetailRequest)
        {
            //ProductDetail productdetail = new ProductDetail();
            //productdetail.Id = updateProductDetailRequest.Id;
            //productdetail.ProductName= updateProductDetailRequest.ProductName;
            //productdetail.ProductDescription= updateProductDetailRequest.ProductDescription;
            //productdetail.Price= updateProductDetailRequest.Price;
            //productdetail.VendorId = vendorId;
            var product = dbContextAccess.productDetails.Where(x => x.Id == updateProductDetailRequest.Id).FirstOrDefault();
            //dbContextAccess.productDetails.Update(productdetail);
            dbContextAccess.SaveChanges();
        }

        public void DeleteProductDetail(Guid id)
        {
            var productDetail = dbContextAccess.productDetails.FirstOrDefault(x => x.Id == id);
            if(productDetail != null) {
                productDetail.IsActive = false;
                dbContextAccess.productDetails.Update(productDetail);
                dbContextAccess.SaveChanges();
            }
        }

        //            return Ok(productdetail);
        //        }
        //    }
        //}
    }
}
