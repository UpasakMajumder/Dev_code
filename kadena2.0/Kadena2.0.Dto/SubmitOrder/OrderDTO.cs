using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder
{
    public class OrderDTO
    {
        /// <summary>
        ///  empty
        /// </summary>
         public string OrderID { get; set; }
         
         
         public AddressDTO BillingAddress { get; set; }

         public AddressDTO ShippingAddress { get; set; }

         public ShippingOptionDTO ShippingOption { get; set; }

         public double? TotalShipping { get; set; }

         public double TotalPrice { get; set; }

         public double TotalTax { get; set; }

         public DateTime OrderDate { get; set; }

         public OrderStatusDTO OrderStatus { get; set; }

         public CurrencyDTO OrderCurrency { get; set; }

         public CustomerDTO Customer { get; set; }

         public int? KenticoOrderCreatedByUserID { get; set; }

         public string OrderNote { get; set; }

         public SiteDTO Site { get; set; }

         public PaymentOptionDTO PaymentOption { get; set; }

         public OrderTrackingDTO OrderTracking { get; set; }

         public PaymentResultDTO PaymentResult { get; set; }

         public DateTime LastModified { get; set; }

         public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
