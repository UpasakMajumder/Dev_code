using System.Runtime.Serialization;

namespace Kadena.Old_App_Code.Kadena.Chili
{
    [DataContract]
    public class TemplateServiceDocumentResponse
    {
        [DataMember(Name = "templateId")]
        public string templateId { get; set; }
        [DataMember(Name = "masterTemplateId")]
        public string masterTemplateId { get; set; }
        [DataMember(Name = "user")]
        public string user { get; set; }
        [DataMember(Name = "created")]
        public string created { get; set; }
        [DataMember(Name = "updated")]
        public string updated { get; set; }
        [DataMember(Name = "mailingList")]
        public TemplateServiceDocumentMailingList MailingList { get; set; }
    }

    [DataContract]
    public class TemplateServiceDocumentMailingList
    {
        [DataMember(Name = "containerId")]
        public string ContainerID { get; set; }
        [DataMember(Name = "rowCount")]
        public int RowCount { get; set; }
    }
}