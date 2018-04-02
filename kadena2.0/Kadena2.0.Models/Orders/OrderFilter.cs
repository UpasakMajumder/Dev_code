using System;

namespace Kadena.Models.Orders
{
    public struct OrderFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string OrderByExpression { get; set; }

        public bool TryParseOrderByExpression(out OrderByFields orderByInfo)
        {
            orderByInfo = null;
            if (string.IsNullOrWhiteSpace(OrderByExpression))
            {
                return false;
            }

            var parts = OrderByExpression.Split('-');
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
            OrderByDirection directionTyped;
            if (!Enum.TryParse(direction, out directionTyped))
            {
                return false;
            }

            orderByInfo = new OrderByFields
            {
                Property = prop,
                Direction = directionTyped
            };
            return true;
        }

        public enum OrderByDirection { ASC, DESC }
        public class OrderByFields
        {
            public string Property { get; set; }
            public OrderByDirection Direction { get; set; }
        }
    }
}
