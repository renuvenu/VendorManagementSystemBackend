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

        public UserController(DbContextAccess dbContextAccess) { 
            userService = new UserService(dbContextAccess);
        }

        [HttpGet]
        public async Task<IActionResult> getAllUsers()
        {
            return Ok(userService.GetUsers());
        }

        [HttpGet]
        [Route("/user-requests/pending")]
        public async Task<IActionResult> GetAllApprovalPendingRequests()
        {
            return Ok(userService.GetAllApprovalPendingRequests());
        }

        [HttpGet]
        [Route("/user-requests/approved")]
        public async Task<IActionResult> GetAllApprovalApprovedRequests()
        {
            return Ok(userService.GetAllApprovalApprovedRequests());
        }


        [HttpGet]
        [Route("/user-requests/declined")]
        public async Task<IActionResult> GetAllApprovalDeclinedRequests()
        {
            return Ok(userService.GetAllApprovalDeclinedRequests());
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser(UserRegisterRequest userRegisterRequest)
        {
            User user = userService.InsertUser(userRegisterRequest);
            if(user != null && user.Id > 0)
            {
                return Ok(user);
            }
           return BadRequest("Invalid user request");
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
        {
            User user = userService.LoginUser(loginRequest);
            if(user != null && user.Id > 0)
            {
                return Ok(user);
            }
            return NotFound("User not found");
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id,UserRegisterRequest userRegisterRequest)
        {
            return Ok();
        }

        [HttpPut]
        [Route("/update/{id:int}/{approverId:int}/{status}")]
        public async Task<IActionResult> UpdateApprovalStatus([FromRoute] int id, [FromRoute] int approverId, [FromRoute] string status)
        {
            var user = userService.UpdateApprovalStatus(id,approverId,status);

            if(user !=null && user.Id > 0)
            {
                return Ok(user);
            }
            return NotFound("User not found");
        }

        [HttpDelete]
        [Route("{id:int}/{deletedBy:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id, [FromRoute] int deletedBy)
        {
           var user = userService.DeleteUser(id, deletedBy);
            if(user != null)
            {
                return Ok(user);
            }
            return NotFound("user not found");
        }

        [HttpGet]
        [Route("/get/approvalStatus{id:int}")]
        public async Task<IActionResult> getApprovalStatus([FromRoute] int id)
        {
            return Ok(userService.getApprovalStatus(id));
        }
    }
}
