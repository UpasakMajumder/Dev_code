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
        public TableRowDto Convert(TableRow source, TableRowDto destination, ResolutionContext context)
        {
            var result = new TableRowDto
            {
                Url = source.Url
            };

            dynamic items = new ExpandoObject();
            var itemsDictionary = (IDictionary<string, object>)items;
            var headers = DIContainer.Resolve<IOrderReportFactoryHeaders>().GetCodeNameHeaders();

            for (var i = 0; i < source.Items.Length; i++)
            {
                var dictionaryKey = headers[i];

                if (i == 11)
                {
                    var trackingInfos = source.Items[i] as IEnumerable<TrackingInfo>;
                    var trackingInfo = trackingInfos?.FirstOrDefault();

                    var trackingValue = new
                    {
                        Type = "tracking",
                        Value = trackingInfo?.Id ?? string.Empty,
                        trackingInfo?.Url
                    };
                    itemsDictionary.Add(dictionaryKey, trackingValue);

                    var shippedValue = new
                    {
                        Value = trackingInfo?.QuantityShipped ?? 0
                    };
                    itemsDictionary.Add(headers[i + 1], shippedValue);

                    var methodValue = new
                    {
                        Value = trackingInfo?.ShippingMethod.ShippingService ?? string.Empty
                    };
                    itemsDictionary.Add(headers[i + 2], methodValue);
                    break;
                }

                var dictionaryValue = new
                {
                    Value = source.Items[i]
                };

                itemsDictionary.Add(dictionaryKey, dictionaryValue);
            }

            result.Items = items;

            return result;
        }
    }
}
