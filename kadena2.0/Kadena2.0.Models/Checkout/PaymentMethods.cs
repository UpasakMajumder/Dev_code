using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models.Checkout
{
    public class PaymentMethods
    {
        public bool IsPayable { get; set; }
        public string UnPayableText { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ApprovalRequiredText { get; set; }
        public string ApprovalRequiredDesc { get; set; }
        public string ApprovalRequiredButton { get; set; }

        public List<PaymentMethod> Items { get; set; }

        public void CheckPayability()
        {
            IsPayable = !Items.Any(i => i.IsUnpayable);
        }

        public void CheckDefault()
        {
            Items.ForEach(i => i.Checked = false);

            var unpayableMethod = Items.Where(m => m.IsUnpayable && !m.Disabled).FirstOrDefault();
            if (unpayableMethod != null)
            {
                unpayableMethod.Checked = true;
                return;
            }

            var defaultItem = Items.FirstOrDefault(i => i.Disabled == false);

            if (defaultItem != null)
            {
                defaultItem.Checked = true;
            }
        }
    }
}