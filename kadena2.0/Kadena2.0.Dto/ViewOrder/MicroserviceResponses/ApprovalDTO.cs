using System;

namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class ApprovalDTO
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int State { get; set; }
    }
}
