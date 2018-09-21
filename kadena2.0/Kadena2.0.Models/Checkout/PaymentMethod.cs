using Kadena.Models.CreditCard;
using System.Collections.Generic;

namespace Kadena.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Disabled { get; set; }
        public bool Checked { get; set; }
        public bool HasInput { get; set; }
        public string InputPlaceholder { get; set; }
        public string ClassName { get; set; }
        public bool IsUnpayable { get; set; }
        public IList<StoredCard> Items { get; set; } = new List<StoredCard>();
        public string ShortClassName
        {
            get
            {
                return (ClassName.Contains(".") && !ClassName.EndsWith("."))
                    ? ClassName.Substring(ClassName.LastIndexOf(".") + 1)
                    : ClassName;
            }
        }

    }
}