using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Globalization;

namespace Kadena.BusinessLogic.Services
{
    public class DateTimeFormatter : IDateTimeFormatter
    {
        private readonly IKenticoLocalizationProvider localization;

        private const string defaultFormatString = "MMM dd, yyyy";

        private Dictionary<string,string> customFormatStrings => new Dictionary<string, string>
        {
            {"ja-JP" , "M\u6708 dd, yyyy"}
        };

        public DateTimeFormatter(IKenticoLocalizationProvider localization)
        {
            if (localization == null)
            {
                throw new ArgumentNullException(nameof(localization));
            }

            this.localization = localization;
        }

        public string GetFormatString()
        {
            var culture = localization.GetContextCultureCode();
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
            var culture = localization.GetContextCultureCode();
            var formatProvider =  new CultureInfo(culture);
            var formatString = GetFormatString(culture);
            return dt.ToString(formatString, formatProvider);
        }
    }
}
