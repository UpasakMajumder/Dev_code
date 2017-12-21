using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class CampaignDTO
    {
        public int ID { get; set; }
        public int ProgramID { get; set; }
        public int DistributorID { get; set; }
    }
}
