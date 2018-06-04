using System;

namespace Kadena.Models.Orders
{
    public class OrderNumber
    {
        public OrderNumber(int orderCustomerId, int orderUserId, string sequenceNumber)
        {
            UserId = orderUserId;
            CustomerId = orderCustomerId;
            SequenceNumber = sequenceNumber;
        }

        public int UserId { get; }
        public int CustomerId { get; }
        public string SequenceNumber { get; }

        public static OrderNumber Parse(string orderId)
        {
            var orderIdparts = orderId.Split(new char[] { '-' }, count: 3);
            if (orderIdparts.Length != 3 
                || !int.TryParse(orderIdparts[0], out var orderCustomerId) 
                || !int.TryParse(orderIdparts[1], out var orderUserId))
            {
                throw new ArgumentOutOfRangeException(nameof(orderId), "Bad format of order ID");
            }

            return new OrderNumber(orderCustomerId, orderUserId, orderIdparts[2]);
        }

        public override string ToString()
        {
            return $"{CustomerId}-{UserId}-{SequenceNumber}";
        }

        public static implicit operator string(OrderNumber on)
        {
            return on.ToString();
        }
    }
}
