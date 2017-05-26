using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class TotalsContainerDTO: ContainerDTO
    {
        public List<TotalsDTO> items { get; set; }
    }
}
