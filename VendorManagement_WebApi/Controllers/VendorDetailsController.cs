using Microsoft.AspNetCore.Mvc;
using Model.Requests;
using Model;
using Repository;
using Services;
using Microsoft.EntityFrameworkCore;

namespace VendorManagement_WebApi.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class VendorDetailsController : Controller
    {
        private readonly DbContextAccess dbContextAccess;

        public VendorDetailsController(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
        }
        [HttpGet]
        public async Task<IActionResult> GetLiftFunctionDetails()
        {
            return Ok(await dbContextAccess.VendorDetails.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> InsertVendorDetails(VendorDetailsRequest vendorDetailsRequest)
        {
           // ProductDetail productDetail=new ProductDetail();
            VendorDetails vendorDetails = new VendorDetails();
            vendorDetails.Id = new Guid();
            vendorDetails.VendorName = vendorDetailsRequest.VendorName;
            vendorDetails.IsActive = true;
            vendorDetails.AddressLine1 = vendorDetailsRequest.AddressLine1;
            vendorDetails.AddressLine2 = vendorDetailsRequest.AddressLine2;
            vendorDetails.City = vendorDetailsRequest.City;
            vendorDetails.State = vendorDetailsRequest.State;
            vendorDetails.PostalCode = vendorDetailsRequest.PostalCode;
            vendorDetails.Country = vendorDetailsRequest.Country;
            vendorDetails.TelePhone1 = vendorDetailsRequest.TelePhone1;
            vendorDetails.TelePhone2 = vendorDetailsRequest.TelePhone2;
            vendorDetails.VendorEmail = vendorDetailsRequest.VendorEmail;
            vendorDetails.VendorWebsite = vendorDetailsRequest.VendorWebsite;



            await dbContextAccess.VendorDetails.AddAsync(vendorDetails);
            await dbContextAccess.SaveChangesAsync();


            ProductDetailsController productDetailsController = new ProductDetailsController(dbContextAccess);
            vendorDetailsRequest.ProductDetailsRequest.ForEach(data =>
            {
                data.VendorId = vendorDetails.Id;
                productDetailsController.InsertProductDetail(data);
            });
          //  VendorDetailsServices vendorDetailsServices=new VendorDetailsServices();
         //   res= await vendorDetailsServices.InsertVendorDetails(vendorDetailsRequest);

            return Ok(vendorDetails);

        }




        [HttpPut]
        [Route("editvendor/{id:guid}")]
        public async Task<IActionResult> UpdatePersonDetails([FromRoute] Guid id, VendorDetailsRequest updatevendorDetails)
        {
            if (updatevendorDetails != null)
            {
                var vendorDetails = await dbContextAccess.VendorDetails.FirstOrDefaultAsync(x => x.Id == id);

                if (vendorDetails != null)
                {

                    vendorDetails.VendorName = updatevendorDetails.VendorName;
                    vendorDetails.IsActive = true;
                    vendorDetails.AddressLine1 = updatevendorDetails.AddressLine1;
                    vendorDetails.AddressLine2 = updatevendorDetails.AddressLine2;
                    vendorDetails.City = updatevendorDetails.City;
                    vendorDetails.State = updatevendorDetails.State;
                    vendorDetails.PostalCode = updatevendorDetails.PostalCode;
                    vendorDetails.Country = updatevendorDetails.Country;
                    vendorDetails.TelePhone1 = updatevendorDetails.TelePhone1;
                    vendorDetails.TelePhone2 = updatevendorDetails.TelePhone2;
                    vendorDetails.VendorEmail = updatevendorDetails.VendorEmail;
                    vendorDetails.VendorWebsite = updatevendorDetails.VendorWebsite;

                    dbContextAccess.VendorDetails.Update(vendorDetails);
                    await dbContextAccess.SaveChangesAsync();
                    ProductDetailsController productDetailsController = new ProductDetailsController(dbContextAccess);
                    updatevendorDetails.ProductDetailsRequest.ForEach(data =>
                    {
                        productDetailsController.UpdateProductDetail(id,data);
                    });
                }

                return Ok(vendorDetails);
            }
            else
            {
                return BadRequest("Not available");
            }
        }

        [HttpGet]
        [Route("Vendor/Product")]
        public async Task<IActionResult> GetVendorDetailsWithProductDetails()
        {
            
            var vendorDetailsWithProductDetails = await dbContextAccess.VendorDetails
                .Select(vendor => new VendorDetailswithProductDetailsRequest
                {
                    VendorDetails = vendor,
                    ProductDetails= dbContextAccess.productDetails.Where(p => p.VendorId == vendor.Id).ToList()
                })
                .ToListAsync();

            return Ok(vendorDetailsWithProductDetails);
        }

        [HttpGet]
        [Route("Vendor/Product/{vendorId}")]
        public async Task<IActionResult> GetVendorWithProductDetails(Guid vendorId)
        {
            var vendorWithProductDetails = await dbContextAccess.VendorDetails
                .Where(v => v.Id == vendorId)
                .Select(vendor => new VendorDetailswithProductDetailsRequest
                {
                    VendorDetails = vendor,
                    ProductDetails = dbContextAccess.productDetails.Where(p => p.VendorId == vendor.Id).ToList()
                })
                .SingleOrDefaultAsync();

            if (vendorWithProductDetails == null)
            {
                return NotFound(); 
            }

            return Ok(vendorWithProductDetails);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteVendorDetails([FromRoute] Guid id)
        {
            var functionDetail = dbContextAccess.VendorDetails.FirstOrDefault(x => x.Id == id);
            if (functionDetail != null)
            {
                functionDetail.IsActive = false;
                dbContextAccess.VendorDetails.Update(functionDetail);
                await dbContextAccess.SaveChangesAsync();
            }
            return Ok(functionDetail);
        }

        [HttpGet]
        [Route("/vendor/vendordetails/{userId}")]
        public async Task<IActionResult> DetailOfVendor([FromRoute] Guid userId)
        {
            bool person = dbContextAccess.VendorDetails.Any(x => x.Id == userId && x.IsActive == true);
            if (person)
            {
                var list = dbContextAccess.VendorDetails.Where(x => x.Id == userId);
                if (list.Count() > 0)
                {

                    return Ok(await list.ToListAsync());
                }
                return Ok("No Record found");
            }
            else
            {
                return BadRequest("Invalid User");
            }

        }
    
}
}
