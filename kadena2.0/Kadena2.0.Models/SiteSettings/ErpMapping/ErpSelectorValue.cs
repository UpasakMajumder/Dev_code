namespace Kadena.Models.SiteSettings.ErpMapping
{
    public class ErpSelectorValue
    {
        public string MicroserviceId { get; set; }
        public string TargetErpCodename { get; set; }
        public string CustomerErpId { get; set; }
        public bool ToSync { get; set; }
    }
}
