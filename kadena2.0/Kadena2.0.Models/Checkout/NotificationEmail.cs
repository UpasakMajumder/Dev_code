namespace Kadena.Models.Checkout
{
    public class NotificationEmail
    {
        public bool Exists { get; set; }
        public int MaxItems { get; set; }
        public NotificationEmailTooltip TooltipText { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}