using System;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class ChiliProcessDto
    {
        public Guid TemplateId { get; set; }

        public Guid PdfSettings { get; set; }
    }
}