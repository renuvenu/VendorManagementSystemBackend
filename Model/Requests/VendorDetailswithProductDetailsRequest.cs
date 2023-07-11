using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Requests
{
    public class VendorDetailswithProductDetailsRequest
    {
        public VendorDetails VendorDetails { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
    }
}
