namespace Kadena.Dto.CustomerData
{
    public class CustomerDataDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerAddressDTO Address { get; set; }
    }
}
