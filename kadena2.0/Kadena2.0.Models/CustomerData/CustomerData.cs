using Kadena.Models.Approval;
using System.Collections.Generic;

namespace Kadena.Models.CustomerData
{
    public class CustomerData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerAddress Address { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public string PreferredLanguage { get; set; }
        public Approver[] Approvers { get; set; }
    }
}