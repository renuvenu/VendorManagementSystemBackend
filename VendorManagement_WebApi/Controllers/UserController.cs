using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Model.Requests;
using Repository;
using Services;
using System.Diagnostics.Eventing.Reader;

namespace VendorManagement_WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController: Controller
    {
        public UserService userService;

        public UserController(DbContextAccess dbContextAccess,IConfiguration configuration, IOptions<MailSettings> mailSettings) { 
            userService = new UserService(dbContextAccess,configuration, mailSettings);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAllUsers()
        {
            var users = await userService.GetUsers();
            return Ok(users.Value);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/user-requests/pending")]
        public async Task<IActionResult> GetAllApprovalPendingRequests()
        {
            return Ok(userService.GetAllApprovalPendingRequests());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/user-requests/approved")]
        public async Task<IActionResult> GetAllApprovalApprovedRequests()
        {
            return Ok(userService.GetAllApprovalApprovedRequests());
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/user-requests/declined")]
        public async Task<IActionResult> GetAllApprovalDeclinedRequests()
        {
            return Ok(userService.GetAllApprovalDeclinedRequests());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> InsertUser(UserRegisterRequest userRegisterRequest)
        {
            var user = await userService.InsertUser(userRegisterRequest);
            if (user.Value != null && user.Value.Id > 0)
            {
                return Ok(user.Value);
            }
            return BadRequest("Invalid user request");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
        {
            var user = userService.LoginUser(loginRequest);
            if(user.User != null && user.User.Id > 0)
            {
                return Ok(user.Value);
            }
            return NotFound("User not found");
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin,Approver")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id,UserRegisterRequest userRegisterRequest)
        {
            var user = await userService.UpdateUser(id, UserUpdateRequest);
            if(user.Value !=null && user.Value.Id > 0) { 
                return Ok(user.Value); 
            }
            return BadRequest("Invalid request");
        }

        [HttpPut]
        [Route("/update/{id:int}/{approverId:int}/{status}")]
        [Authorize(Roles = "Admin,Approver")]
        public async Task<IActionResult> UpdateApprovalStatus([FromRoute] int id, [FromRoute] int approverId, [FromRoute] string status)
        {
            var user = await userService.UpdateUserRole(id,role);

            if(user.Value !=null && user.Value.Id > 0)
            {
                return Ok(user.Value);
            }
            return NotFound("User not found");
        }

        [HttpDelete]
        [Route("{id:int}/{deletedBy:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id, [FromRoute] int deletedBy)
        {
           var user = await userService.DeleteUser(id, deletedBy);
            if(user.Value != null && user.Value.Id > 0)
            {
                return Ok(user.Value);
            }
            return NotFound("user not found");
        }

        [HttpGet]
        [Route("/get/approvalStatus{id:int}")]
        [Authorize(Roles = "Admin,Approver")]
        public async Task<IActionResult> getApprovalStatus([FromRoute] int id)
        {
            return Ok(userService.getApprovalStatus(id));
        }
    }
}
