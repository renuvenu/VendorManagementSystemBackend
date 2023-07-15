using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Requests;
using Repository;
using Services;

namespace VendorManagement_WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RoleController: Controller
    {
        public RoleService RoleService;

        public RoleController(DbContextAccess dbContextAccess) { 
            RoleService = new RoleService(dbContextAccess);
        }

        [HttpPost]
        public async Task<IActionResult> InsertRole(RoleRequest roleRequest)
        {
            var role = RoleService.InsertRole(roleRequest);
            if (role != null) { 
                return Ok(role);
            }
            return BadRequest("Invalid role");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(RoleService.GetAllRoles());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id)
        {
            var role = RoleService.GetRoleById(id);
            if (role != null)
            {
                return Ok(role);
            }
            return NotFound("Role not found");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            var role = RoleService.DeleteRole(id);
            if (role != null)
            {
                return Ok(role);
            }
            return NotFound("Role not found");
        }
    }
}
