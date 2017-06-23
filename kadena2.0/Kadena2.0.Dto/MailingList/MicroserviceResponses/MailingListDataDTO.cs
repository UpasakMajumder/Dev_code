using System;
using System.Runtime.Serialization;

namespace Kadena.Dto.MailingList.MicroserviceResponses
{
    [DataContract]
    public class MailingListDataDTO //TODO remove DataMembers and use sereialize config
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "customerName")]
        public string CustomerName { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "createDate")]
        public DateTime CreateDate { get; set; }

        [DataMember(Name = "updateDate")]
        public DateTime UpdateDate { get; set; }

        [DataMember(Name = "validTo")]
        public DateTime ValidTo { get; set; }

        [DataMember(Name = "state")]
        public object State { get; set; }

        [DataMember(Name = "mailType")]
        public string MailType { get; set; }

        [DataMember(Name = "productType")]
        public string ProductType { get; set; }

        [DataMember(Name = "version")]
        public int Version { get; set; }

        [DataMember(Name = "addressCount")]
        public int AddressCount { get; set; }

        [DataMember(Name = "errorCount")]
        public int ErrorCount { get; set; }
    }
}