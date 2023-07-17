using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Requests
{
    public class VendorDetailsRequest
    {
        [Required]
        [MaxLength(255)]
        public string? VendorName { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string VendorType { get; set; }
        [Required]
        [MaxLength(255)]
        public string? AddressLine1 { get; set; }
        [MaxLength(255)]
        public string? AddressLine2 { get; set; }

        [Required]
        [MaxLength(255)]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }
        [Required]
        [MaxLength(6)]
        public string? PostalCode { get; set; }
        [Required]
        public string? Country { get; set; }
        [Required]
        [MaxLength(10)]
        public string? TelePhone1 { get; set; }
        [MaxLength(10)]
        public string? TelePhone2 { get; set; }
        [Required]
        [MaxLength(255)]
        public string? VendorEmail { get; set; }

        public string? VendorWebsite { get; set; }


        [Required]
        public List<InsertProductDetailRequest>? ProductDetailsRequest { get; set; }




    }
}
