using Model.Requests;
using Model;
using Repository;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnitTest_VendorDetailsServices
{
    public class UnitTestUserService
    {

        private readonly DbContextAccess dbContextAccess;
        public ProductDetailsService productDetailsService;
        public UserService userService;

        public UnitTestUserService()
        {
            dbContextAccess = new DbContextAccess(new DbContextOptions<DbContextAccess>());
            productDetailsService = new ProductDetailsService(dbContextAccess);
            //userService = new UserService(dbContextAccess);
        }

        [Fact]
        public void InsertUser()
        {
            var userRegisterRequest = new UserRegisterRequest
            {
                Name = "TestName2",
                Email="testname2@gmail.com",
                PhoneNumber="987654321",
                Password="test@123",
                //Role="User"
            };
            var result=userService.InsertUser(userRegisterRequest);
            Assert.NotNull(result);
           userService.DeleteUser_Test(result.Id);
        }

        [Fact]
        public void UserLogin()
        {
            var loginRequest = new LoginRequest
            {
                Email="testname@gmail.com",
                Password="test@123"
            };
            var result=userService.LoginUser(loginRequest);
            Assert.NotNull(result);
            //Assert.Equal(loginRequest.Email, result.Email);
        }
        //[Fact]
        //public void GetUserStatus()
        //{
        //    var id = 1003;
        //    var result=userService.getApprovalStatus(id);
        //    Assert.Equal("Approved", result);
        //}

        //[Fact]
        //public void DeleteUser()
        //{
        //    var UserId = 1005;
        //    var DeletedBy = 1;
        //    var result=userService.DeleteUser(UserId, DeletedBy);
        //    Assert.NotNull(result);
        //    Assert.Equal(DeletedBy, result.DeletedBy);
            
        //}

        //[Fact]
        //public void GetAllPendingStatusOfUser()
        //{
        //    var result = userService.GetAllApprovalPendingRequests();

        //    Assert.NotEmpty(result);

        //}

        //[Fact]
        //public void GetAllApprovedStatusOfUser()
        //{
        //    var result=userService.GetAllApprovalApprovedRequests();
        //    Assert.NotEmpty(result);
        //}
        //[Fact]
        //public void GetAllDeclinedStatusofUser()
        //{
        //    var result= userService.GetAllApprovalDeclinedRequests();
        //    Assert.NotEmpty(result);
        //}

        ////Negative Tests

        //[Fact]
        //public void GetUserStatus_InvalidId()
        //{
        //    var id = 108;
        //    string result = userService.getApprovalStatus(id);
        //    Assert.Null(result);
        //}

        [Fact]
        public void UserLogin_InvalidWEmailPassword()
        {
            var loginRequest = new LoginRequest
            {
                Email = "testname@gmail.com",
                Password = "test123"
            };
            var result = userService.LoginUser(loginRequest);
            Assert.Null(result);
           
        }

        //[Fact]
        //public void InsertUser_InvalidDetails()
        //{
        //    var userRegisterRequest = new UserRegisterRequest
        //    {
        //        Name = "",
        //        Email = "",
        //        PhoneNumber = "987654321",
        //        Password = "test@123",
        //        Role = "User"
        //    };
        //    var result = userService.InsertUser(userRegisterRequest);
        //    Assert.NotNull (result);
        //    Assert.Null(result.Name);
        //    Assert.Null(result.Email);
        //    Assert.Null(result.ApprovalStatus);
        //    Assert.Null(result.ApprovedBy);
        //}
    }

}
