namespace Kadena.Dto.TemplatedProduct.MicroserviceRequests
{
    public class NewTemplateRequestDto
    {
        public string User { get; set; }
        public string TemplateId { get; set; }
        public string WorkSpaceId { get; set; }
        public bool Use3d { get; set; }
        public bool UseHtmlEditor { get; set; }
    }
}
