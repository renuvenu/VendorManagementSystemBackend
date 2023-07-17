using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        public async Task<ActionResult<User>> InsertUser(UserRegisterRequest userRegisterRequest)
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
                user.RoleId = dbContextAccess.Roles.FirstOrDefault(x => x.Name == "Readonly").Id;
                await dbContextAccess.Users.AddAsync(user);
                await dbContextAccess.SaveChangesAsync();
                
            }
            return user;
        }

        public async Task<ActionResult<User>> UpdateUser(int id, UserUpdateRequest userUpdateRequest)
        {
            User user = new User();
            if (userUpdateRequest!=null)
            {
                user = await dbContextAccess.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    user.Name = userUpdateRequest.Name;
                    user.Email = userUpdateRequest.Email;
                    user.PhoneNumber = userUpdateRequest.PhoneNumber;
                    user.Password = BCrypt.Net.BCrypt.HashPassword(userUpdateRequest.Password);
                    user.UpdatedOn = DateTime.Now.ToString();
                    dbContextAccess.Users.Update(user);
                    await dbContextAccess.SaveChangesAsync();
                }
            }
            return user;
        }

        public async Task<ActionResult<LoginResponse>> LoginUser(LoginRequest loginRequest)
        {
            if (loginRequest != null)
            {
                User user1 = dbContextAccess.Users.FirstOrDefault(x => x.Email == loginRequest.Email);
                if (user1 != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, user1.Password))
                {
                    user1.Role =await dbContextAccess.Roles.FindAsync(user1.RoleId);
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

                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<ActionResult<List<User>>> GetUsers()
        {
            List<User> users =await dbContextAccess.Users.ToListAsync();
            users.ForEach(user => user.Role = dbContextAccess.Roles.Find(user.RoleId));
            return users;
        }

        public async Task<ActionResult<User>> UpdateUserRole(int id,string role)
        {
            var user =await dbContextAccess.Users.FindAsync(id);
            if (user != null) {
                user.UpdatedOn = DateTime.Now.ToString();
                user.RoleId = dbContextAccess.Roles.FirstOrDefault(x => x.Name == role).Id;
                user.Role = dbContextAccess.Roles.Find(user.RoleId);
                dbContextAccess.Users.Update(user);
                await dbContextAccess.SaveChangesAsync();
            }
            return user;
        }
        public async Task<ActionResult<User>> DeleteUser(int id,int deletedBy)
        {
            var user =await dbContextAccess.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                user.DeletedBy = deletedBy;
                user.DeletedOn = DateTime.Now.ToString();
                dbContextAccess.Users.Update(user);
                await dbContextAccess.SaveChangesAsync();
            }
            return user;
        } 

        public void DeleteUser_Test(int id)
        {
            var vendor = dbContextAccess.Users.FirstOrDefault(p => p.Id == id);
            if (vendor != null)
            {
                dbContextAccess.Users.Remove(vendor);

            }

        }
    }
}
