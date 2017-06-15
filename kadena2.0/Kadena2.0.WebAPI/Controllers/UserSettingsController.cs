using AutoMapper;
using Kadena.Dto.Settings;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using Kadena.WebAPI.Models;
using CMS.Helpers;

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
                //////Uncomment when billing addresses will be developed
                ////new 
                ////{
                ////    Title = ResHelper.GetString("Kadena.Settings.Addresses.BillingAddress"),
                ////    AddButton = new 
                ////    {
                ////        Exists = false,
                ////        Tooltip = ResHelper.GetString("Kadena.Settings.Addresses.AddBilling")
                ////    },
                ////    Addresses = _mapper.Map<List<AddressDto>>(billingAddresses)
                ////},
                Shipping = new
                {
                    Title = ResHelper.GetString("Kadena.Settings.Addresses.ShippingAddresses"),
                    AddButton = new
                    {
                        Exists = false,
                        Tooltip = ResHelper.GetString("Kadena.Settings.Addresses.AddShipping")
                    },
                    EditButtonText = ResHelper.GetString("Kadena.Settings.Addresses.Edit"),
                    RemoveButtonText = ResHelper.GetString("Kadena.Settings.Addresses.Remove"),
                    Addresses = _mapper.Map<List<AddressDto>>(shippingAddresses)
                },
                Dialog = new
                {
                    Types = new
                    {
                        Add = ResHelper.GetString("Kadena.Settings.Addresses.AddAddress"),
                        Edit = ResHelper.GetString("Kadena.Settings.Addresses.EditAddress")
                    },
                    Buttons = new
                    {
                        Discard = ResHelper.GetString("Kadena.Settings.Addresses.DiscardChanges"),
                        Save = ResHelper.GetString("Kadena.Settings.Addresses.SaveAddress")
                    },
                    Fields = new List<EditorFieldDto> {
                        new EditorFieldDto { Id="street1",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.AddressLine1"),
                            Type = "text"},
                        new EditorFieldDto { Id="street2",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.AddressLine2"),
                            Type = "text"},
                        new EditorFieldDto { Id="city",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.City"),
                            Type = "text"},
                        new EditorFieldDto { Id="state",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.State"),
                            Type = "select",
                            Values = states.Select(s=>(object)s.StateCode).ToList() },
                        new EditorFieldDto { Id="zip",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.Zip"),
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
            var result = new { Id = addressModel.Id };
            return ResponseJson(result);
        }
    }
}