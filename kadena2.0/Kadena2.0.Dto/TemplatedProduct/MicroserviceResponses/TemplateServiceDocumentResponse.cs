using System.Runtime.Serialization;

namespace Kadena.Dto.TemplatedProduct.MicroserviceResponses
{
    [DataContract]
    public class TemplateServiceDocumentResponse
    {
        [DataMember(Name = "templateId")]
        public string TemplateId { get; set; }
        [DataMember(Name = "masterTemplateId")]
        public string MasterTemplateId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "user")]
        public string User { get; set; }
        [DataMember(Name = "created")]
        public string Created { get; set; }
        [DataMember(Name = "updated")]
        public string Updated { get; set; }
        [DataMember(Name = "mailingList")]
        public TemplateServiceDocumentMailingList MailingList { get; set; }
    }
}