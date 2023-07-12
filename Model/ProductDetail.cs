using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductDetail
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "VendorDetails")]
        public Guid VendorId { get; set; }

        [Required]
        [ForeignKey("VendorId")]
        public virtual VendorDetails? VendorDetails { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ProductName { get; set; }

        [MaxLength(255)]
        public string? ProductDescription { get; set; } 

        

        [Required]
        [Column(TypeName = "decimal(28, 2)")]
        public decimal? Price { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }
}
