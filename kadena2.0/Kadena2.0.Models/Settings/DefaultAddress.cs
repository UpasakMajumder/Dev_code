namespace Kadena.Models.Settings
{
    public class DefaultAddress
    {
        public int Id { get; set; }
        public string LabelDefault { get; set; }
        public string LabelNonDefault { get; set; }
        public string Tooltip { get; set; }
        public string SetUrl { get; set; }
        public string UnsetUrl { get; set; }
    }
}
