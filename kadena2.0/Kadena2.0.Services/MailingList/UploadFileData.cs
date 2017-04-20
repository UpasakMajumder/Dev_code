using System.Runtime.Serialization;

namespace Kadena.Services.MailingList
{
    [DataContract]
    public class UploadFileData
    {
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        [DataMember(Name = "mailType")]
        public string MailType { get; set; }

        [DataMember(Name = "product")]
        public string Product { get; set; }

        [DataMember(Name = "validity")]
        public string Validity { get; set; }

        [DataMember(Name = "fileUrl")]
        public string FileUrl { get; set; }
    }
}
