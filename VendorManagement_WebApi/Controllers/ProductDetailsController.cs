using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Requests;
using Repository;
using Services;

namespace VendorManagement_WebApi.Controllers
{
    
        [ApiController]
        [Route("api/[Controller]")]
        public class ProductDetailsController : Controller
        {
            public ProductDetailsService productDetailsService;

            public ProductDetailsController()
            {
                productDetailsService = new ProductDetailsService();
            }
    }
    }

