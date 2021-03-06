﻿using System.Runtime.Serialization;

namespace Kadena.Dto.Order
{
    [DataContract]
    public class OrderItemDto
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }
    }
}