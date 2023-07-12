using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Requests;
using Repository;




namespace Services
{
    public class VendorDetailsServices
    {
        private readonly DbContextAccess dbContextAccess;

        public VendorDetailsServices()
        {
        }

        public VendorDetailsServices(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
        }

        public async Task<IActionResult> InsertVendorDetails(VendorDetailsRequest vendorDetailsRequest)
        {
            ProductDetail productDetail = new ProductDetail();
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


            ////ProductDetailsController productDetailsController = new ProductDetailsController(dbContextAccess);
            //vendorDetailsRequest.ProductDetailsRequest.ForEach(data =>
            //{
            //    data.VendorId = vendorDetails.Id;
            //    productDetailsController.InsertProductDetail(data);
            //});
            return Ok(vendorDetails);

        }

        private IActionResult Ok(VendorDetails vendorDetails)
        {
            throw new NotImplementedException();
        }
    }

    
}
