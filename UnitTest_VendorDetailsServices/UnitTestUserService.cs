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
using Microsoft.AspNetCore.Mvc;

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
            userService = new UserService(dbContextAccess);
        }

        [Fact]
        public async Task InsertUser()
        {
            var userRegisterRequest = new UserRegisterRequest
            {
                Name = "TestName2",
                Email="testname2@gmail.com",
                PhoneNumber="987654321",
                Password="test@123",
            };
            var result= await userService.InsertUser(userRegisterRequest);
            Assert.NotNull(result);
            userService.DeleteUser_Test(result.Value.Id);
        }
       
        //[Fact]
        //public async Task UserLogin()
        //{
        //    var userRegisterRequest = new UserRegisterRequest
        //    {
        //        Name = "TestName3",
        //        Email = "testname0@gmail.com",
        //        PhoneNumber = "987654321",
        //        Password = "test@123",
        //    };
        //     var insertUserResult=await userService.InsertUser(userRegisterRequest);
        //    var loginRequest = new LoginRequest
        //    {
        //        Email = "testname0@gmail.com",
        //        Password = "test@123"
        //    };
        //    var loginresult = await userService.LoginUser(loginRequest);
        //    Assert.NotNull(loginresult);
        //    userService.DeleteUser_Test(insertUserResult.Value.Id);
            
        //}

        [Fact]
        public async Task UpdateUserDetail_ValidUser()
        {
            var user = await InsertUser_Test();
            var userUpdateRequest = new UserUpdateRequest
            {
                Name = "TestName4",
                Email="testname4@gmail.com",
                PhoneNumber="987654123"

            };
            var updateUserResult = await userService.UpdateUser(user.Value.Id, userUpdateRequest);
            Assert.NotNull(updateUserResult);
            Assert.Equal(userUpdateRequest.Name, updateUserResult.Value.Name);
            Assert.Equal(userUpdateRequest.Email, updateUserResult.Value.Email);
             Assert.Equal(userUpdateRequest.PhoneNumber, updateUserResult.Value.PhoneNumber);
            userService.DeleteUser_Test(user.Value.Id);


        }

       

        [Fact]
        public async Task DeleteUser_Valid()
        {
            var userRegisterRequest = new UserRegisterRequest
            {
                Name = "TestName5",
                Email = "testname5@gmail.com",
                PhoneNumber = "987654321",
                Password = "test@123",
            };
            var userRegisterRequest1 = new UserRegisterRequest
            {
                Name = "TestName6",
                Email = "testname6@gmail.com",
                PhoneNumber = "987654321",
                Password = "test@123",
            };

            var userResult = await userService.InsertUser(userRegisterRequest);
            var deletedByUserResult = await userService.InsertUser(userRegisterRequest1);

            var userId = userResult.Value.Id;
            var deletedBy = deletedByUserResult.Value.Id;

            var result = await userService.DeleteUser(userId, deletedBy);
            Assert.NotNull(result);
            Assert.False(result.Value.IsActive);
            Assert.Equal(userId, result.Value.Id);
            userService.DeleteUser_Test(userId);
            userService.DeleteUser_Test(deletedBy);

        }




        ////Negative Tests


        [Fact]
        public async void UserLogin_InvalidWEmailPassword()
        {
            var loginRequest = new LoginRequest
            {
                Email = "testname@gmail.com",
                Password = "test123"
            };
            var result = await userService.LoginUser(loginRequest);
            Assert.Null(result);
           
        }

        [Fact]
        public async Task UpdateUserDetail_InValidUser()
        {
            var userUpdateRequest = new UserUpdateRequest
            {
                Name = "TestName4",
                Email = "testname4@gmail.com",
                PhoneNumber = "987654123"

            };
            var updateUserResult = await userService.UpdateUser(-1, userUpdateRequest);
            Assert.NotNull(updateUserResult);
            Assert.Null(updateUserResult.Value);
        }

        [Fact]
        public async Task DeleteUser_InvalidId() 
        {
            var userId = -2;
            var deletedBy = -1;

            var result = await userService.DeleteUser(userId, deletedBy); 

            Assert.Null(result);
        }


        [Fact]
        public async void InsertUser_InvalidDetails()
        {
            var userRegisterRequest = new UserRegisterRequest
            {
                Name = "",
                Email = "",
                PhoneNumber = "",
                Password = "",
                
            };
            var result = await userService.InsertUser(userRegisterRequest);
            Assert.Null(result);
            
            
        }


        //Sample data  inserted for testing
        public async Task<ActionResult<User>> InsertUser_Test()
        {
            var userRegisterRequest = new UserRegisterRequest
            {
                Name = "TestName3",
                Email = "testname0@gmail.com",
                PhoneNumber = "987654321",
                Password = "test@123",
            };
            var insertUserResult = await userService.InsertUser(userRegisterRequest);
            return insertUserResult;

        }
    }

}
