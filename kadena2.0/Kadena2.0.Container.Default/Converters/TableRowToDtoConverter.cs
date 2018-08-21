using System.Collections.Generic;
using System.Dynamic;
using AutoMapper;
using Kadena.Dto.Common;
using Kadena.Models.Common;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.OrderReport;

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

                if (i == 10)
                {
                    var value = context.Mapper.Map<TrackingFieldDto>(source.Items[10]);
                    itemsDictionary.Add(dictionaryKey, value);
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
