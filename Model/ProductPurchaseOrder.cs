using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductPurchaseOrder
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public long Quantity { get; set; }
    }
}
