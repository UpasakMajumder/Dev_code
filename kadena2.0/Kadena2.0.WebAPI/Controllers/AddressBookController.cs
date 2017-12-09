using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class AddressBookController : ApiControllerBase
    {
        private readonly IAddressBookService addressBookService;
        private readonly IMapper mapper;

        public AddressBookController(IAddressBookService addressBookService, IMapper mapper)
        {
            if (addressBookService == null)
            {
                throw new ArgumentNullException(nameof(addressBookService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.addressBookService = addressBookService;
        }

        [HttpDelete]
        [Route("api/deleteaddress/{addressID}")]
        public IHttpActionResult DeleteAddress(int addressID)
        {
            addressBookService.DeleteAddress(addressID);
            return ResponseJson<string>("OK");
        }
    }
}
