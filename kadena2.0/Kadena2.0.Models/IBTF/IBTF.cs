namespace Kadena.Models.IBTF
{
    public class IBTF
    {
        public int ItemID { get; set; }
        public int SKUID { get; set; }
        public int QtyOrdered { get; set; }
        public int DemandGoal { get; set; }
        public int QtyReceived { get; set; }
        public int QtyProduced { get; set; }
        public int Overage { get; set; }
        public string Vendor { get; set; }
        public decimal ActualPrice { get; set; }
    }
}
