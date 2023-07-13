
using Model.Requests;
using Model;
using Repository;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public interface InterfaceProductDetailService
    {
        void InsertProductDetail(InsertProductDetailRequest insertProductDetailRequest);
        void UpdateProductDetail(Guid vendorId, UpdateProductDetailRequest updateProductDetailRequest);

        void DeleteProductDetail(Guid id);


    }
    public class ProductDetailsService : InterfaceProductDetailService
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
            
            var product = dbContextAccess.productDetails.Where(x => x.Id == updateProductDetailRequest.Id).FirstOrDefault();
           
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
    }
}
