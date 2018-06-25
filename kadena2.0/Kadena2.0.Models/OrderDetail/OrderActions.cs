using Kadena.Models.Common;

namespace Kadena.Models.OrderDetail
{
    public class OrderActions
    {
        public DialogButton<Dialog> Accept { get; set; }
        public DialogButton<Dialog> Reject { get; set; }
        public TitleValuePair<string> Comment { get; set; }
    }
}
