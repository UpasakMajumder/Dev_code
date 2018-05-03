using System.Collections.Generic;

namespace Kadena.Dto.CustomerData
{
    public class CustomerDataDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerAddressDTO Address { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public string PreferredLanguage { get; set; }
        public ApproverDto[] Approvers { get; set; }
    }
}
