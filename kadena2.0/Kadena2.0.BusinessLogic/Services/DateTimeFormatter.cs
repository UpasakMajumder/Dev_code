using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;

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

        public string Format(DateTime dt)
        {
            string formatString;
            var culture = resources.GetKenticoSite()?.DefaultCultureCode;
            if (!customFormatStrings.TryGetValue(culture, out formatString))
            {
                formatString = defaultFormatString;
            }
            return dt.ToString(formatString);
        }
    }
}
