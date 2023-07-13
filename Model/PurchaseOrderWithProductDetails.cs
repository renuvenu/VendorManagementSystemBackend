using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Model.Requests;

namespace Model
{
    public class PurchaseOrderWithProductDetails
    {
        public PurchaseOrder PurchaseOrders { get; set; }

        public List<PurchaseProductDetails> PurchaseProducts { get; set;}

        public VendorDetails VendorDetails { get; set; }
        //public List<ProductPurchaseOrder> ProductDetails { get; set; }
        //public List<ProductDetail> Details { get; set; }    
    }
}
