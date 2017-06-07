using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.Models
{
    public class PaymentMethods
    {
        public bool IsPayable { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PaymentMethod> Items { get; set; }

        public void CheckDefault()
        {
            Items.ForEach(i => i.Checked = false);

            var defaultItem = Items.FirstOrDefault(i => i.Disabled == false);

            if (defaultItem != null)
            {
                defaultItem.Checked = true;
            }
        }
    }
}