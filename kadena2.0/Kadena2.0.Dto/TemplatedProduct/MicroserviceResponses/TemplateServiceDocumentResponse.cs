using System;
using System.Collections.Generic;

namespace Kadena.Dto.TemplatedProduct.MicroserviceResponses
{
    public class TemplateServiceDocumentResponse
    {
        public Guid TemplateId { get; set; }
        public Guid MasterTemplateId { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public TemplateServiceDocumentMailingList MailingList { get; set; }
        public TemplateMetaData MetaData { get; set; }
        public string[] PreviewUrls { get; set; }
    }
}