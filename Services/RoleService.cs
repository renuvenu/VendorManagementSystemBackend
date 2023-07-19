using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Requests;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RoleService
    {
        private readonly DbContextAccess dbContextAccess;

        public RoleService(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
        }

        public RoleService() { }

        public async Task<ActionResult<Role>> InsertRole(RoleRequest roleRequest)
        {
            Role role = new Role();
            if (roleRequest != null && !string.IsNullOrWhiteSpace(roleRequest.Name))
            {
                role.Name = roleRequest.Name;
                role.IsActive = true;
                await dbContextAccess.Roles.AddAsync(role);
                await dbContextAccess.SaveChangesAsync();
                return role;
            }
            return null;
        }

        public async Task<ActionResult<List<Role>>> GetAllRoles()
        {
            return await dbContextAccess.Roles.Where(role => role.IsActive).ToListAsync();  
        }

        public async Task<ActionResult<Role>> GetRoleById(Guid id)
        {
            return await dbContextAccess.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<ActionResult<Role>> DeleteRole(Guid id)
        {
            var role = await dbContextAccess.Roles.FindAsync(id);
            if (role != null)
            {
                role.IsActive = false;
                dbContextAccess.Roles.Update(role);
                await dbContextAccess.SaveChangesAsync();
            }
            return role;
        } 

        public void DeleteRole_Test(Guid id)
        {
            var role = dbContextAccess.Roles.Find(id);
            if (role != null)
            {
                dbContextAccess.Roles.Remove(role);
                dbContextAccess.SaveChanges();
            }
        }
    }
}
