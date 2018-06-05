namespace Kadena.Dto.Notification.MicroserviceRequests
{
    public class SendCustomNotificationRequestDto
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
    }
}
