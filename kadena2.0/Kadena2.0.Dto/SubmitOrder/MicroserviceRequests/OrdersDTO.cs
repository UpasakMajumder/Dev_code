using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
   public class OrdersDTO 
    {
        public string Type { get; set; }
        public CampaignDTO Campaign { get; set; }
        public string OrderID { get; set; }

        public AddressDTO BillingAddress { get; set; }

        public AddressDTO ShippingAddress { get; set; }

        public ShippingOptionDTO ShippingOption { get; set; }

        public decimal? TotalShipping { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalTax { get; set; }

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

        public IEnumerable<NotificationInfoDto> NotificationsData { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}