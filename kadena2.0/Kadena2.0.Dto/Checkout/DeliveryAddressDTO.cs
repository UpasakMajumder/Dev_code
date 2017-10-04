using Kadena.Dto.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Checkout
{
    public class DeliveryAddressDTO
    {
        [MinLength(1)]
        [MaxLength(35)]
        public string Street1 { get; set; }

        [MinLength(1)]
        [MaxLength(35)]
        public string Street2 { get; set; }

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

        [MaxLength(3)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(77)]
        public string Email { get; set; }
    }
}
