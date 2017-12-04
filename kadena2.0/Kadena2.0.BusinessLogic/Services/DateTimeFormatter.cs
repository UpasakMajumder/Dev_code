using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Globalization;

namespace Kadena.BusinessLogic.Services
{
    public class DateTimeFormatter : IDateTimeFormatter
    {
        private readonly IKenticoResourceService resources;

        private readonly string defaultFormatString = "MMM dd, yyyy";

        private Dictionary<string,string> customFormatStrings => new Dictionary<string, string>
        {
            {"ja-JP" , "M\u6708 dd, yyyy"}
        };

        public DateTimeFormatter(IKenticoResourceService resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            this.resources = resources;
        }

        public string GetFormatString()
        {
            var culture = resources.GetContextCultureCode();
            return GetFormatString(culture);
        }

        public string GetFormatString(string cultureCode)
        {
            string formatString;
            if (!customFormatStrings.TryGetValue(cultureCode, out formatString))
            {
                formatString = defaultFormatString;
            }
            return formatString;
        }

        public string Format(DateTime dt)
        {
            var culture = resources.GetContextCultureCode();
            var formatProvider =  new CultureInfo(culture);
            var formatString = GetFormatString(culture);
            return dt.ToString(formatString, formatProvider);
        }
    }
}
