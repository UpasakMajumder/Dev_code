using AutoMapper;
using Kadena.Dto.Settings;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Controllers
{
    public class UserSettingsController : ApiControllerBase
    {
        private readonly IKenticoProviderService _service;
        private readonly IMapper _mapper;

        public UserSettingsController(IKenticoProviderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/usersettings")]
        public IHttpActionResult Get()
        {
            var billingAddresses = _service.GetCustomerAddresses("Billing");
            var shippingAddresses = _service.GetCustomerAddresses("Shipping");
            var states = _service.GetStates();

            var result = new
            {
                Billing = new object(),
                //////Uncomment billing addresses will developed
                ////new 
                ////{
                ////    Title = _service.GetResourceString("Kadena.Settings.Addresses.BillingAddress"),
                ////    AddButton = new 
                ////    {
                ////        Exists = false,
                ////        Tooltip = _service.GetResourceString("Kadena.Settings.Addresses.AddBilling")
                ////    },
                ////    Addresses = _mapper.Map<List<AddressDto>>(billingAddresses)
                ////},
                Shipping = new 
                {
                    Title = _service.GetResourceString("Kadena.Settings.Addresses.ShippingAddresses"),
                    AddButton = new 
                    {
                        Exists = false,
                        Tooltip = _service.GetResourceString("Kadena.Settings.Addresses.AddShipping")
                    },
                    Addresses = _mapper.Map<List<AddressDto>>(shippingAddresses)
                },
                Dialog = new 
                {
                    Types = new 
                    {
                        Add = _service.GetResourceString("Kadena.Settings.Addresses.AddAddress"),
                        Edit = _service.GetResourceString("Kadena.Settings.Addresses.EditAddress")
                    },
                    Buttons = new 
                    {
                        Discard = _service.GetResourceString("Kadena.Settings.Addresses.DiscardChanges"),
                        Save = _service.GetResourceString("Kadena.Settings.Addresses.SaveAddress")
                    },
                    Fields = new List<EditorFieldDto> {
                        new EditorFieldDto { Label = _service.GetResourceString("Kadena.Settings.Addresses.AddressLine1"),
                            Type = "text"},
                        new EditorFieldDto { Label = _service.GetResourceString("Kadena.Settings.Addresses.AddressLine2"),
                            Type = "text"},
                        new EditorFieldDto { Label = _service.GetResourceString("Kadena.Settings.Addresses.City"),
                            Type = "text"},
                        new EditorFieldDto { Label = _service.GetResourceString("Kadena.Settings.Addresses.State"),
                            Type = "select",
                            Values = states.Select(s=>(object)s.StateCode).ToList() },
                        new EditorFieldDto { Label = _service.GetResourceString("Kadena.Settings.Addresses.Zip"),
                            Type = "text"}
                    }
                }
            };

            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/usersettings/saveshippingaddress")]
        public IHttpActionResult SaveShippingAddress([FromBody] AddressDto address)
        {
            var addressModel = _mapper.Map<DeliveryAddress>(address);
            _service.SaveShippingAddress(addressModel);
            return ResponseJson("Address saved.");
        }
    }
}