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
        public string? DueDate { get; set; }

        [Required]
        public Guid ApprovedBy { get; set; }

        [Required]
        public string? ApprovedDateTime { get; set; }

        [Required]
        public string? BillingAddress { get; set; }

        [Required]
        public string? BillingAddressCity { get; set; }

        [Required]
        public string? BillingAddressState { get; set; }

        [Required]
        public string? BillingAddressCountry { get; set; }

        [Required]
        public string? BillingAddressZipcode { get; set; }

        [Required]
        public string? ShippingAddress { get; set; }

        [Required]
        public string? ShippingAddressCity { get; set; }

        [Required]
        public string? ShippingAddressState { get; set; }

        [Required]
        public string? ShippingAddressCountry { get; set; }

        [Required]
        public string? ShippingAddressZipcode { get; set; }

        [Required]
        public string? TermsAndConditions { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Total { get; set; }

    }
}