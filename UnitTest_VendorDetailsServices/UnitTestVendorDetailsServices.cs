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

            // Assert
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
        }
       
        [Fact]
        public void GetVendorDetails_ReturnsCorrectData()
        {
                 var result = vendorDetailsServices.GetVendorDetails();
                 Assert.NotNull(result);
                 Assert.Equal(10, result.Count);
          }


        [Fact]
        public void DeleteVendor_ReturnsDeletedVendor()
        {
           

            var id = "F9472FA7-16BA-4120-40BB-08DB82BFB607";
            if (Guid.TryParse(id, out Guid vendorId))
            {
                var deletedVendor = vendorDetailsServices.DeleteVendor(vendorId);
                Assert.NotNull(deletedVendor);
                Assert.Equal(vendorId, deletedVendor.Id);
                Assert.False(deletedVendor.IsActive);

            }

            // Act
           
        }
    }
}
