using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.MailingList.MicroserviceResponses
{
    public enum ContainerState
    {
        [Display(Name = "Ready")]
        Ready = 0,

        [Display(Name = "Addresses need to be verified")]
        AddressesNeedVerification = 5,

        [Display(Name = "Addresses on verification")]
        AddressesOnVerification = 10,

        [Display(Name = "Addresses verified")]
        AddressesVerified = 20
    }
}
