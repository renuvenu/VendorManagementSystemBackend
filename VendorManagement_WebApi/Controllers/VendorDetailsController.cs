using Microsoft.AspNetCore.Mvc;
using Model.Requests;
using Model;
using Repository;
using Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace VendorManagement_WebApi.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class VendorDetailsController : Controller
    {
        public VendorDetailsServices vendorDetailsServices;
        public VendorDetailsController(DbContextAccess dbContextAccess) {
            vendorDetailsServices = new VendorDetailsServices(dbContextAccess);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertVendorDetails(VendorDetailsRequest vendorDetailsRequest)
        {
            if(vendorDetailsRequest != null && vendorDetailsRequest.ProductDetailsRequest.Count>0) {
                return Ok(vendorDetailsServices.InsertVendorDetails(vendorDetailsRequest));
            }
            else
            {
                return BadRequest("Invalid vendor details");
            }

        }

        [HttpGet]
        [Route("/get/count/vendors")]
        public async Task<IActionResult> CountOfVendors()
        {
            return Ok(vendorDetailsServices.CountOfVendors());
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorWithProductDetails()
        {
            return Ok(vendorDetailsServices.GetVendorDetails());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetVendor([FromRoute] Guid id)
        {
            var vendor = vendorDetailsServices.GetVendor(id);
            if( vendor!= null)
            {
                return Ok(vendor);
            }
            return NotFound("Vendor not found");
        }


        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVendor([FromRoute] Guid id)
        {
            return Ok(vendorDetailsServices.DeleteVendor(id));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVendor([FromRoute] Guid id, VendorDetailsUpdateRequest vendorDetailsUpdateRequest)
        {
            if (vendorDetailsUpdateRequest != null && vendorDetailsUpdateRequest.ProductDetailsRequest.Count>0)
            {
                return Ok(vendorDetailsServices.UpdateVendor(id, vendorDetailsUpdateRequest));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
