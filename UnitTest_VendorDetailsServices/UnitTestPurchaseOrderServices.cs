using Microsoft.EntityFrameworkCore;
using Model.Requests;
using Model;
using Repository;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_VendorDetailsServices
{
    public class UnitTestPurchaseOrderServices
    {
        private readonly DbContextAccess dbContextAccess;
        public ProductDetailsService productDetailsService;
        public PurchaseOrderService purchaseOrderService;
        public ProductPurchaseOrderService productPurchaseOrderService;

        public UnitTestPurchaseOrderServices()
        {
            dbContextAccess = new DbContextAccess(new DbContextOptions<DbContextAccess>());
            productDetailsService = new ProductDetailsService(dbContextAccess);
            purchaseOrderService= new PurchaseOrderService(dbContextAccess);
            productPurchaseOrderService= new ProductPurchaseOrderService(dbContextAccess);
        }





        [Fact]
        public void InsertPurchaseOrder_InsertPurchaseOrderAndProducts()
        {
            // Arrange
            var purchaseOrderRequest = new PurchaseOrderRequest
            {
                UserId = new Guid("00000000-0000-0000-0000-000000000000"),
                BillingAddress = "123 Billing St",
                BillingAddressCity = "Billing City",
                BillingAddressState = "Billing State",
                BillingAddressCountry = "Billing Country",
                BillingAddressZipcode = "12345",
                ShippingAddress = "456 Shipping St",
                ShippingAddressCity = "Shipping City",
                ShippingAddressState = "Shipping State",
                ShippingAddressCountry = "Shipping Country",
                ShippingAddressZipcode = "54321",
                TermsAndConditions = "Terms and conditions text",
                Description = "Purchase order description",
                ProductsPurchased = new List<InsertProductPurchaseRequest>
            {
                new InsertProductPurchaseRequest
                {


                    VendorId= new Guid("CA449F57-739F-40F6-2F93-08DB841B8D4C"),
                    ProductId = new Guid("D4928EF4-4E11-4ACD-9BC8-08DB841B8D4D"),
                    Quantity = 10
                  }

            }
            };
            var result= purchaseOrderService.InsertPurchaseOrder(purchaseOrderRequest);
            Assert.NotNull(result);
            Assert.True(result.IsActive);
        }


            [Fact]
        public void PurchaseOrder_ReturnsCorrectData()
        {
            var result=purchaseOrderService.GetPurchasedOrders();
            Assert.NotNull(result);
        }
        [Fact]
        public void DeleteOrder_ValidPurchaseOrderId()
        {
            var PurchaseOrderId = new Guid("c0bf64b1-a4ba-42db-0f4f-08db8441ee81");
            var result = purchaseOrderService.DeletePurchaseOrder(PurchaseOrderId);
          
;            Assert.NotNull(result);
        }  






        // Negative Tests
        [Fact]
        public void DeleteOrder_InValidPurchaseOrderId()
        {
            var PurchaseOrderId = new Guid("d0bf64b1-a4ba-42db-0f4f-08db8441ee81");
            var result = purchaseOrderService.DeletePurchaseOrder(PurchaseOrderId);
            Assert.Null(result);
        }





    }
}
