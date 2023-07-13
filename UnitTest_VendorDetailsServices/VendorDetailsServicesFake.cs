using Model.Requests;
using Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace UnitTest_VendorDetailsServices
{
    public class VendorDetailsServicesFake: InterfaceVendorDetailsService
        
    {
        private  List<VendorDetails> _vendorDetailsList;
        public VendorDetailsServicesFake()
        {
            _vendorDetailsList = new List<VendorDetails>();
            

                _vendorDetailsList = new List<VendorDetails>()
                {
                    new VendorDetails()
                    {

                        VendorName = "ABC",
                        IsActive = true,
                        AddressLine1 = "addsa",
                        VendorType=0,
                        AddressLine2 = "kguhgh",
                        City = "hgh",
                        State = "hljh",
                        PostalCode = "uyghu",
                        Country = "jhguhg",
                        TelePhone1 = "kjhjh",
                        TelePhone2 = "jhg",
                        VendorEmail = "jhgfyg",
                        VendorWebsite = "kjhghj"
                    },

                    new VendorDetails()
                    {
                        VendorName = "ABC",
                        IsActive = true,
                        AddressLine1 = "addsa",
                        AddressLine2 = "kguhgh",
                        VendorType=0,
                        City = "hgh",
                        State = "hljh",
                        PostalCode = "uyghu",
                        Country = "jhguhg",
                        TelePhone1 = "kjhjh",
                        TelePhone2 = "jhg",
                        VendorEmail = "jhgfyg",
                        VendorWebsite = "kjhghj"
                    }
                };
        }
        public VendorDetails InsertVendorDetails(VendorDetailsRequest vendorDetailsRequest)
        {
         
             _vendorDetailsList.InsertVendorDetails(vendorDetailsRequest);
            return _vendorDetailsList;
            throw new NotImplementedException();

        }

        VendorDetails InterfaceVendorDetailsService.DeleteVendor(Guid id)
        {
            throw new NotImplementedException();
        }

        VendorDetailswithProductDetailsRequest InterfaceVendorDetailsService.GetVendor(Guid id)
        {
            throw new NotImplementedException();
        }

        List<VendorDetailswithProductDetailsRequest> InterfaceVendorDetailsService.GetVendorDetails()
        {
            throw new NotImplementedException();
        }

        VendorDetailswithProductDetailsRequest InterfaceVendorDetailsService.UpdateVendor(Guid id, VendorDetailsUpdateRequest vendorDetailsUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
