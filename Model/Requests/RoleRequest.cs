using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Requests
{
    public class RoleRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
