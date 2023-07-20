using Repository;
using Model;
using Model.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services;

namespace UnitTest_VendorDetailsServices
{
    public class UnitTestPurchaseOrderServices
    {
        private readonly DbContextAccess dbContextAccess;
        public ProductDetailsService productDetailsService;
        public PurchaseOrderService purchaseOrderService;
        public ProductPurchaseOrderService productPurchaseOrderService;
        public UnitTestVendorDetailsServices unitTestVendorDetailsService;
        public VendorDetailsServices vendorDetailsServices;
        public MailService mailService;
        public IOptions<MailSettings> mailSettings;


        public UnitTestPurchaseOrderServices()
        {

            this.mailSettings = mailSettings;
            dbContextAccess = new DbContextAccess(new DbContextOptions<DbContextAccess>());
            productDetailsService = new ProductDetailsService(dbContextAccess);
            purchaseOrderService = new PurchaseOrderService(dbContextAccess);
            vendorDetailsServices = new VendorDetailsServices(dbContextAccess);
            productPurchaseOrderService = new ProductPurchaseOrderService(dbContextAccess);
            unitTestVendorDetailsService = new UnitTestVendorDetailsServices();
            
        }


        //[Fact]
        //public async Task InsertPurchaseOrder_InsertPurchaseOrderAndProducts()
        //{

        //    var insertVendor = await unitTestVendorDetailsService.InsertVendorDetails_Test();
        //    var result = await vendorDetailsServices.GetVendor(insertVendor.Value.Id);
        //    var firstProduct = result.Value.ProductDetails[0];
        //    // Arrange
        //    var purchaseOrderRequest = new PurchaseOrderRequest
        //    {
        //        CreatedBy = 1,
        //        BillingAddress = "123 Billing St",
        //        BillingAddressCity = "Billing City",
        //        BillingAddressState = "Billing State",
        //        BillingAddressCountry = "Billing Country",
        //        BillingAddressZipcode = "12345",
        //        ShippingAddress = "456 Shipping St",
        //        ShippingAddressCity = "Shipping City",
        //        ShippingAddressState = "Shipping State",
        //        ShippingAddressCountry = "Shipping Country",
        //        ShippingAddressZipcode = "54321",
        //        TermsAndConditions = "Terms and conditions text",
        //        Description = "Purchase order description",
        //        ProductsPurchased = new List<InsertProductPurchaseRequest>
        //    {
        //        new InsertProductPurchaseRequest
        //        {
        //            VendorId= insertVendor.Value.Id,
        //            PurchaseOrderId=new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
        //            ProductId =firstProduct.Id,
        //            Quantity = 10
        //          }
        //    }
        //    };
        //    var insertPurchaseOrderResult = await purchaseOrderService.InsertPurchaseOrder(purchaseOrderRequest);
        //    Assert.NotNull(insertPurchaseOrderResult);
        //    //Assert.True(result.IsActive);
        //}


