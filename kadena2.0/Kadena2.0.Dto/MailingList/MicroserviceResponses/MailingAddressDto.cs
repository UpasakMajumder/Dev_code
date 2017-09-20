using System;
using System.Runtime.Serialization;

namespace Kadena.Dto.MailingList.MicroserviceResponses
{
    [DataContract]
    public class MailingAddressDto
    {
        [DataMember(IsRequired = true, Name = "id")]
        public Guid Id { get; set; }

        [DataMember(IsRequired = true, Name = "mailingContainerId")]
        public Guid ContainerId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "address1")]
        public string Address1 { get; set; }

        [DataMember(Name = "address2")]
        public string Address2 { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "zip")]
        public string Zip { get; set; }

        [DataMember(IsRequired = true, Name = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}