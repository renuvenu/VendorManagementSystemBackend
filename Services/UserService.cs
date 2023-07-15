using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.Requests;
using Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly DbContextAccess dbContextAccess;
        private readonly IConfiguration _configuration;
        public PasswordEncryption PasswordEncryption = new PasswordEncryption();

        public UserService(DbContextAccess dbContextAccess, IConfiguration configuration)
        {
            this.dbContextAccess = dbContextAccess;
            this._configuration = configuration;
        }

        public UserService() {
            
        }

        public User InsertUser(UserRegisterRequest userRegisterRequest)
        {
            User user = new User();
            if (userRegisterRequest != null && dbContextAccess.Users.Where(x => x.Email == userRegisterRequest.Email).ToList().Count() == 0)
            {
                
                user.Name = userRegisterRequest.Name;
                user.Email = userRegisterRequest.Email;
                user.PhoneNumber = userRegisterRequest.PhoneNumber;
                user.CreatedOn = DateTime.Now.ToString();
                user.IsActive = true;
                user.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterRequest.Password);
                user.RoleId = dbContextAccess.Roles.FirstOrDefault(x => x.Name == userRegisterRequest.Role).Id;
                if (userRegisterRequest.Role == "Approver" || userRegisterRequest.Role == "User")
                {
                    user.ApprovalStatus = "Pending";
                } else
                {
                    user.ApprovalStatus = "Approved";
                }
                dbContextAccess.Users.Add(user);
                dbContextAccess.SaveChanges();
                
            }
            return user;
        }

        public LoginResponse LoginUser(LoginRequest loginRequest)
        {
            if (loginRequest != null)
            {
                User user1 = dbContextAccess.Users.FirstOrDefault(x => x.Email == loginRequest.Email);
                if (user1 != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, user1.Password))
                {
                    user1.Role = dbContextAccess.Roles.Find(user1.RoleId);
                    if ((user1.Role.Name == "Approver" || user1.Role.Name == "User") && user1.ApprovalStatus != "Approved")
                    {
                        user1.RoleId = dbContextAccess.Roles.FirstOrDefault(x => x.Name == "Readonly").Id;
                        user1.Role = dbContextAccess.Roles.Find(user1.RoleId);
                    }
                    string token = CreateToken(user1);
                    return new LoginResponse
                    {
                        User = user1,
                        Token = token
                    };
                }
            }
            return null;
           
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public List<User> GetUsers()
        {
            var users = dbContextAccess.Users.ToList();
            users.ForEach(user => user.Role = dbContextAccess.Roles.Find(user.RoleId));
            return users;
        }

        public User UpdateApprovalStatus(int id,int approverId,string status)
        {
            var user = dbContextAccess.Users.Find(id);
            if (user != null) {
                user.ApprovalStatus = status;
                user.ApprovedBy = approverId;
                user.ApprovedOn = DateTime.Now.ToString();
                user.UpdatedOn = DateTime.Now.ToString();
                user.Role = dbContextAccess.Roles.Find(user.RoleId);
                dbContextAccess.Users.Update(user);
                dbContextAccess.SaveChanges();
            }
            return user;
        }
        public User DeleteUser(int id,int deletedBy)
        {
            var user = dbContextAccess.Users.Find(id);
            if (user != null)
            {
                user.IsActive = false;
                user.DeletedBy = deletedBy;
                user.DeletedOn = DateTime.Now.ToString();
                dbContextAccess.Users.Update(user);
                dbContextAccess.SaveChanges();
            }
            return user;
        }

        public List<User> GetAllApprovalPendingRequests()
        {
            var users = dbContextAccess.Users.Where(x => x.ApprovalStatus == "Pending").ToList();
            users.ForEach(user => user.Role = dbContextAccess.Roles.Find(user.RoleId));
            return users;
        }

        public List<User> GetAllApprovalApprovedRequests()
        {
            var users = dbContextAccess.Users.Where(x => x.ApprovalStatus == "Approved").ToList();
            users.ForEach(user => user.Role = dbContextAccess.Roles.Find(user.RoleId));
            return users;
        }

        public List<User> GetAllApprovalDeclinedRequests()
        {
            var users = dbContextAccess.Users.Where(x => x.ApprovalStatus == "Declined").ToList();
            users.ForEach(user => user.Role = dbContextAccess.Roles.Find(user.RoleId));
            return users;
        }

        public string getApprovalStatus(int id)
        {
            return dbContextAccess.Users.Find(id).ApprovalStatus;
        }
    }
}
