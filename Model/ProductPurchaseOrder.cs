using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductPurchaseOrder
    {
        public Guid Id { get; set; }
 
        public Guid PurchaseOrderId { get; set; }

        public Guid ProductId { get; set; }

        public long Quantity { get; set; }
    }
}
