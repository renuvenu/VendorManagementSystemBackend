using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Requests
{
    public class InsertProductPurchaseRequest
    {
        public Guid PurchaseOrderId { get; set; }

        public Guid ProductId { get; set; }

        public long Quantity { get; set; }
    }
}
