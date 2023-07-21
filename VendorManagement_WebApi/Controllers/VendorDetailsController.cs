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
                var vendorDetails = await vendorDetailsServices.InsertVendorDetails(vendorDetailsRequest);
                return Ok(vendorDetails.Value);
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
            var count = await vendorDetailsServices.CountOfVendors();
            return Ok(count.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorWithProductDetails()
        {
            var vendors = await vendorDetailsServices.GetVendorDetails();
            return Ok(vendors.Value);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetVendor([FromRoute] Guid id)
        {
            var vendor = await vendorDetailsServices.GetVendor(id);
            if( vendor.Value!= null)
            {
                return Ok(vendor.Value);
            }
            return NotFound("Vendor not found");
        }


        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVendor([FromRoute] Guid id)
        {
            var vendor = await vendorDetailsServices.DeleteVendor(id);
            return Ok(vendor.Value);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVendor([FromRoute] Guid id, VendorDetailsUpdateRequest vendorDetailsUpdateRequest)
        {
            if (vendorDetailsUpdateRequest != null && vendorDetailsUpdateRequest.ProductDetailsRequest.Count>0)
            {
                var vendorDetails = await vendorDetailsServices.UpdateVendor(id, vendorDetailsUpdateRequest);
                return Ok(vendorDetails.Value);
            }
            else
            {
                return BadRequest("Invalid vendor");
            }
        }
    }
}
