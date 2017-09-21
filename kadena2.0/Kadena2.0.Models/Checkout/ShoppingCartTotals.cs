namespace Kadena.Models.Checkout
{
    public class ShoppingCartTotals
    {
        public decimal TotalItemsPrice { get; set; }
        public decimal TotalShipping { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return Subtotal + TotalTax;
            }
        }

        public decimal Subtotal
        {
            get
            {
                return TotalItemsPrice + TotalShipping;
            }
        }
    }
}