using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User { 

        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string? CreatedOn { get; set; }

        public string? UpdatedOn { get; set; }

        public bool IsActive { get; set; }

        public string? ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        public string? ApprovedOn { get; set; }

        public string? DeletedOn { get; set; }

        public int? DeletedBy { get; set; }

        [Display(Name = "Role")]
        public virtual Guid RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}
