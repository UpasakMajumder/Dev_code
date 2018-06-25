namespace Kadena.Models
{
    public class DeliveryAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public State State { get; set; } = new State();
        public Country Country { get; set; } = new Country();
        public string Zip { get; set; }
        public int Id { get; set; }
        public bool Checked { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressPersonalName { get; set; }
        public string CompanyName { get; set; }
        public string AddressName { get; set; }
        public int CustomerId { get; set; }
    }
}