using Kadena.Dto.Common;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderStatusInfoDTO
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public LinkDto OrderHistory { get; set; }
    }
}
