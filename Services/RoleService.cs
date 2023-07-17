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

        public Role InsertRole(RoleRequest roleRequest)
        {
            Role role = new Role();
            if (roleRequest != null)
            {
                role.Name = roleRequest.Name;
                role.IsActive = true;
                dbContextAccess.Roles.Add(role);
                dbContextAccess.SaveChanges();
            }
            return role;
        }

        public List<Role> GetAllRoles()
        {
            return dbContextAccess.Roles.Where(role => role.IsActive).ToList();  
        }

        public Role GetRoleById(Guid id)
        {
            return dbContextAccess.Roles.FirstOrDefault(r => r.Id == id && r.IsActive);
        }

        public Role DeleteRole(Guid id)
        {
            var role = dbContextAccess.Roles.Find(id);
            if (role != null)
            {
                role.IsActive = false;
                dbContextAccess.Roles.Update(role);
                dbContextAccess.SaveChanges();
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
