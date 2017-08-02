using System;

namespace Kadena.Models
{
    public class MailingList
    {
        public string Id { get; set; }

        public string CustomerName { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime ValidTo { get; set; }

        public object State { get; set; }

        public string MailType { get; set; }

        public string ProductType { get; set; }

        public int Version { get; set; }

        public int AddressCount { get; set; }

        public int ErrorCount { get; set; }
    }
}
