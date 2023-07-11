using Microsoft.AspNetCore.Mvc;
using Repository;

namespace VendorManagement_WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductPurchaseOrderController : Controller
    {
        private readonly DbContextAccess productpurchasedetailDBContext1;

        public ProductPurchaseOrderController(DbContextAccess productpurchasedetailDBContext1)
        {
            this.productpurchasedetailDBContext1 = productpurchasedetailDBContext1;
        }
    }
}
