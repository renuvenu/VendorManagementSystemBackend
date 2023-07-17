using Model.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum VendorType
{
    Service=0,
    Product=1,
    Service_and_Product=2
}
namespace Model
{
    public class VendorDetails
    {

        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? VendorName { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public VendorType VendorType { get; set; }
        [Required]
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? PostalCode { get; set; }
        [Required]
        public string? Country { get; set; }
        [Required]
        public string? TelePhone1 { get; set; }
        public string? TelePhone2 { get; set; }
        [Required]
        public string? VendorEmail { get; set; }

        public string? VendorWebsite { get; set; }

        public string? CreatedOn { get; set; }

        public string? UpdatedOn { get;set; }

        public string? DeletedOn { get; set; }

    }
}
