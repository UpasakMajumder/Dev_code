using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2._0.Dto
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Disabled { get; set; }
        public bool Checked { get; set; }
    }
}
