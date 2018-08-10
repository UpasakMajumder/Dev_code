using System;

namespace Kadena.Dto.MailingList.MicroserviceResponses
{
    public class MailingListDataDTO
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ValidTo { get; set; }
        public ContainerState State { get; set; }
        public string MailType { get; set; }
        public string ProductType { get; set; }
        public int Version { get; set; }
        public int AddressCount { get; set; }
        public int ErrorCount { get; set; }
    }
}