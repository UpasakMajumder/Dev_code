using Kadena.Dto.Common;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderHistoryDto
    {
        public string Title { get; set; }
        public TitledMessageDto Message { get; set; }
        public OrderHistoryChangesDto ItemChanges { get; set; }
        public OrderHistoryChangesDto OrderChanges { get; set; }
    }
}