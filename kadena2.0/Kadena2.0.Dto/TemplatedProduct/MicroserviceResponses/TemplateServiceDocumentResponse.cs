namespace Kadena.Dto.TemplatedProduct.MicroserviceResponses
{
    public class TemplateServiceDocumentResponse
    {
        public string TemplateId { get; set; }
        public string MasterTemplateId { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public TemplateServiceDocumentMailingList MailingList { get; set; }
    }
}