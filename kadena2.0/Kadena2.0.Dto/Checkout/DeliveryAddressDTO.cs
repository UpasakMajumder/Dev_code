using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Checkout
{
    public class DeliveryAddressDTO
    {
        [MaxLength(35)]
        public string Address1 { get; set; }

        [MaxLength(35)]
        public string Address2 { get; set; }

        [MaxLength(40)]
        public string City { get; set; }

        [MaxLength(3)]
        public string State { get; set; }

        [MaxLength(10)]
        public string Zip { get; set; }

        public int Id { get; set; }

        public bool Checked { get; set; }

        [MaxLength(35)]
        public string CustomerName { get; set; }

        public string Country { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(77)]
        public string Email { get; set; }
    }
}
