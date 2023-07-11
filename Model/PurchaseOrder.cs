using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string? TrackingNumber { get; set; }

        [Required]
        public string? OrderDateTime { get; set; } = DateTime.Now.ToString();

        [Required]
        public string? DueDate { get; set; } = DateTime.Now.ToString();

        [Required]
        public Guid ApprovedBy { get; set; }

        [Required]
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
        [MaxLength(6)]
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

    }
}