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
        public string Created { get; set; }
        public string Updated { get; set; }
        public TemplateServiceDocumentMailingList MailingList { get; set; }
        public TemplateMetaData MetaData { get; set; }
    }
}