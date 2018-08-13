using Kadena.Models.Common;

namespace Kadena.Models.OrderHistory
{
    public class OrderHistory
    {
        public string Title { get; set; }
        public TitledMessage Message { get; set; }
        public OrderChanges OrderChanges { get; set; }
        public ItemChanges ItemChanges { get; set; }
    }
}
