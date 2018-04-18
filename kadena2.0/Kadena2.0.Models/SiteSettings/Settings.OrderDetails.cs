using Kadena.Models.SiteSettings.Attributes;

namespace Kadena.Models.SiteSettings
{
    public partial class Settings
    {
        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Order Details")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVyIERldGFpbHMgU2hvdyBQcm9kdWN0IFN0YXR1cyIsIktleU5hbWUiOiJLREFfT3JkZXJEZXRhaWxzU2hvd1Byb2R1Y3RTdGF0dXMiLCJLZXlUeXBlIjoiYm9vbGVhbiIsIktleURlZmF1bHRWYWx1ZSI6IkZhbHNlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik9yZGVyIERldGFpbHMifX0=")]
        public const string KDA_OrderDetailsShowProductStatus = "KDA_OrderDetailsShowProductStatus";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Order Details")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkZhaWxlZCBPcmRlcnMgRW1haWwgVGVtcGxhdGUiLCJLZXlOYW1lIjoiS0RBX0ZhaWxlZE9yZGVyc0VtYWlsVGVtcGxhdGUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IkVtYWlsX3RlbXBsYXRlX3NlbGVjdG9yIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48U2hvd0VkaXRCdXR0b24+VHJ1ZTwvU2hvd0VkaXRCdXR0b24+PFNob3dOZXdCdXR0b24+VHJ1ZTwvU2hvd05ld0J1dHRvbj48U2l0ZU5hbWU+IyNjdXJyZW50c2l0ZSMjPC9TaXRlTmFtZT48VGVtcGxhdGVUeXBlPmVjb21tZXJjZTwvVGVtcGxhdGVUeXBlPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiT3JkZXIgRGV0YWlscyJ9fQ==")]
        public const string KDA_FailedOrdersEmailTemplate = "KDA_FailedOrdersEmailTemplate";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Order Details")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkZhaWxlZCBPcmRlcnMgRW1haWwgVGVtcGxhdGUgR2kiLCJLZXlOYW1lIjoiS0RBX0ZhaWxlZE9yZGVyc0VtYWlsVGVtcGxhdGVHSSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiRW1haWxfdGVtcGxhdGVfc2VsZWN0b3IiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxTaG93RWRpdEJ1dHRvbj5UcnVlPC9TaG93RWRpdEJ1dHRvbj48U2hvd05ld0J1dHRvbj5UcnVlPC9TaG93TmV3QnV0dG9uPjxTaXRlTmFtZT4jI2N1cnJlbnRzaXRlIyM8L1NpdGVOYW1lPjxUZW1wbGF0ZVR5cGU+ZWNvbW1lcmNlPC9UZW1wbGF0ZVR5cGU+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJPcmRlciBEZXRhaWxzIn19")]
        public const string KDA_FailedOrdersEmailTemplateGI = "KDA_FailedOrdersEmailTemplateGI";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Order Details")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IklCVEYgRmluYWxpemUgRW1haWwgVGVtcGxhdGUiLCJLZXlOYW1lIjoiS0RBX0lCVEZGaW5hbGl6ZUVtYWlsVGVtcGxhdGUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IkVtYWlsX3RlbXBsYXRlX3NlbGVjdG9yIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48U2hvd0VkaXRCdXR0b24+VHJ1ZTwvU2hvd0VkaXRCdXR0b24+PFNob3dOZXdCdXR0b24+VHJ1ZTwvU2hvd05ld0J1dHRvbj48U2l0ZU5hbWU+IyNjdXJyZW50c2l0ZSMjPC9TaXRlTmFtZT48VGVtcGxhdGVUeXBlPmVjb21tZXJjZTwvVGVtcGxhdGVUeXBlPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiT3JkZXIgRGV0YWlscyJ9fQ==")]
        public const string KDA_IBTFFinalizeEmailTemplate = "KDA_IBTFFinalizeEmailTemplate";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Order Details")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVyIFJlc2VydmF0aW9uIEVtYWlsIFRlbXBsYXRlIiwiS2V5TmFtZSI6IktEQV9PcmRlclJlc2VydmF0aW9uRW1haWxUZW1wbGF0ZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiRW1haWxfdGVtcGxhdGVfc2VsZWN0b3IiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxTaG93RWRpdEJ1dHRvbj5UcnVlPC9TaG93RWRpdEJ1dHRvbj48U2hvd05ld0J1dHRvbj5UcnVlPC9TaG93TmV3QnV0dG9uPjxTaXRlTmFtZT4jI2N1cnJlbnRzaXRlIyM8L1NpdGVOYW1lPjxUZW1wbGF0ZVR5cGU+ZWNvbW1lcmNlPC9UZW1wbGF0ZVR5cGU+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJPcmRlciBEZXRhaWxzIn19")]
        public const string KDA_OrderReservationEmailTemplate = "KDA_OrderReservationEmailTemplate";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Order Details")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVyIFJlc2VydmF0aW9uIEVtYWlsIFRlbXBsYXRlIEdpIiwiS2V5TmFtZSI6IktEQV9PcmRlclJlc2VydmF0aW9uRW1haWxUZW1wbGF0ZUdJIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJFbWFpbF90ZW1wbGF0ZV9zZWxlY3RvciIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PFNob3dFZGl0QnV0dG9uPlRydWU8L1Nob3dFZGl0QnV0dG9uPjxTaG93TmV3QnV0dG9uPlRydWU8L1Nob3dOZXdCdXR0b24+PFNpdGVOYW1lPiMjY3VycmVudHNpdGUjIzwvU2l0ZU5hbWU+PFRlbXBsYXRlVHlwZT5lY29tbWVyY2U8L1RlbXBsYXRlVHlwZT48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik9yZGVyIERldGFpbHMifX0=")]
        public const string KDA_OrderReservationEmailTemplateGI = "KDA_OrderReservationEmailTemplateGI";

    }
}
