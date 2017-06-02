namespace Kadena.WebAPI.Models
{
    public class DeliveryService
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

        public void UpdateSummaryText(string price, string cannotBeDelivered)
        {
            if (Disabled)
            {
                PricePrefix = cannotBeDelivered;
                Price = string.Empty;
            }
            else
            {
                PricePrefix = price;
            }
        }
    }
}