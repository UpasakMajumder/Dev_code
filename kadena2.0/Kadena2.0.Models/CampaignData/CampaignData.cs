using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Models.CampaignData
{
    public class CampaignData
    {
        public int CampaignID { get; set; }
        public string Name { get; set; }
        public bool OpenCampaign { get; set; }
        public bool CloseCampaign { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IBTFFinalized { get; set; }
        public bool Status { get; set; }
        public string FiscalYear { get; set; }
    }
}
