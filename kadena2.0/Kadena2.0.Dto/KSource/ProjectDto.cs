using System;
using System.Runtime.Serialization;

namespace Kadena.Dto.KSource
{
    [DataContract]
    public class ProjectDto
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
