using System;
using System.Collections.Generic;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class OrderDTO
    {
        public string OrderID { get; set; }
        public string Type { get; set; }
        public CampaignDTO Campaign { get; set; }
        public AddressDTO BillingAddress { get; set; }

        public AddressDTO ShippingAddressSource { get; set; }
        public AddressDTO ShippingAddressDestination { get; set; }

        public ShippingOptionDTO ShippingOption { get; set; }

        public TotalsDto Totals { get; set; }
        
        public DateTime OrderDate { get; set; }

        public OrderStatusDTO OrderStatus { get; set; }

        public CurrencyDTO OrderCurrency { get; set; }

        public CustomerDTO Customer { get; set; }

        public string OrderNote { get; set; }

        public SiteDTO Site { get; set; }

        public PaymentOptionDTO PaymentOption { get; set; }

        public IEnumerable<NotificationInfoDto> NotificationsData { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
