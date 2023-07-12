using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductPurchaseOrder
    {
        [Required]
        public Guid Id { get; set; }

        
        [Display(Name = "PurchaseOrder")]
        public virtual Guid? PurchaseOrderId { get; set; }

        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        
        [Display(Name = "VendorDetails")]
        public virtual Guid? VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual VendorDetails? VendorDetails { get; set; }

        
        [Display(Name ="ProductDetail")]
        public virtual Guid? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductDetail? ProductDetail { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
