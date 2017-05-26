using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2._0.Dto
{
    public class TotalsContainerDTO: ContainerDTO
    {
        public List<TotalsDTO> items { get; set; }
    }
}
