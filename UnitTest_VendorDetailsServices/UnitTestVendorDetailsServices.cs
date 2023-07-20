using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Model;
using Model.Requests;
using Moq;
using Repository;
using Services;
using Xunit;

namespace UnitTest_VendorDetailsServices
{
    public class UnitTestVendorDetailsServices
    {
        private readonly DbContextAccess dbContextAccess;
        public ProductDetailsService productDetailsService;
        public VendorDetailsServices vendorDetailsServices;

        public UnitTestVendorDetailsServices()
        {
            dbContextAccess = new DbContextAccess(new DbContextOptions<DbContextAccess>());
            productDetailsService = new ProductDetailsService(dbContextAccess);
            vendorDetailsServices = new VendorDetailsServices(dbContextAccess)
            {
                productDetailsService = productDetailsService
            };
        }

        [Fact]
        public async Task InsertVendorDetails_ValidRequest_ReturnsVendorDetails()
        {

            
                var vendorDetailsRequest = new VendorDetailsRequest
            {
                VendorName = "Test Vendor",
                AddressLine1 = "123 Test Street",
                City = "Test City",
                State = "Test State",
                VendorType = "product",
                PostalCode = "12345",
                Country = "Test Country",
                TelePhone1 = "123-456-7890",
                VendorEmail = "test@example.com",
                VendorWebsite = "https://www.testvendor.com",
                ProductDetailsRequest = new List<InsertProductDetailRequest>
                {
                    new InsertProductDetailRequest
                    {
                        Price = 110,
                        ProductDescription = "xyzProduct",
                        ProductName = "XYZ"
                    }
                }
            };

            var result = await vendorDetailsServices.InsertVendorDetails(vendorDetailsRequest);


            Assert.NotEqual(default, result.Value.Id);
            Assert.Equal(result.Value.VendorName, result.Value.VendorName);
            Assert.True(result.Value.IsActive);
            await vendorDetailsServices.DeleteVendor_Test(result.Value.Id);


        }

        [Fact]
        public async Task GetVendorDetailandProductById_ReturnsCorrectData()
        {
            var insertVendor = await InsertVendorDetails_Test();
            var result = await vendorDetailsServices.GetVendor(insertVendor.Value.Id);
            Assert.NotNull(result.Value.VendorDetails);
            Assert.NotNull(result.Value.ProductDetails);

            Assert.Equal(insertVendor.Value.Id,result.Value.VendorDetails.Id);
            await vendorDetailsServices.DeleteVendor_Test(insertVendor.Value.Id);

        }


       
        [Fact]
        public async Task GetVendorDetails_ReturnsCorrectData()
        {
                    
                 
                 var result = await vendorDetailsServices.GetVendorDetails();
                 Assert.NotNull(result);
                 Assert.NotEmpty(result.Value);
                 

           
        }

        [Fact]
        public async Task CountOfVendors()
        {
            var insertVendor = await InsertVendorDetails_Test();
            var result= await dbContextAccess.VendorDetails.Where(x => x.IsActive).CountAsync();
            Assert.NotNull(result);
            Assert.True(result>0);
            await vendorDetailsServices.DeleteVendor_Test(insertVendor.Value.Id);
        }

        [Fact]
        public async Task DeleteVendor_ReturnsDeletedVendor()
        {


                var insertVendor = await InsertVendorDetails_Test();
           
                var deletedVendor =await vendorDetailsServices.DeleteVendor(insertVendor.Value.Id);
                
                Assert.Equal(insertVendor.Value.Id, deletedVendor.Value.Id);
                Assert.False(deletedVendor.Value.IsActive);
            await vendorDetailsServices.DeleteVendor_Test(insertVendor.Value.Id);

        }

        [Fact]
        public async Task UpdateVendorDetails()
        {
            var insertVendor = await InsertVendorDetails_Test();
            var vendorDetailsWithUpdateRequest = new VendorDetailsUpdateRequest
            {
                VendorName = "Test Vendor Update",
                AddressLine1 = "123 Test Street Update",
                City = "Test City Update",
                State = "Test State Update",
                VendorType = "product Update",
                PostalCode = "12345",
                Country = "Test Country Update",
                TelePhone1 = "1234567890",
                VendorEmail = "testupdate@example.com",
                VendorWebsite = "https://www.testvendor.com",
                ProductDetailsRequest = new List<UpdateProductDetailRequest>
                {
                    new UpdateProductDetailRequest
                    {
                        Id=Guid.Empty,
                        Price = 110,
                        ProductDescription = "abcProduct",
                        ProductName = "ABC"
                    }
                }
            };
            var result=await vendorDetailsServices.UpdateVendor(insertVendor.Value.Id, vendorDetailsWithUpdateRequest);
            Assert.NotNull(result);
            Assert.Equal(vendorDetailsWithUpdateRequest.VendorName, result.Value.VendorDetails.VendorName);


            await vendorDetailsServices.DeleteVendor_Test(insertVendor.Value.Id);


        }

        //Negative Test
        [Fact]
        public async Task DeleteInvalidVendor()
        {

            var deletedVendor = await vendorDetailsServices.DeleteVendor(Guid.Empty);
            Assert.Null(deletedVendor.Value);
        }

       

        [Fact]
        public async Task GetVendorDetailandProductById_InvalidId()
        {
            var result = await vendorDetailsServices.GetVendor(new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateVendorDetails_InvalidVendorId()
        {
            
            var vendorDetailsWithUpdateRequest = new VendorDetailsUpdateRequest
            {
                VendorName = "Test Vendor Update",
                AddressLine1 = "123 Test Street Update",
                City = "Test City Update",
                State = "Test State Update",
                VendorType = "product Update",
                PostalCode = "12345",
                Country = "Test Country Update",
                TelePhone1 = "1234567890",
                VendorEmail = "testupdate@example.com",
                VendorWebsite = "https://www.testvendor.com",
                ProductDetailsRequest = new List<UpdateProductDetailRequest>
                {
                    new UpdateProductDetailRequest
                    {
                        Id=Guid.Empty,
                        Price = 110,
                        ProductDescription = "abcProduct",
                        ProductName = "ABC"
                    }
                }
            };
            var result = await vendorDetailsServices.UpdateVendor(Guid.Empty, vendorDetailsWithUpdateRequest);
            Assert.Null(result.Value);
        }

        // Method to Insert Test Data
        public async Task<ActionResult<VendorDetails>> InsertVendorDetails_Test()
        {
            var vendorDetailsRequest = new VendorDetailsRequest
            {
                VendorName = "Test Vendor",
                AddressLine1 = "123 Test Street",
                City = "Test City",
                State = "Test State",
                VendorType = "product",
                PostalCode = "12345",
                Country = "Test Country",
                TelePhone1 = "123-456-7890",
                VendorEmail = "test@example.com",
                VendorWebsite = "https://www.testvendor.com",
                ProductDetailsRequest = new List<InsertProductDetailRequest>
                {
                    new InsertProductDetailRequest
                    {
                        Price = 110,
                        ProductDescription = "xyzProduct",
                        ProductName = "XYZ"
                    }
                }
            };

            var result = await vendorDetailsServices.InsertVendorDetails(vendorDetailsRequest);
            return result;
        }
    }
}
