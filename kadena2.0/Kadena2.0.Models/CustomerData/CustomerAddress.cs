using System.Collections.Generic;

namespace Kadena.Models.CustomerData
{
    public class CustomerAddress
    {
        public List<string> Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
    }
}