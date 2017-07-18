namespace Kadena.Models.Checkout
{
    public class ShoppingCartTotals
    {
        public double TotalItemsPrice { get; set; }
        public double TotalShipping { get; set; }
        public double TotalTax { get; set; }
        public double TotalPrice
        {
            get
            {
                return Subtotal + TotalTax;
            }
        }

        public double Subtotal
        {
            get
            {
                return TotalItemsPrice + TotalShipping;
            }
        }
    }
}