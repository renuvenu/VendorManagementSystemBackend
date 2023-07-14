using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Requests
{
    public class PurchaseOrderRequest
    {
        [Required]
        public int CreatedBy{ get; set; }

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
        public List<InsertProductPurchaseRequest>? ProductsPurchased { get; set; }


    }
}
