using System;

namespace Kadena.Dto.MailingList
{
    public class UpdateAddressDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string FirstAddressLine { get; set; }
        public string SecondAddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
