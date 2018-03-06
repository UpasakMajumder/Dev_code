namespace Kadena.Models
{
    public class DeliveryOption
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public string Title { get; set; }
        public bool Checked { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public double PriceAmount { get; set; }
        public string DatePrefix { get; set; }
        public string Date { get; set; }
        public bool Disabled { get; set; }
        public string CarrierCode { get; set; }
        public string Service { get; set; }
        public string SAPName { get; set; }
        public bool IsCustomerPrice
        {
            get
            {
                return Service.Contains("CUSTOMER_PRICE");
            }
        }

        public void UpdateSummaryText(string price, string cannotBeDelivered, string customerPrice)
        {
            if (Disabled)
            {
                PricePrefix = cannotBeDelivered;
                Price = string.Empty;
            }
            else if (IsCustomerPrice)
            {
                PricePrefix = customerPrice;
                Price = string.Empty;
            }
            else
            {
                PricePrefix = price;
            }
        }

        public void Disable()
        {
            this.Disabled = true;
            this.PriceAmount = 0.0d;
            this.Price = null;
        }

        public override string ToString()
        {
            return $"Shipping option {Title} Id {Id}";
        }
    }
}