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

            var result = new SettingsAddressesDto
            {
                Billing = new AddressBlockDto
                {
                    Title = "Billing addresses",
                    AddButton = new ButtonDto
                    {
                        Exists = false,
                        Tooltip = "Add new shipping address"
                    },
                    Addresses = _mapper.Map<List<AddressDto>>(billingAddresses)
                },
                Shipping = new AddressBlockDto
                {
                    Title = "Shipping addresses",
                    AddButton = new ButtonDto
                    {
                        Exists = false,
                        Tooltip = "Add new shipping address"
                    },
                    Addresses = _mapper.Map<List<AddressDto>>(shippingAddresses)
                },
                Dialog = new EditorDto
                {
                    Types = new EditorTypeDto
                    {
                        Add = "Add address:",
                        Edit = "Edit address:"
                    },
                    Buttons = new EditorButtonDto
                    {
                        Discard = "Discard changes",
                        Save = "Save address"
                    },
                    Fields = new List<EditorFieldDto> {
                        new EditorFieldDto { Label = "Address line 1", Type = "text"},
                        new EditorFieldDto { Label = "Address line 2", Type = "text"},
                        new EditorFieldDto { Label = "City", Type = "text"},
                        new EditorFieldDto { Label = "State", Type = "select",
                            Values = states.Select(s=>(object)s.StateCode).ToList() },
                        new EditorFieldDto { Label = "Zip code", Type = "text"}
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