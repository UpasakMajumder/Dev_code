using System;

namespace Kadena.Models.Orders
{
    public struct OrderFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Sort { get; set; }

        public bool TryParseSort(out SortFields sortInfo)
        {
            sortInfo = null;
            if (string.IsNullOrWhiteSpace(Sort))
            {
                return false;
            }

            var parts = Sort.Split('-');
            if (parts.Length != 2)
            {
                return false;
            }

            var prop = parts[0];
            if (string.IsNullOrWhiteSpace(prop))
            {
                return false;
            }

            var direction = parts[1].ToUpper();
            SortDirection directionTyped;
            if (!Enum.TryParse(direction, out directionTyped))
            {
                return false;
            }

            sortInfo = new SortFields
            {
                Property = prop,
                Direction = directionTyped
            };
            return true;
        }

        public enum SortDirection { ASC, DESC }
        public class SortFields
        {
            public string Property { get; set; }
            public SortDirection Direction { get; set; }
        }
    }
}
