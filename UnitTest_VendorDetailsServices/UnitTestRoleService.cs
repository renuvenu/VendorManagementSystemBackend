using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Requests;
using Repository;
using Services;


namespace UnitTest_VendorDetailsServices
{
    public class UnitTestRoleService
    {
        private readonly DbContextAccess dbContextAccess;
        public RoleService roleService;
        public UnitTestRoleService() {
            dbContextAccess = new DbContextAccess(new DbContextOptions<DbContextAccess>());
            roleService = new RoleService(dbContextAccess);
        }
        [Fact]
        public async Task InsertRole()
        {
            var roleRequest = new RoleRequest
            {
                Name = "TestRole"

            };
            var result= await roleService.InsertRole(roleRequest);
            Assert.NotNull(result);
            Assert.Equal(roleRequest.Name, result.Value.Name);
            roleService.DeleteRole_Test(result.Value.Id);
        }

        

        [Fact]
        public async Task DeleteRole()
        {
            var Role =await InsertRole_Test();
            var Result= await roleService.DeleteRole(Role.Value.Id);
            Assert.NotNull(Result);
            Assert.False(Result.Value.IsActive);
            roleService.DeleteRole_Test(Role.Value.Id);
        }

      

        [Fact]
        public async void GetAllRoles()
        {
            var Result = await roleService.GetAllRoles();
            Assert.NotNull(Result);
            Assert.NotEmpty(Result.Value);
        }

        [Fact]
        public async Task GetRoleById()
        {
            var Role = await InsertRole_Test();
            var Result=await roleService.GetRoleById(Role.Value.Id);
            Assert.NotNull(Result);
            Assert.Equal(Role.Value.Id, Result.Value.Id);
            roleService.DeleteRole_Test(Role.Value.Id);
        }

        public async Task<ActionResult<Role>> InsertRole_Test()
        {
            var roleRequest = new RoleRequest
            {
                Name = "TestRole"

            };
            var result = await roleService.InsertRole(roleRequest);
            return result;
        }

        // Negative Tests
        [Fact]
        public async Task GetRoleById_Invalid() 
        {
            var RoleId = new Guid("6163950B-E69B-40AB-287E-08DB85C04932");
            var Result = await roleService.GetRoleById(RoleId);
            Assert.Null(Result.Value);
    
        }
        [Fact]
        public async Task DeleteRole_InvalidId()
        {
            var RoleId = new Guid("277D1834-89C7-4FC1-3E07-08DB85C066CC");
            var Result = await roleService.DeleteRole(RoleId);
            Assert.Null(Result.Value);
            
        }

        [Fact]
        public async Task InsertRole_InvalidDetails()
        {
            var roleRequest = new RoleRequest
            {
                Name = ""

            };
            var result = await roleService.InsertRole(roleRequest);
            Assert.Null(result);
        }
    }
}
