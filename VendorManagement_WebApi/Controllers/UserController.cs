using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Model.Requests;
using Repository;
using Services;
using System.Data;
using System.Diagnostics.Eventing.Reader;



namespace VendorManagement_WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        public UserService userService;



        public UserController(DbContextAccess dbContextAccess, IConfiguration configuration, IOptions<MailSettings> mailSettings)
        {
            userService = new UserService(dbContextAccess, configuration, mailSettings);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAllUsers()
        {
            var users = await userService.GetUsers();
            return Ok(users.Value);
        }



        //[HttpGet]
        //[Route("/user-requests/pending")]
        //public async Task<IActionResult> GetAllApprovalPendingRequests()
        //{
        //    var pendingRequests = await userService.GetAllApprovalPendingRequests();
        //    return Ok(pendingRequests.Value);
        //}



        //[HttpGet]
        //[Route("/user-requests/approved")]
        //public async Task<IActionResult> GetAllApprovalApprovedRequests()
        //{
        //    var approvedRequests =await userService.GetAllApprovalApprovedRequests();
        //    return Ok(approvedRequests.Value);
        //}




        //[HttpGet]
        //[Route("/user-requests/declined")]
        //public async Task<IActionResult> GetAllApprovalDeclinedRequests()
        //{
        //    var declinedRequests = await userService.GetAllApprovalDeclinedRequests();
        //    return Ok(declinedRequests.Value);
        //}

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
            var user = await userService.LoginUser(loginRequest);
            if (user != null && user.Value.User.Id > 0)
            {
                return Ok(user.Value);
            }
            return NotFound("User not found");
        }



        [Authorize(Roles = "Admin,Approver,User")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, UserUpdateRequest UserUpdateRequest)
        {
            var user = await userService.UpdateUser(id, UserUpdateRequest);
            if (user.Value != null && user.Value.Id > 0)
            {
                return Ok(user.Value);
            }
            return BadRequest("Invalid request");
        } 



        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("/update/{id:int}/{role}")]
        public async Task<IActionResult> UpdateUserRole([FromRoute] int id, [FromRoute] string role)
        {
            var user = await userService.UpdateUserRole(id, role);



            if (user.Value != null && user.Value.Id > 0)
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
            if (user.Value != null && user.Value.Id > 0)
            {
                return Ok(user.Value);
            }
            return NotFound("user not found");
        }
    }
}