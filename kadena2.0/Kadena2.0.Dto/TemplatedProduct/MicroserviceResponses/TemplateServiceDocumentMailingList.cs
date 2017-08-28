using System.Runtime.Serialization;

namespace Kadena.Dto.TemplatedProduct.MicroserviceResponses
{
    [DataContract]
    public class TemplateServiceDocumentMailingList
    {
        [DataMember(Name = "containerId")]
        public string ContainerId { get; set; }
        [DataMember(Name = "rowCount")]
        public int RowCount { get; set; }
    }
}
