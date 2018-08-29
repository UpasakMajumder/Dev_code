using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AutoMapper;
using Kadena.Dto.Common;
using Kadena.Models.Common;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Shipping;

namespace Kadena.Container.Default.Converters
{
    public class TableRowToDtoConverter : ITypeConverter<TableRow, TableRowDto>
    {
        private static string[] _headers;

        public TableRowDto Convert(TableRow source, TableRowDto destination, ResolutionContext context)
        {
            if(_headers == null)
                _headers = DIContainer.Resolve<IOrderReportFactoryHeaders>().GetCodeNameHeaders();
            var result = new TableRowDto
            {
                Url = source.Url
            };

            dynamic items = new ExpandoObject();
            var itemsDictionary = (IDictionary<string, object>)items;
            

            for (var i = 0; i < source.Items.Length; i++)
            {
                var dictionaryKey = _headers[i];

                if (i == 11)
                {
                    var trackingInfos = source.Items[i] as IEnumerable<TrackingInfo>;
                    var trackingInfo = trackingInfos?.FirstOrDefault();

                    var trackingValue = new
                    {
                        Type = "tracking",
                        Value = trackingInfo?.Id ?? string.Empty,
                        Url = trackingInfo?.Url ?? string.Empty
                    };
                    itemsDictionary.Add(dictionaryKey, trackingValue);

                    var shippedValue = new
                    {
                        Value = trackingInfo?.QuantityShipped ?? 0
                    };
                    itemsDictionary.Add(_headers[i + 1], shippedValue);

                    var methodValue = new
                    {
                        Value = trackingInfo?.ShippingMethod.ShippingService ?? string.Empty
                    };
                    itemsDictionary.Add(_headers[i + 2], methodValue);

                    var trackingInfoIdValue = new
                    {
                        Value = trackingInfo?.ItemId ?? string.Empty
                    };
                    itemsDictionary.Add(_headers[i + 3], trackingInfoIdValue);

                    break;
                }

                var dictionaryValue = new
                {
                    Value = source.Items[i] ?? string.Empty
                };

                itemsDictionary.Add(dictionaryKey, dictionaryValue);
            }

            result.Items = items;

            return result;
        }
    }
}
