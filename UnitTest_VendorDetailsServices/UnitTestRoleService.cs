using Microsoft.EntityFrameworkCore;
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
        public void InsertRole()
        {
            var roleRequest = new RoleRequest
            {
                Name = "TestRole"

            };
            var result= roleService.InsertRole(roleRequest);
            Assert.NotNull(result);
            //Assert.Equal(roleRequest.Name, result.Name);
            //roleService.DeleteRole_Test(result.Id);
        }
        [Fact]
        public void DeleteRole()
        {
            var RoleId = new Guid("277D3834-89C7-4FC1-3E07-08DB85C066CC");
            var Result= roleService.DeleteRole(RoleId);
            Assert.NotNull(Result);
           // Assert.Equal(RoleId, Result.Id);
        }

        [Fact]
        public void GetAllRoles()
        {
            var Result = roleService.GetAllRoles();
            Assert.NotNull(Result);
            //Assert.NotEmpty(Result);
        }

        [Fact]
        public void GetRoleById()
        {
            var RoleId = new Guid("6163955B-E69B-40AB-287E-08DB85C04932");
            var Result=roleService.GetRoleById(RoleId);
            Assert.NotNull(Result);
           // Assert.Equal((Guid)RoleId, Result.Id);
        }

        // Negative Tests
        [Fact]
        public void GetRoleById_Invalid() 
        {
            var RoleId = new Guid("6163950B-E69B-40AB-287E-08DB85C04932");
            var Result = roleService.GetRoleById(RoleId);
            Assert.Null(Result);
    
        }
        [Fact]
        public void DeleteRole_InvalidId()
        {
            var RoleId = new Guid("277D1834-89C7-4FC1-3E07-08DB85C066CC");
            var Result = roleService.DeleteRole(RoleId);
            Assert.Null(Result);
            
        }

        [Fact]
        public void InsertRole_InvalidDetails()
        {
            var roleRequest = new RoleRequest
            {
                Name = ""

            };
            var result = roleService.InsertRole(roleRequest);
            Assert.NotNull(result);
            //Assert.Equal("", result.Name);
            // roleService.DeleteRole_Test(result.Id);
        }
    }
}
