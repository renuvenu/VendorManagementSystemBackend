using Microsoft.AspNetCore.Mvc;
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

        public UserController(DbContextAccess dbContextAccess,IConfiguration configuration) { 
            userService = new UserService(dbContextAccess,configuration);
        }

        [HttpGet]
        public async Task<IActionResult> getAllUsers()
        {
            var users = await userService.GetUsers();
            return Ok(users.Value);
        }

        [HttpGet]
        [Route("/user-requests/pending")]
        public async Task<IActionResult> GetAllApprovalPendingRequests()
        {
            var pendingRequests = await userService.GetAllApprovalPendingRequests();
            return Ok(pendingRequests.Value);
        }

        [HttpGet]
        [Route("/user-requests/approved")]
        public async Task<IActionResult> GetAllApprovalApprovedRequests()
        {
            var approvedRequests =await userService.GetAllApprovalApprovedRequests();
            return Ok(approvedRequests.Value);
        }


        [HttpGet]
        [Route("/user-requests/declined")]
        public async Task<IActionResult> GetAllApprovalDeclinedRequests()
        {
            var declinedRequests = await userService.GetAllApprovalDeclinedRequests();
            return Ok(declinedRequests.Value);
        }

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

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
        {
            var user =await userService.LoginUser(loginRequest);
            if(user.Value.User != null && user.Value.User.Id > 0)
            {
                return Ok(user.Value);
            }
            return NotFound("User not found");
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, UserUpdateRequest UserUpdateRequest)
        {
            var user = await userService.UpdateUser(id, UserUpdateRequest);
            if(user.Value !=null && user.Value.Id > 0) { 
                return Ok(user.Value); 
            }
            return BadRequest("Invalid request");
        }

        [HttpPut]
        [Route("/update/{id:int}/{approverId:int}/{status}")]
        public async Task<IActionResult> UpdateApprovalStatus([FromRoute] int id, [FromRoute] int approverId, [FromRoute] string status)
        {
            var user = await userService.UpdateApprovalStatus(id,approverId,status);

            if(user.Value !=null && user.Value.Id > 0)
            {
                return Ok(user.Value);
            }
            return NotFound("User not found");
        }

        [HttpDelete]
        [Route("{id:int}/{deletedBy:int}")]
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
        public async Task<IActionResult> getApprovalStatus([FromRoute] int id)
        {
            var status =await userService.getApprovalStatus(id);
            return Ok(status.Value);
        }
    }
}
