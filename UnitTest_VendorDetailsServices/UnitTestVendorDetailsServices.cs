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

        //Positive Tests

        [Fact]
        public void InsertVendorDetails_ValidRequest_ReturnsVendorDetails()
        {
            var vendorDetailsRequest = new VendorDetailsRequest
            {
                VendorName = "Test Vendor",
                AddressLine1 = "123 Test Street",
                City = "Test City",
                State = "Test State",
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

            var result = vendorDetailsServices.InsertVendorDetails(vendorDetailsRequest);

            
            Assert.NotEqual(default, result.Id);
            Assert.Equal(vendorDetailsRequest.VendorName, result.VendorName);
            Assert.True(result.IsActive);
            Assert.Equal(vendorDetailsRequest.AddressLine1, result.AddressLine1);
            Assert.Equal(vendorDetailsRequest.AddressLine2, result.AddressLine2);
            Assert.Equal(vendorDetailsRequest.City, result.City);
            Assert.Equal(vendorDetailsRequest.State, result.State);
            Assert.Equal(vendorDetailsRequest.PostalCode, result.PostalCode);
            Assert.Equal(vendorDetailsRequest.Country, result.Country);
            Assert.Equal(vendorDetailsRequest.TelePhone1, result.TelePhone1);
            Assert.Equal(vendorDetailsRequest.TelePhone2, result.TelePhone2);
            Assert.Equal(vendorDetailsRequest.VendorEmail, result.VendorEmail);
            Assert.Equal(vendorDetailsRequest.VendorWebsite, result.VendorWebsite);
            vendorDetailsServices.DeleteTestVendor(result.Id);
        }
       
        [Fact]
        public void GetVendorDetails_ReturnsCorrectData()
        {
                 var result = vendorDetailsServices.GetVendorDetails();
                 Assert.NotNull(result);
               //  Assert.Equal(10, result.Count);
          }


        [Fact]
        public void DeleteVendor_ReturnsDeletedVendor()
        {
           

            var id = new Guid("1E69902F-90D2-4811-D25B-08DB841932FD");
           
                var deletedVendor = vendorDetailsServices.DeleteVendor(id);
                Assert.NotNull(deletedVendor);
                Assert.Equal(id, deletedVendor.Id);
                Assert.False(deletedVendor.IsActive);

            

           
        }

        [Fact]
        public void GetVendorById_ReturnsCorrectData()
        {
            var vendorId = new Guid("CA449F57-739F-40F6-2F93-08DB841B8D4C");
            var result = vendorDetailsServices.GetVendor(vendorId);
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateVendor_ReturnsCorrectData()
        {
            Guid VendorId = new Guid("2D0F1C52-1B3E-4C06-D25C-08DB841932FD");
                var vendorDetailsUpdateRequest = new VendorDetailsUpdateRequest
            {
                VendorName = "Test Update Vendor",
                AddressLine1 = "123 Update Test Street",
                City = "Update Test City",
                State = "Update Test State",
                PostalCode = "Update 12345",
                Country = "Update Test Country",
                TelePhone1 = "123-456-7890",
                VendorEmail = "Updatetest@example.com",
                VendorWebsite = "https://www.testvendor.com",
                ProductDetailsRequest = new List<UpdateProductDetailRequest>
                {
                    new UpdateProductDetailRequest
                    {
                        
                        Id=new Guid("D8953783-2B88-4BBC-1B5A-08DB84193410"),
                         ProductName = "UpdateChocolate",
                         ProductDescription = "ChocolateProduct",
                         Price = 110,
                    },
                     new UpdateProductDetailRequest
                    {

                        Id=new Guid("A70D1D3F-E730-41CF-1B5B-08DB84193410"),
                         ProductName = "UpdateCake",
                         ProductDescription = "CakeProduct",
                         Price = 110,
                    },
                    new UpdateProductDetailRequest { 
                        Id=new Guid("00000000-0000-0000-0000-000000000000"),
                        ProductName = "UpdateMilk",
                        ProductDescription = "MilkProduct",
                        Price = 110,

                    }
                }

               
        };
            var result = vendorDetailsServices.UpdateVendor(VendorId,vendorDetailsUpdateRequest);
            Assert.NotNull(result);
            Assert.NotNull(result.ProductDetails);
            Assert.NotNull(result.VendorDetails);
            Assert.Equal(VendorId,result.VendorDetails.Id);
            Assert.Equal(vendorDetailsUpdateRequest.VendorName, result.VendorDetails.VendorName);
            Assert.Equal(vendorDetailsUpdateRequest.State, result.VendorDetails.State);
            Assert.Equal(vendorDetailsUpdateRequest.Country, result.VendorDetails.Country);
            Assert.Equal(vendorDetailsUpdateRequest.City, result.VendorDetails.City);
            Assert.Equal(vendorDetailsUpdateRequest.PostalCode, result.VendorDetails.PostalCode);
            Assert.Equal(vendorDetailsUpdateRequest.AddressLine1, result.VendorDetails.AddressLine1);
            Assert.Equal(vendorDetailsUpdateRequest.VendorEmail,result.VendorDetails.VendorEmail);
            Assert.Equal(vendorDetailsUpdateRequest.VendorWebsite, result.VendorDetails.VendorWebsite);
            

        }


        //NegativeTests

       

        [Fact]
        public void GetVendorById_Invalid()
        {
          
            // Invalid Vendor Id
            var vendorId = new Guid("CD449F57-739F-40F6-2F93-08DB841B8D4C");
            var result = vendorDetailsServices.GetVendor(vendorId);
            Assert.Null(result);
        }

        [Fact]
        public void UpdateVendor_InvalidVendorIdInput()
        {
            Guid VendorId = new Guid("1D0F1C52-1B3E-4C06-D25C-08DB841932FD"); //Invalid Vendor Id
            var vendorDetailsUpdateRequest = new VendorDetailsUpdateRequest
            {
                VendorName = "Test Update Vendor",
                AddressLine1 = "123 Update Test Street",
                City = "Update Test City",
                State = "Update Test State",
                PostalCode = "Update 12345",
                Country = "Update Test Country",
                TelePhone1 = "123-456-7890",
                VendorEmail = "Updatetest@example.com",
                VendorWebsite = "https://www.testvendor.com",
                ProductDetailsRequest = new List<UpdateProductDetailRequest>
                {
                    new UpdateProductDetailRequest
                    {

                        Id=new Guid("D8953783-2B88-4BBC-1B5A-08DB84193410"),
                         ProductName = "UpdateChocolate",
                         ProductDescription = "ChocolateProduct",
                         Price = 110,
                    },
                     new UpdateProductDetailRequest
                    {

                        Id=new Guid("A70D1D3F-E730-41CF-1B5B-08DB84193410"),
                         ProductName = "UpdateCake",
                         ProductDescription = "CakeProduct",
                         Price = 110,
                    },
                    new UpdateProductDetailRequest {
                        Id=new Guid("00000000-0000-0000-0000-000000000000"),
                        ProductName = "UpdateMilk",
                        ProductDescription = "MilkProduct",
                        Price = 110,

                    }
                }


            };
            var result = vendorDetailsServices.UpdateVendor(VendorId, vendorDetailsUpdateRequest);
            Assert.Null(result);
           
        }

        [Fact]
        public void DeleteVendor_InvalidVendorId()
        {
               var Id = new Guid("2E69902F-90D2-4811-D25B-08DB841932FD");
            
                var deletedVendor = vendorDetailsServices.DeleteVendor(Id);
                Assert.Null(deletedVendor);
               // Assert.False(deletedVendor.IsActive);

        }


        
    }
}
