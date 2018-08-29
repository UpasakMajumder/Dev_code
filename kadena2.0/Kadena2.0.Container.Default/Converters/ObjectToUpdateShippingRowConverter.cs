using System;
using System.Linq;
using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Orders;
using Newtonsoft.Json.Linq;

namespace Kadena.Container.Default.Converters
{
    public class ObjectToUpdateShippingRowConverter : ITypeConverter<object, UpdateShippingRow>
    {
        private static string[] _headers;
        public UpdateShippingRow Convert(object source, UpdateShippingRow destination, ResolutionContext context)
        {
            if(_headers == null)
                _headers = DIContainer.Resolve<IOrderReportFactoryHeaders>().GetCodeNameHeaders();

            var jObject = source as JObject;
            if (jObject == null)
                throw new InvalidCastException(nameof(source));

            var result = new UpdateShippingRow
            {
                LineNumber = jObject.SelectToken(_headers[0]).Value<int>(),
                OrderNumber = jObject.SelectToken(_headers[2]).Value<string>(),
                QuantityShipped = jObject.SelectToken(_headers[12]).Value<int>(),
                ShippingDate = jObject.SelectToken(_headers[10]).Value<string>(),
                ShippingMethod = jObject.SelectToken(_headers[13]).Value<string>(),
                TrackingInfoId = jObject.SelectToken(_headers[14]).Value<string>(),
                TrackingNumber = jObject.SelectToken(_headers[11]).Value<string>()
            };

            return result;
        }
    }
}
