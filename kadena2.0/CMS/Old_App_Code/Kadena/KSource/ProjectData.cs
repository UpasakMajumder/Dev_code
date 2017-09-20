using System;
using System.Runtime.Serialization;

namespace Kadena.Old_App_Code.Kadena.KSource
{
    [DataContract]
    public class ProjectData
    {
        [DataMember(Name = "requestId")]
        public int RequestId { get; set; }

        [DataMember(Name = "projectName")]
        public string Name { get; set; }

        [DataMember(Name = "projectStatus")]
        public string Status { get; set; }

        [DataMember(Name = "lastUpdate")]
        public DateTime? UpdateDate { get; set; }

        [DataMember(Name = "isActive")]
        public bool Active { get; set; }
    }
}