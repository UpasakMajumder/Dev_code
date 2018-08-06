using System;

namespace Kadena.Models.Program
{
    public class CampaignProgram
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public int CampaignID { get; set; }
        public int BrandID { get; set; }
        public bool GlobalAdminNotified { get; set; }
        public DateTime DeliveryDateToDistributors { get; set; }
    }
}
