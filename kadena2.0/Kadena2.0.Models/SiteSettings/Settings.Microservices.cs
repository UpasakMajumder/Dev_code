using Kadena.Models.SiteSettings.Attributes;

namespace Kadena.Models.SiteSettings
{
    public partial class Settings
    {

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9UYXhFc3RpbWF0aW9uU2VydmljZVZlcnNpb24iLCJLZXlOYW1lIjoiS0RBX1RheEVzdGltYXRpb25TZXJ2aWNlVmVyc2lvbiIsIktleVR5cGUiOiJpbnQiLCJLZXlEZWZhdWx0VmFsdWUiOiIxIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1pY3Jvc2VydmljZXMgc2V0dGluZ3MifX0=")]
        public const string KDA_TaxEstimationServiceVersion = "KDA_TaxEstimationServiceVersion";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9TaGlwcGluZ0Nvc3RTZXJ2aWNlVmVyc2lvbiIsIktleU5hbWUiOiJLREFfU2hpcHBpbmdDb3N0U2VydmljZVZlcnNpb24iLCJLZXlUeXBlIjoiaW50IiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWljcm9zZXJ2aWNlcyBzZXR0aW5ncyJ9fQ==")]
        public const string KDA_ShippingCostServiceVersion = "KDA_ShippingCostServiceVersion";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9PcmRlclZpZXdTZXJ2aWNlVmVyc2lvbiIsIktleU5hbWUiOiJLREFfT3JkZXJWaWV3U2VydmljZVZlcnNpb24iLCJLZXlUeXBlIjoiaW50IiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWljcm9zZXJ2aWNlcyBzZXR0aW5ncyJ9fQ==")]
        public const string KDA_OrderViewServiceVersion = "KDA_OrderViewServiceVersion";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9PcmRlclNlcnZpY2VWZXJzaW9uIiwiS2V5TmFtZSI6IktEQV9PcmRlclNlcnZpY2VWZXJzaW9uIiwiS2V5VHlwZSI6ImludCIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1pY3Jvc2VydmljZXMgc2V0dGluZ3MifX0=")]
        public const string KDA_OrderServiceVersion = "KDA_OrderServiceVersion";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9PcmRlclJlc3VibWl0U2VydmljZVZlcnNpb24iLCJLZXlOYW1lIjoiS0RBX09yZGVyUmVzdWJtaXRTZXJ2aWNlVmVyc2lvbiIsIktleVR5cGUiOiJpbnQiLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNaWNyb3NlcnZpY2VzIHNldHRpbmdzIn19")]
        public const string KDA_OrderResubmitServiceVersion = "KDA_OrderResubmitServiceVersion";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9Ob3RpZmljYXRpb25TZXJ2aWNlVmVyc2lvbiIsIktleU5hbWUiOiJLREFfTm90aWZpY2F0aW9uU2VydmljZVZlcnNpb24iLCJLZXlUeXBlIjoiaW50IiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWljcm9zZXJ2aWNlcyBzZXR0aW5ncyJ9fQ==")]
        public const string KDA_NotificationServiceVersion = "KDA_NotificationServiceVersion";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlN0YXRpc3RpY3Mgc2VydmljZSB2ZXJzaW9uIiwiS2V5TmFtZSI6IktEQV9TdGF0aXN0aWNzU2VydmljZVZlcnNpb24iLCJLZXlUeXBlIjoiaW50IiwiS2V5RGVmYXVsdFZhbHVlIjoiMSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6Ik1pY3Jvc2VydmljZXMiLCJOYW1lIjoiTWljcm9zZXJ2aWNlcyIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkthZGVuYSJ9LCJEaXNwbGF5TmFtZSI6IlZlcnNpb25zIn19")]
        public const string KDA_StatisticsServiceVersion = "KDA_StatisticsServiceVersion";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9BcHByb3ZhbFNlcnZpY2VWZXJzaW9uIiwiS2V5TmFtZSI6IktEQV9BcHByb3ZhbFNlcnZpY2VWZXJzaW9uIiwiS2V5VHlwZSI6ImludCIsIktleURlZmF1bHRWYWx1ZSI6IjEiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWljcm9zZXJ2aWNlcyBzZXR0aW5ncyJ9fQ==")]
        public const string KDA_ApprovalServiceVersion = "KDA_ApprovalServiceVersion";
    }
}
