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
            var role =await RoleService.InsertRole(roleRequest);
            if (role.Value != null) { 
                return Ok(role.Value);
            }
            return BadRequest("Invalid role");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await RoleService.GetAllRoles();
            return Ok(roles.Value);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id)
        {
            var role =await RoleService.GetRoleById(id);
            if (role.Value != null && role.Value.Id == id)
            {
                return Ok(role.Value);
            }
            return NotFound("Role not found");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            var role = await RoleService.DeleteRole(id);
            if (role.Value != null && role.Value.Id == id)
            {
                return Ok(role.Value);
            }
            return NotFound("Role not found");
        }
    }
}
