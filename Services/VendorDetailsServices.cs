﻿using Microsoft.EntityFrameworkCore;
using Model;
using Model.Requests;
using Repository;


namespace Services
{
    public class VendorDetailsServices
    {
        private readonly DbContextAccess dbContextAccess;
        public ProductDetailsService productDetailsService;
        public VendorDetailsServices()
        {

        }
        public VendorDetailsServices(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
            productDetailsService = new ProductDetailsService(dbContextAccess);
        }

        public VendorDetails InsertVendorDetails(VendorDetailsRequest vendorDetailsRequest)
        {
            ProductDetail productDetail = new ProductDetail();
            VendorDetails vendorDetails = new VendorDetails();
            vendorDetails.Id = new Guid();
            vendorDetails.VendorName = vendorDetailsRequest.VendorName;
            vendorDetails.IsActive = true;
            vendorDetails.AddressLine1 = vendorDetailsRequest.AddressLine1;
            vendorDetails.AddressLine2 = vendorDetailsRequest.AddressLine2;
            vendorDetails.City = vendorDetailsRequest.City;
            vendorDetails.State = vendorDetailsRequest.State;
            vendorDetails.PostalCode = vendorDetailsRequest.PostalCode;
            vendorDetails.Country = vendorDetailsRequest.Country;
            vendorDetails.TelePhone1 = vendorDetailsRequest.TelePhone1;
            vendorDetails.TelePhone2 = vendorDetailsRequest.TelePhone2;
            vendorDetails.VendorEmail = vendorDetailsRequest.VendorEmail;
            vendorDetails.VendorWebsite = vendorDetailsRequest.VendorWebsite;
            dbContextAccess.VendorDetails.Add(vendorDetails);
            dbContextAccess.SaveChanges();

            vendorDetailsRequest.ProductDetailsRequest.ForEach(product =>
            {
                product.VendorId = vendorDetails.Id;
                productDetailsService.InsertProductDetail(product);

            });
            return vendorDetails;

        }

        public List<VendorDetailswithProductDetailsRequest> GetVendorDetails()
        {
            List<VendorDetailswithProductDetailsRequest> vendorDetailswithProductDetailsRequest = new List<VendorDetailswithProductDetailsRequest>();
            vendorDetailswithProductDetailsRequest = dbContextAccess.VendorDetails
                .Select(vendor => new VendorDetailswithProductDetailsRequest
                {
                    VendorDetails = vendor,
                    ProductDetails = dbContextAccess.productDetails.Where(p => p.VendorId == vendor.Id && p.IsActive).ToList()
                })
                .ToList();
            vendorDetailswithProductDetailsRequest = vendorDetailswithProductDetailsRequest.Where(p => p.VendorDetails.IsActive).ToList();
            vendorDetailswithProductDetailsRequest.ForEach(product =>
            {
                product.ProductDetails.ForEach(productDetails =>
                {
                    productDetails.VendorDetails = null;
                });
            });
            return vendorDetailswithProductDetailsRequest;
        }

        public VendorDetails DeleteVendor(Guid id)
        {
            var vendor = dbContextAccess.VendorDetails.FirstOrDefault(p => p.Id == id);
            if (vendor != null)
            {
                vendor.IsActive = false;
                dbContextAccess.VendorDetails.Update(vendor);
                dbContextAccess.SaveChanges(); 
            }
            return vendor;
        }

        public VendorDetailswithProductDetailsRequest GetVendor(Guid id)
        {
            var vendorWithProductDetails = dbContextAccess.VendorDetails
                .Where(v => v.Id == id && v.IsActive)
                .Select(vendor => new VendorDetailswithProductDetailsRequest
                {
                    VendorDetails = vendor,
                    ProductDetails = dbContextAccess.productDetails.Where(p => p.VendorId == vendor.Id && p.IsActive).ToList()
                })
                .SingleOrDefault();
                vendorWithProductDetails.ProductDetails.ForEach(productDetails =>
                {
                    productDetails.VendorDetails = null;
                });
            return vendorWithProductDetails;
        }

        public VendorDetailswithProductDetailsRequest UpdateVendor(Guid id, VendorDetailsUpdateRequest vendorDetailsUpdateRequest)
        {
            VendorDetails vendorDetails = dbContextAccess.VendorDetails.FirstOrDefault(v => v.Id == id && v.IsActive);
            if(vendorDetails != null)
            {
                vendorDetails.VendorName = vendorDetailsUpdateRequest.VendorName;
                vendorDetails.AddressLine1 = vendorDetailsUpdateRequest.AddressLine1;
                vendorDetails.AddressLine2 = vendorDetailsUpdateRequest.AddressLine2;
                vendorDetails.City = vendorDetailsUpdateRequest.City;
                vendorDetails.State = vendorDetailsUpdateRequest.State;
                vendorDetails.PostalCode = vendorDetailsUpdateRequest.PostalCode;
                vendorDetails.Country = vendorDetailsUpdateRequest.Country;
                vendorDetails.TelePhone1 = vendorDetailsUpdateRequest.TelePhone1;
                vendorDetails.TelePhone2 = vendorDetailsUpdateRequest.TelePhone2;
                vendorDetails.VendorEmail = vendorDetailsUpdateRequest.VendorEmail;
                vendorDetails.VendorWebsite = vendorDetailsUpdateRequest.VendorWebsite;
                dbContextAccess.VendorDetails.Update(vendorDetails);
                dbContextAccess.SaveChanges();

                List<Guid> ids = new List<Guid>();
                dbContextAccess.productDetails.ToList().ForEach(prod =>
                {
                    if(prod.VendorId == id)
                    {
                        ids.Add(prod.Id);
                    }
                });

                vendorDetailsUpdateRequest.ProductDetailsRequest.ForEach(product =>
                {
                    var prod = dbContextAccess.productDetails.Where(p => p.Id  == product.Id).FirstOrDefault();
                    if(prod != null)
                    {
                        ids.Remove(prod.Id);
                        //UpdateProductDetailRequest updateProductDetailRequest = new UpdateProductDetailRequest();
                        //updateProductDetailRequest.Id = prod.Id;
                        //updateProductDetailRequest.ProductName = prod.ProductName;
                        //updateProductDetailRequest.ProductDescription = prod.ProductDescription;
                        //updateProductDetailRequest.Price = prod.Price;
                        //updateProductDetailRequest.IsActive = prod.IsActive;
                        //productDetailsService.UpdateProductDetail(id,updateProductDetailRequest);
                        prod.Price = product.Price;
                        prod.ProductName = product.ProductName;
                        prod.ProductDescription = product.ProductDescription;
                        dbContextAccess.productDetails.Update(prod); 
                        dbContextAccess.SaveChanges();

                    } else
                    {
                        InsertProductDetailRequest insertProductDetailRequest = new InsertProductDetailRequest();
                        insertProductDetailRequest.VendorId = id;
                        insertProductDetailRequest.ProductName = product.ProductName;
                        insertProductDetailRequest.ProductDescription = product.ProductDescription;
                        insertProductDetailRequest.Price = product.Price;
                        productDetailsService.InsertProductDetail(insertProductDetailRequest);
                    }
                });

                ids.ForEach(id =>
                {
                    productDetailsService.DeleteProductDetail(id);
                });

               
            }
            return GetVendor(id);
        }
    }


}
