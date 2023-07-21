using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PurchaseOrderWithUsersName
    {
        public Guid Id { get; set; }

        //[Display(Name = "User")]
        public UserForPO? CreatedBy { get; set; }

        //[ForeignKey("CreatedBy")]
        //public virtual User? User { get; set; }

        public string? TrackingNumber { get; set; }

        [Required]
        public string? OrderDateTime { get; set; } = DateTime.Now.ToString();

        [Required]
        public string? DueDate { get; set; } = DateTime.Now.ToString();

        //[Display(Name = "User")]
        public UserForPO? ApprovedBy { get; set; }

        //[ForeignKey("ApprovedBy")]
        //public virtual User? User1 { get; set; }

        public string? ApprovedDateTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string? BillingAddress { get; set; }

        [Required]
        [MaxLength(255)]
        public string? BillingAddressCity { get; set; }

        [Required]
        [MaxLength(255)]
        public string? BillingAddressState { get; set; }

        [Required]
        [MaxLength(255)]
        public string? BillingAddressCountry { get; set; }

        [Required]
        [MaxLength(255)]
        public string? BillingAddressZipcode { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ShippingAddress { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ShippingAddressCity { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ShippingAddressState { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ShippingAddressCountry { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ShippingAddressZipcode { get; set; }

        [Required]
        [MaxLength(255)]
        public string? TermsAndConditions { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Total { get; set; }


        [Required]
        public bool IsActive { get; set; }
    }
}
