using AutoMapper;
using System;

namespace Kadena.Container.Default.Converters
{
    public class StringToNullableDateTimeConverter : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            if (DateTime.TryParse(source, out var deliveryDate))
            {
                DateTime? nullableDeliveryDate = deliveryDate;
                return nullableDeliveryDate;
            }

            return null;
        }
    }

}
