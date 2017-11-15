namespace Kadena.Dto.MailTemplate.Responses
{
    public class MailTemplateDto
    {
        public string From { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string BodyPlain { get; set; }
    }
}
