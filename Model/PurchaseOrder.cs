using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("UserId")] //need to add relation to table
        public Guid? UserId { get; set; }

        public string? TrackingNumber { get; set; }

        [Required]
        public string? OrderDateTime { get; set; } = DateTime.Now.ToString();

        [Required]
        public string? DueDate { get; set; } = DateTime.Now.ToString();

        [ForeignKey("ApprovedBy")]//need to add relation to table
        public virtual Guid? ApprovedBy { get; set; }

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