using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class TotalsDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<TotalsDTO> Items { get; set; }
    }
}