        [Fact]
        public void UpdatePurchaseOrder_ValidPurchaseOrderId()
        {
            var purchaseOrderId = new Guid("58260DDC-8F79-4E0D-32E9-08DB851A6B4B");
            var purchaseOrderRequest = new PurchaseOrderRequest
            {

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
                    VendorId= new Guid("77CAB252-588D-4405-6972-08DB8505AF71"),
                    PurchaseOrderId=new Guid("00000000-0000-0000-0000-000000000000"),
                    ProductId = new Guid("EC0B0901-4FB7-4F18-B5B9-08DB8505AF80"),
                    Quantity = 10
                  },
                new InsertProductPurchaseRequest
                {


                    VendorId= new Guid("77CAB252-588D-4405-6972-08DB8505AF71"),
                    PurchaseOrderId=new Guid("00000000-0000-0000-0000-000000000000"),
                    ProductId = new Guid("02642067-CED5-4DCC-B5BC-08DB8505AF80"),
                    Quantity = 13
                  }
            }
            };
            var result=purchaseOrderService.updatePurchaseOrder(purchaseOrderId, purchaseOrderRequest);
            Assert.NotNull(result);
           


        }


         [Fact]
        public async Task PurchaseOrder_ReturnsCorrectData()
        {
            var result=await purchaseOrderService.GetPurchasedOrders();
            Assert.NotNull(result);
        }
       

        [Fact]
        public void DeleteOrder_ValidPurchaseOrderId()
        {
            var PurchaseOrderId = new Guid("58260DDC-8F79-4E0D-32E9-08DB851A6B4B");
            var result = purchaseOrderService.DeletePurchaseOrder(PurchaseOrderId);
          
;            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllPendingPurchaseOrderofaUser()
        {
            var result = await purchaseOrderService.GetAllPendingRequestsOfaUser(2047);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Value);
        }
        [Fact]
        public async Task GetAllPendingPurchaseOrder()
        {
            var result = await purchaseOrderService.GetAllPendingPurchaseOrders();
            Assert.NotNull(result);
            Assert.NotEmpty(result.Value);
        }
        [Fact]
        public  void  GetMonthlyExpense()
        {
            var result=purchaseOrderService.GetCurrentMonthExpense();
            Assert.NotNull(result);
            Assert.True(result>0);
        }
        [Fact]
        public async Task GetYearlyExpense()
        {
            var result=await purchaseOrderService.GetCurrentYearExpense();
            decimal Expense = (decimal)result.Value;
            Assert.NotNull(result);
            Assert.True(Expense>0);
        }
        [Fact]
        public async Task GetListOfExpensesForMonth()
        {
            var result=await purchaseOrderService.GetListOfExpensesForMonth();
            Assert.NotNull(result);
            Assert.NotEmpty(result.Value);
        }
        [Fact]
        public async Task GetCountOfPendingRequests()
        {
            var result = await purchaseOrderService.GetCountOfAllPendingPurchaseOrders();
            Assert.NotNull(result);
            Assert.True(result.Value > 0);
        }





        // Negative Tests
        
        [Fact]
        public async Task DeletePurchaseOrder_NullResult_Test()
        {

            var PurchaseOrderId = new Guid("d8bf64b1-a4ba-42db-0f4f-08db84417881");
            
            var result = await purchaseOrderService.DeletePurchaseOrder(PurchaseOrderId);

            
            Assert.Null(result);
        }


        [Fact]
        public void GetPurchaseOrderById_InValid_PID()
        {
            var PurchaseOrderId = new Guid("05088A1D-702E-4AD6-1CA7-08DB850BFBC6");
            var result = purchaseOrderService.GetVendorDetails(PurchaseOrderId);
            Assert.Null(result);

        }


        [Fact]
        public async Task UpdatePurchaseOrder_InValidPurchaseOrderId_Test()
        {
            var purchaseOrderId = new Guid("68260DDC-8F79-4E0D-32E9-08DB851A6B4B");
            var purchaseOrderRequest = new PurchaseOrderRequest
            {
               

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


                    VendorId= new Guid("77CAB252-588D-4405-6972-08DB8505AF71"),
                    PurchaseOrderId=new Guid("00000000-0000-0000-0000-000000000000"),
                    ProductId = new Guid("EC0B0901-4FB7-4F18-B5B9-08DB8505AF80"),
                    Quantity = 10
                  },
                new InsertProductPurchaseRequest
                {


                    VendorId= new Guid("77CAB252-588D-4405-6972-08DB8505AF71"),
                    PurchaseOrderId=new Guid("00000000-0000-0000-0000-000000000000"),
                    ProductId = new Guid("02642067-CED5-4DCC-B5BC-08DB8505AF80"),
                    Quantity = 13
                  }
            }
            };
       

            var result = await purchaseOrderService.updatePurchaseOrder(purchaseOrderId, purchaseOrderRequest);
            Assert.Null(result);
        }



       







    }
}
