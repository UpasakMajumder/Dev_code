using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Forms
{
    public class MediaMultiField
    {
        public static readonly char Separator = '|';

        public string Url { get; private set; }
        public string Name { get; private set; }

        public static string[] GetValues(string fieldValue)
        {
            if (fieldValue != null)
            {
                return fieldValue.Split(Separator);
            }

            return new string[0];
        }

        public static string CreateFieldValue(string[] values)
        {
            return string.Join(Separator.ToString(), values);
        }

        public static MediaMultiField ParseFrom(string value)
        {
            var urlParts = value.Split('/');
            var filenamePart = urlParts.Last();
            var filename = filenamePart.Split('?').First();

            return new MediaMultiField
            {
                Name = filename,
                Url = value
            };
        }

        public static MediaMultiField[] ParseMultipleFrom(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new MediaMultiField[0];
            }

            var values = GetValues(value);
            var items = values.Select(v => ParseFrom(v)).ToArray();
            return items;
        }
    }
}