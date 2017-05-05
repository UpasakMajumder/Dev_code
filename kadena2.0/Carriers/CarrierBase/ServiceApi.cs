using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.CarrierBase
{
    public class Address
    {
        public List<string> streetLines { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal { get; set; }
        public string country { get; set; }
    }

    public class Weight
    {
        public string unit { get; set; }
        public double value { get; set; }
    }

    public class EstimateDeliveryPriceRequest
    {
        public Address sourceAddress { get; set; }
        public Address targetAddress { get; set; }
        public Weight weight { get; set; }
        public string provider { get; set; }
        public string providerService { get; set; }
    }

    public class Response
    {
        public bool serviceSuccess { get; set; }
        public double cost { get; set; }
        public string currency { get; set; }
        public object errorMessage { get; set; }
    }

    public class EstimateDeliveryPriceResponse
    {
        public bool success { get; set; }
        public EstimateDeliveryPricePayload payload { get; set; }
        public string errorMessages { get; set; }
    }

    public class EstimateDeliveryPricePayload
    {
        public decimal cost { get; set; }
        public string currency { get; set; }
    }
}
