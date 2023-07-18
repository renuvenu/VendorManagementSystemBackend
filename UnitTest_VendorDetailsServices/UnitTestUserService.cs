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
            userService = new UserService(dbContextAccess);
        }

        [Fact]
        public async void InsertUser()
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

        [Fact]
        public async void UserLogin()
        {
            var userRegisterRequest = new UserRegisterRequest
            {
                Name = "TestName3",
                Email = "testname0@gmail.com",
                PhoneNumber = "987654321",
                Password = "test@123",
            };
             userService.InsertUser(userRegisterRequest);
            var loginRequest = new LoginRequest
            {
                Email = "testname0@gmail.com",
                Password = "test@123"
            };
            var loginresult = await userService.LoginUser(loginRequest);
            Assert.NotNull(loginresult);
            
        }
        

        [Fact]
        public void DeleteUser()
        {

            var userRegisterRequest = new UserRegisterRequest
            {
                Name = "TestName5",
                Email = "testname5@gmail.com",
                PhoneNumber = "987654321",
                Password = "test@123",
            };
            var UserResult =  userService.InsertUser(userRegisterRequest);
            var userRegisterRequest1 = new UserRegisterRequest
            {
                Name = "TestName6",
                Email = "testname6@gmail.com",
                PhoneNumber = "987654321",
                Password = "test@123",
            };
            var DeletedbyUserresult = userService.InsertUser(userRegisterRequest1);
            var UserId =UserResult.Result.Value.Id;
            var DeletedBy=DeletedbyUserresult.Result.Value.Id;
            
            var result = userService.DeleteUser(UserId, DeletedBy);
            //Assert.NotNull(result);
            Assert.Equal(UserId,result.Id);
           // userService.DeleteUser_Test(UserResult.Id);
           // userService.DeleteUser_Test(DeletedbyUserresult.Id);

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
    }

}
