using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class VendorForPurchaseOrder
    {
        public Guid Id { get; set; }
        public string? VendorName { get; set; }
        public VendorType VendorType { get; set; }
    }
}
