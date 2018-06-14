namespace Kadena.Models.Site
{
    public class KenticoSite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ErpCustomerId { get; set; }
        public string OrderManagerEmail { get; set; }
        public string SiteDomain { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}