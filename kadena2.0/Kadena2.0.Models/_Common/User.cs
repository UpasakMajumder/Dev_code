using System;

namespace Kadena.Models
{
    public class User
    {
        public int UserId { get; set; }
        public DateTime TermsConditionsAccepted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsExternal { get; set; }
    }
}