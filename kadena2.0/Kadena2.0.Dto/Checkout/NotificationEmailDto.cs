namespace Kadena.Dto.Checkout
{
    public class NotificationEmailDto
    {
        public bool Exists { get; set; }
        public int MaxItems { get; set; }
        public NotificationEmailTooltipDto TooltipText { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}