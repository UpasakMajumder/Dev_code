using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Checkout
{
    public class DeliveryAddressDTO
    {
        public List<string> Street { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(2)]
        public string State { get; set; }

        [MaxLength(50)]
        public string Zip { get; set; }

        public int Id { get; set; }

        public bool Checked { get; set; }

        [MaxLength(50)]
        public string CustomerName { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }
    }
}
