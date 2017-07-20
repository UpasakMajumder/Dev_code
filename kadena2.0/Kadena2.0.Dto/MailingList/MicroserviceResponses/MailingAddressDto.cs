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
        public string firstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "address1")]
        public string address1 { get; set; }

        [DataMember(Name = "address2")]
        public string address2 { get; set; }

        [DataMember(Name = "city")]
        public string city { get; set; }

        [DataMember(Name = "state")]
        public string state { get; set; }

        [DataMember(Name = "zip")]
        public string zip { get; set; }

        [DataMember(IsRequired = true, Name = "errorMessage")]
        public string Error { get; set; }
    }
}