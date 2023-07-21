using Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PurchaseOrderWithProdDetailsWithUserName
    {
        public PurchaseOrderWithUsersName PurchaseOrderWithUsersName { get; set; }

        public List<PurchaseProductDetails> PurchaseProducts { get; set; }

        public VendorForPurchaseOrder VendorForPurchaseOrder { get; set; }
    }
}
