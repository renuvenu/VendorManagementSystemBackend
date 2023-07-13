using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Requests
{
    public  class PurchaseProductDetails
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ProductName { get; set; }

        [MaxLength(255)]
        public string? ProductDescription { get; set; }


        [Required]
        [Column(TypeName = "decimal(28, 2)")]
        public decimal? Price { get; set; }
    }
}
