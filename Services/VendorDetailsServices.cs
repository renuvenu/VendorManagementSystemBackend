using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Requests;
using Repository;


namespace Services
{
    public class VendorDetailsServices
    {
        private readonly DbContextAccess dbContextAccess;


        public VendorDetailsServices(DbContextAccess dbContextAccess)
        {
            this.dbContextAccess = dbContextAccess;
        }


        //public async Task<IActionResult> InsertVendorDetails(VendorDetailsRequest vendorDetailsRequest)
        //{
        //    ProductDetail productDetail = new ProductDetail();
        //    VendorDetails vendorDetails = new VendorDetails();
        //    vendorDetails.Id = new Guid();
        //    vendorDetails.VendorName = vendorDetailsRequest.VendorName;
        //    vendorDetails.IsActive = true;
        //    vendorDetails.AddressLine1 = vendorDetailsRequest.AddressLine1;
        //    vendorDetails.AddressLine2 = vendorDetailsRequest.AddressLine2;
        //    vendorDetails.City = vendorDetailsRequest.City;
        //    vendorDetails.State = vendorDetailsRequest.State;
        //    vendorDetails.PostalCode = vendorDetailsRequest.PostalCode;
        //    vendorDetails.Country = vendorDetailsRequest.Country;
        //    vendorDetails.TelePhone1 = vendorDetailsRequest.TelePhone1;
        //    vendorDetails.TelePhone2 = vendorDetailsRequest.TelePhone2;
        //    vendorDetails.VendorEmail = vendorDetailsRequest.VendorEmail;
        //    vendorDetails.VendorWebsite = vendorDetailsRequest.VendorWebsite;



        //    await dbContextAccess.VendorDetails.AddAsync(vendorDetails);
        //    await dbContextAccess.SaveChangesAsync();


        //    //productdetailscontroller productdetailscontroller = new productdetailscontroller(dbcontextaccess);
        //    //vendordetailsrequest.productdetailsrequest.foreach(data =>
        //    //{
        //    //    data.vendorid = vendordetails.id;
        //    //    productdetailscontroller.insertproductdetail(data);
        //    //});
        //    //  VendorDetailsServices vendorDetailsServices=new VendorDetailsServices();
        //    //   res= await vendorDetailsServices.InsertVendorDetails(vendorDetailsRequest);

        //    return Ok(vendorDetails);

        //}




    }


}
