using Kadena.Dto.ViewOrder.Responses;

namespace Kadena.Dto.OrderManualUpdate.Responses
{
    public class OrderUpdateResultDto
    {
        public TitleValuePairDto<string>[] PricingInfo { get; set; }
        public ItemUpdateResultDto[] OrdersPrice { get; set; }
    }
}
