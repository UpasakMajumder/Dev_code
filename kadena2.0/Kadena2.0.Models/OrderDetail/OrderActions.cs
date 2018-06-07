using Kadena.Models.Common;

namespace Kadena.Models.OrderDetail
{
    public class OrderActions
    {
        public DialogButton Accept { get; set; }
        public DialogButton Reject { get; set; }
        public TitleValuePair<string> Comment { get; set; }
    }
}
