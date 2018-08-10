using Kadena.Models.SiteSettings.Attributes;

namespace Kadena.Models.SiteSettings
{
    public partial class Settings
    {
        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Address Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFkZHJlc3NlcyBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfQWRkcmVzc2VzTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXHJcbmRpc2FibGVkO0Rpc2FibGVkXHJcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQWRkcmVzcyBNYW5hZ2VtZW50In19")]
        public const string KDA_AddressesModuleEnabled = "KDA_AddressesModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Admin Management")]
        [DefaultValueAttribute(@"hidden")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFkbWluIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9BZG1pbk1vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaGlkZGVuIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSYWRpb0J1dHRvbnNDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48T3B0aW9ucz5lbmFibGVkO0VuYWJsZWRcclxuZGlzYWJsZWQ7RGlzYWJsZWRcclxuaGlkZGVuO0hpZGRlbjwvT3B0aW9ucz48UmVwZWF0RGlyZWN0aW9uPmhvcml6b250YWw8L1JlcGVhdERpcmVjdGlvbj48UmVwZWF0TGF5b3V0PkZsb3c8L1JlcGVhdExheW91dD48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJBZG1pbiBNYW5hZ2VtZW50In19")]
        public const string KDA_AdminModuleEnabled = "KDA_AdminModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(@"AAAA_CustomRole")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFkbWluIFJvbGUiLCJLZXlOYW1lIjoiS0RBX0FkbWluUm9sZU5hbWUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiQUFBQV9DdXN0b21Sb2xlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJVbmlfc2VsZWN0b3IiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBZGRHbG9iYWxPYmplY3ROYW1lUHJlZml4PkZhbHNlPC9BZGRHbG9iYWxPYmplY3ROYW1lUHJlZml4PjxBZGRHbG9iYWxPYmplY3RTdWZmaXg+RmFsc2U8L0FkZEdsb2JhbE9iamVjdFN1ZmZpeD48QWxsb3dBbGw+RmFsc2U8L0FsbG93QWxsPjxBbGxvd0RlZmF1bHQ+RmFsc2U8L0FsbG93RGVmYXVsdD48QWxsb3dFZGl0VGV4dEJveD5GYWxzZTwvQWxsb3dFZGl0VGV4dEJveD48QWxsb3dFbXB0eT5GYWxzZTwvQWxsb3dFbXB0eT48RGlhbG9nV2luZG93TmFtZT5TZWxlY3Rpb25EaWFsb2c8L0RpYWxvZ1dpbmRvd05hbWU+PEVkaXREaWFsb2dXaW5kb3dIZWlnaHQ+NzAwPC9FZGl0RGlhbG9nV2luZG93SGVpZ2h0PjxFZGl0RGlhbG9nV2luZG93V2lkdGg+MTAwMDwvRWRpdERpYWxvZ1dpbmRvd1dpZHRoPjxFZGl0V2luZG93TmFtZT5FZGl0V2luZG93PC9FZGl0V2luZG93TmFtZT48RW5jb2RlT3V0cHV0PlRydWU8L0VuY29kZU91dHB1dD48R2xvYmFsT2JqZWN0U3VmZml4IGlzbWFjcm89XCJ0cnVlXCI+eyRnZW5lcmFsLmdsb2JhbCR9PC9HbG9iYWxPYmplY3RTdWZmaXg+PEl0ZW1zUGVyUGFnZT4yNTwvSXRlbXNQZXJQYWdlPjxMb2NhbGl6ZUl0ZW1zPlRydWU8L0xvY2FsaXplSXRlbXM+PE1heERpc3BsYXllZEl0ZW1zPjI1PC9NYXhEaXNwbGF5ZWRJdGVtcz48TWF4RGlzcGxheWVkVG90YWxJdGVtcz41MDwvTWF4RGlzcGxheWVkVG90YWxJdGVtcz48T2JqZWN0VHlwZT5DTVMuUm9sZTwvT2JqZWN0VHlwZT48UmVtb3ZlTXVsdGlwbGVDb21tYXM+RmFsc2U8L1JlbW92ZU11bHRpcGxlQ29tbWFzPjxSZXR1cm5Db2x1bW5OYW1lPlJvbGVOYW1lPC9SZXR1cm5Db2x1bW5OYW1lPjxSZXR1cm5Db2x1bW5UeXBlPmlkPC9SZXR1cm5Db2x1bW5UeXBlPjxTZWxlY3Rpb25Nb2RlPjE8L1NlbGVjdGlvbk1vZGU+PFZhbHVlc1NlcGFyYXRvcj47PC9WYWx1ZXNTZXBhcmF0b3I+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYW1wYWlnbiBNYW5hZ2VtZW50In19")]
        public const string KDA_AdminRoleName = "KDA_AdminRoleName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Admin Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFkbWluIFJvbGVzIiwiS2V5TmFtZSI6IktEQV9BZG1pblJvbGVzIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSb2xlQ2hlY2tib3hTZWxlY3RvciIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkFkbWluIE1hbmFnZW1lbnQifX0=")]
        public const string KDA_AdminRoles = "KDA_AdminRoles";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFsbG93IGN1c3RvbSBzaGlwcGluZyBhZGRyZXNzIiwiS2V5TmFtZSI6IktEQV9BbGxvd0N1c3RvbVNoaXBwaW5nQWRkcmVzcyIsIktleVR5cGUiOiJib29sZWFuIiwiS2V5RGVmYXVsdFZhbHVlIjoiRmFsc2UiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTW9kdWxlIEFjY2VzcyJ9fQ==")]
        public const string KDA_AllowCustomShippingAddress = "KDA_AllowCustomShippingAddress";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Logs")]
        [DefaultValueAttribute(@"kenticoLogsAug")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkxvZyBHcm91cCIsIktleU5hbWUiOiJLREFfQVdTX0xvZ0dyb3VwIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6ImtlbnRpY29Mb2dzQXVnIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkxvZ3MifX0=")]
        public const string KDA_AWS_LogGroup = "KDA_AWS_LogGroup";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Logs")]
        [DefaultValueAttribute(@"USEast1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlJlZ2lvbiBFbmRwb2ludCIsIktleU5hbWUiOiJLREFfQVdTX1JlZ2lvbkVuZHBvaW50IiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IlVTRWFzdDEiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTG9ncyJ9fQ==")]
        public const string KDA_AWS_RegionEndpoint = "KDA_AWS_RegionEndpoint";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"hemant.badaya@cenveo.com")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkJpZCByZWNpcGllbnQgZW1haWwgYWRkcmVzcyIsIktleU5hbWUiOiJLREFfQmlkUmVjaXBpZW50RW1haWxBZGRyZXNzIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6ImhlbWFudC5iYWRheWFAY2VudmVvLmNvbSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJLLVNvdXJjZSJ9fQ==")]
        public const string KDA_BidRecipientEmailAddress = "KDA_BidRecipientEmailAddress";

        /// <summary>
        /// Base address of microservices. Expected format is https://services.kadena???.com
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Microservices settings")]
        [DefaultValueAttribute(@"https://services.kadenastage.com")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik1pY3Jvc2VydmljZXMgYmFzZSBhZGRyZXNzIiwiS2V5TmFtZSI6IktEQV9NaWNyb3NlcnZpY2VzQmFzZUFkZHJlc3MiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaHR0cHM6Ly9zZXJ2aWNlcy5rYWRlbmFzdGFnZS5jb20iLCJLZXlEZXNjcmlwdGlvbiI6IkJhc2UgYWRkcmVzcyBvZiBtaWNyb3NlcnZpY2VzLiBFeHBlY3RlZCBmb3JtYXQgaXMgaHR0cHM6Ly9zZXJ2aWNlcy5rYWRlbmE/Pz8uY29tIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNaWNyb3NlcnZpY2VzIHNldHRpbmdzIn19")]
        public const string KDA_MicroservicesBaseAddress = "KDA_MicroservicesBaseAddress";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Brands Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkJyYW5kIExpc3RpbmcgVVJMIiwiS2V5TmFtZSI6IktEQV9CcmFuZExpc3RVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJCcmFuZHMgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_BrandListURL = "KDA_BrandListURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Brands Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkJyYW5kcyBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfQnJhbmRzTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXHJcbmRpc2FibGVkO0Rpc2FibGVkXHJcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQnJhbmRzIE1hbmFnZW1lbnQifX0=")]
        public const string KDA_BrandsModuleEnabled = "KDA_BrandsModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FY Setup")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkJ1c2luZXNzIFVuaXRzIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9CdXNpbmVzc1VuaXRzTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXHJcbmRpc2FibGVkO0Rpc2FibGVkXHJcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiRlkgU2V0dXAifX0=")]
        public const string KDA_BusinessUnitsModuleEnabled = "KDA_BusinessUnitsModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWF0ZSBDYW1wYWlnbiBQYWdlIFVSTCIsIktleU5hbWUiOiJLREFfQ2FtcGFpZ25DcmVhdGVQYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3Rkb2N1bWVudCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNhbXBhaWduIE1hbmFnZW1lbnQifX0=")]
        public const string KDA_CampaignCreatePageUrl = "KDA_CampaignCreatePageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBDYW1wYWlnbnMgUGF0aCIsIktleU5hbWUiOiJLREFfQ2FtcGFpZ25Gb2xkZXJQYXRoIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FtcGFpZ24gTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_CampaignFolderPath = "KDA_CampaignFolderPath";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNhbXBhaWduIFByb2R1Y3RzIFRlbXBsYXRlIE5hbWUiLCJLZXlOYW1lIjoiS0RBX0NhbXBhaWduUHJvZHVjdHNUZW1wbGF0ZU5hbWUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FtcGFpZ24gTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_CampaignProductsTemplateName = "KDA_CampaignProductsTemplateName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNhbXBhaWducyBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfQ2FtcGFpZ25zTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXHJcbmRpc2FibGVkO0Rpc2FibGVkXHJcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FtcGFpZ24gTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_CampaignsModuleEnabled = "KDA_CampaignsModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Cart Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNhcnQgUERGIEZpbGUgTmFtZSIsIktleU5hbWUiOiJLREFfQ2FydFBERkZpbGVOYW1lIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNhcnQgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_CartPDFFileName = "KDA_CartPDFFileName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Cart Management")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlJlcXVlc3QgZGF0ZSBlbmFibGVkIiwiS2V5TmFtZSI6IktEQV9DYXJ0UmVxdWVzdERhdGVFbmFibGVkIiwiS2V5VHlwZSI6ImJvb2xlYW4iLCJLZXlEZWZhdWx0VmFsdWUiOiJGYWxzZSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYXJ0IE1hbmFnZW1lbnQifX0=")]
        public const string KDA_CartRequestDateEnabled = "KDA_CartRequestDateEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Category Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNhdGVnb3JpZXMgTW9kdWxlIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0NhdGVnb3JpZXNNb2R1bGVFbmFibGVkIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSYWRpb0J1dHRvbnNDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48T3B0aW9ucz5lbmFibGVkO0VuYWJsZWRcclxuZGlzYWJsZWQ7RGlzYWJsZWRcclxuaGlkZGVuO0hpZGRlbjwvT3B0aW9ucz48UmVwZWF0RGlyZWN0aW9uPmhvcml6b250YWw8L1JlcGVhdERpcmVjdGlvbj48UmVwZWF0TGF5b3V0PkZsb3c8L1JlcGVhdExheW91dD48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYXRlZ29yeSBNYW5hZ2VtZW50In19")]
        public const string KDA_CategoriesModuleEnabled = "KDA_CategoriesModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Category Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWF0ZSBDYXRlZ29yeSBQYWdlIFVSTCIsIktleU5hbWUiOiJLREFfQ2F0ZWdvcnlDcmVhdGVQYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2F0ZWdvcnkgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_CategoryCreatePageUrl = "KDA_CategoryCreatePageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Category Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBDYXRlZ29yaWVzIFBhdGgiLCJLZXlOYW1lIjoiS0RBX0NhdGVnb3J5Rm9sZGVyUGF0aCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNhdGVnb3J5IE1hbmFnZW1lbnQifX0=")]
        public const string KDA_CategoryFolderPath = "KDA_CategoryFolderPath";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"https://hy709luk3m.execute-api.us-east-1.amazonaws.com/Qa/")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNsb3VkIEV2ZW50IENvbmZpZ3VyYXRvciBVcmwiLCJLZXlOYW1lIjoiS0RBX0Nsb3VkRXZlbnRDb25maWd1cmF0b3JVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaHR0cHM6Ly9oeTcwOWx1azNtLmV4ZWN1dGUtYXBpLnVzLWVhc3QtMS5hbWF6b25hd3MuY29tL1FhLyIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJLLVNvdXJjZSJ9fQ==")]
        public const string KDA_CloudEventConfiguratorUrl = "KDA_CloudEventConfiguratorUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Forms")]
        [DefaultValueAttribute(@"andrey.konovka@actum.cz")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNvbnRhY3QgVXMgRm9ybSBSZWNpcGllbnQgRW1haWwgQWRkcmVzcyIsIktleU5hbWUiOiJLREFfQ29udGFjdFVzRm9ybVJlY2lwaWVudEVtYWlsQWRkcmVzcyIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJhbmRyZXkua29ub3ZrYUBhY3R1bS5jeiIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJGb3JtcyJ9fQ==")]
        public const string KDA_ContactUsFormRecipientEmailAddress = "KDA_ContactUsFormRecipientEmailAddress";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Address Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWF0ZSBBZGRyZXNzIFBhdGgiLCJLZXlOYW1lIjoiS0RBX0NyZWF0ZUFkZHJlc3NVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJBZGRyZXNzIE1hbmFnZW1lbnQifX0=")]
        public const string KDA_CreateAddressURL = "KDA_CreateAddressURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Brands Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWF0ZSBCcmFuZCBQYWdlIFVSTCIsIktleU5hbWUiOiJLREFfQ3JlYXRlQnJhbmRVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJCcmFuZHMgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_CreateBrandURL = "KDA_CreateBrandURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FY Setup")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBCdXNpbmVzcyBVbml0IFBhZ2UgVVJMIiwiS2V5TmFtZSI6IktEQV9DcmVhdGVCdXNpbmVzc1VuaXRVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJGWSBTZXR1cCJ9fQ==")]
        public const string KDA_CreateBusinessUnitURL = "KDA_CreateBusinessUnitURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Program Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWF0ZSBQcm9ncmFtIFBhZ2UgVVJMIiwiS2V5TmFtZSI6IktEQV9DcmVhdGVQcm9ncmFtVVJMIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiUHJvZ3JhbSBNYW5hZ2VtZW50In19")]
        public const string KDA_CreateProgramURL = "KDA_CreateProgramURL";
       
        /// <summary>
        /// KDA Credit Card InsertCardDetailsURL
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(@"/Recent-orders/Insert-card-details")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBDcmVkaXQgQ2FyZCBJbnNlcnRDYXJkRGV0YWlsc1VSTCIsIktleU5hbWUiOiJLREFfQ3JlZGl0Q2FyZF9JbnNlcnRDYXJkRGV0YWlsc1VSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvUmVjZW50LW9yZGVycy9JbnNlcnQtY2FyZC1kZXRhaWxzIiwiS2V5RGVzY3JpcHRpb24iOiJLREEgQ3JlZGl0IENhcmQgSW5zZXJ0Q2FyZERldGFpbHNVUkwiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNyZWRpdCBDYXJkIFBheW1lbnQifX0=")]
        public const string KDA_CreditCard_InsertCardDetailsURL = "KDA_CreditCard_InsertCardDetailsURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(@"https://5sfp17q58d.execute-api.us-east-1.amazonaws.com/Qa")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWRpdCBDYXJkIE1hbmFnZXIgRW5kcG9pbnQiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRNYW5hZ2VyRW5kcG9pbnQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaHR0cHM6Ly81c2ZwMTdxNThkLmV4ZWN1dGUtYXBpLnVzLWVhc3QtMS5hbWF6b25hd3MuY29tL1FhIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNyZWRpdCBDYXJkIFBheW1lbnQifX0=")]
        public const string KDA_CreditCardManagerEndpoint = "KDA_CreditCardManagerEndpoint";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(@"hidden")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkN1c3RvbSBDYXRhbG9nIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9DdXN0b21DYXRhbG9nTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJoaWRkZW4iLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IlJhZGlvQnV0dG9uc0NvbnRyb2wiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxPcHRpb25zPmVuYWJsZWQ7RW5hYmxlZFxyXG5kaXNhYmxlZDtEaXNhYmxlZFxyXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkN1c3RvbSBDYXRhbG9nIFBERiBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_CustomCatalogModuleEnabled = "KDA_CustomCatalogModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRhc2hib2FyZCBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfRGFzaGJvYXJkTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJlbmFibGVkIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSYWRpb0J1dHRvbnNDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48T3B0aW9ucz5lbmFibGVkO0VuYWJsZWRcbmRpc2FibGVkO0Rpc2FibGVkXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1vZHVsZSBBY2Nlc3MifX0=")]
        public const string KDA_DashboardModuleEnabled = "KDA_DashboardModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"5")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVycyBwZXIgcGFnZSBmb3IgSy1DZW50ZXIiLCJLZXlOYW1lIjoiS0RBX0Rhc2hib2FyZE9yZGVyc1BhZ2VDYXBhY2l0eSIsIktleVR5cGUiOiJpbnQiLCJLZXlEZWZhdWx0VmFsdWUiOiI1IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik9yZGVycyJ9fQ==")]
        public const string KDA_DashboardOrdersPageCapacity = "KDA_DashboardOrdersPageCapacity";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"http://default.kadenatest.com/special-pages/module-is-disabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRpc2FibGVkIE1vZHVsZSBVcmwiLCJLZXlOYW1lIjoiS0RBX0Rpc2FibGVkTW9kdWxlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Imh0dHA6Ly9kZWZhdWx0LmthZGVuYXRlc3QuY29tL3NwZWNpYWwtcGFnZXMvbW9kdWxlLWlzLWRpc2FibGVkIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1vZHVsZSBBY2Nlc3MifX0=")]
        public const string KDA_DisabledModuleUrl = "KDA_DisabledModuleUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Cart Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRpc3RyaWJ1dG9yIENhcnQgUERGIEhUTUwiLCJLZXlOYW1lIjoiS0RBX0Rpc3RyaWJ1dG9yQ2FydFBERkhUTUwiLCJLZXlUeXBlIjoibG9uZ3RleHQiLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYXJ0IE1hbmFnZW1lbnQifX0=")]
        public const string KDA_DistributorCartPDFHTML = "KDA_DistributorCartPDFHTML";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Cart Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRpc3RyaWJ1dG9yIENhcnQgUERGIEhUTUwgQm9keSIsIktleU5hbWUiOiJLREFfRGlzdHJpYnV0b3JDYXJ0UERGSFRNTEJvZHkiLCJLZXlUeXBlIjoibG9uZ3RleHQiLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYXJ0IE1hbmFnZW1lbnQifX0=")]
        public const string KDA_DistributorCartPDFHTMLBody = "KDA_DistributorCartPDFHTMLBody";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Cart Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRpc3RyaWJ1dG9yIENhcnQgUERGIE91dGVyIEJvZHkgSFRNTCIsIktleU5hbWUiOiJLREFfRGlzdHJpYnV0b3JDYXJ0UERGT3V0ZXJCb2R5SFRNTCIsIktleVR5cGUiOiJsb25ndGV4dCIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNhcnQgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_DistributorCartPDFOuterBodyHTML = "KDA_DistributorCartPDFOuterBodyHTML";

        /// <summary>
        /// Regular Expression for Email Validation in Sign-up
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("User Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkVtYWlsIFZhbGlkYXRpb24gUmVnRXgiLCJLZXlOYW1lIjoiS0RBX0VtYWlsX1ZhbGlkYXRpb24iLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IlJlZ3VsYXIgRXhwcmVzc2lvbiBmb3IgRW1haWwgVmFsaWRhdGlvbiBpbiBTaWduLXVwIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJVc2VyIE1hbmFnZW1lbnQifX0=")]
        public const string KDA_Email_Validation = "KDA_Email_Validation";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Smarty Steets")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkVuYWJsZSBTbWFydHkgU3RyZWV0cyIsIktleU5hbWUiOiJLREFfRW5hYmxlU21hcnR5U3RyZWV0IiwiS2V5VHlwZSI6ImJvb2xlYW4iLCJLZXlEZWZhdWx0VmFsdWUiOiJGYWxzZSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJTbWFydHkgU3RlZXRzIn19")]
        public const string KDA_EnableSmartyStreet = "KDA_EnableSmartyStreet";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"Test Billing address")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlbmRlciBBZGRyZXNzIGxpbmUgMSIsIktleU5hbWUiOiJLREFfRXN0aW1hdGVEZWxpdmVyeVByaWNlX1NlbmRlckFkZHJlc3NMaW5lMSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJUZXN0IEJpbGxpbmcgYWRkcmVzcyIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYXJyaWVyIHByb3ZpZGVycyJ9fQ==")]
        public const string KDA_EstimateDeliveryPrice_SenderAddressLine1 = "KDA_EstimateDeliveryPrice_SenderAddressLine1";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"938")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlbmRlciBBZGRyZXNzIGxpbmUgMiIsIktleU5hbWUiOiJLREFfRXN0aW1hdGVEZWxpdmVyeVByaWNlX1NlbmRlckFkZHJlc3NMaW5lMiIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiI5MzgiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FycmllciBwcm92aWRlcnMifX0=")]
        public const string KDA_EstimateDeliveryPrice_SenderAddressLine2 = "KDA_EstimateDeliveryPrice_SenderAddressLine2";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"COLLIERVILLE")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlbmRlciBDaXR5IiwiS2V5TmFtZSI6IktEQV9Fc3RpbWF0ZURlbGl2ZXJ5UHJpY2VfU2VuZGVyQ2l0eSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJDT0xMSUVSVklMTEUiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FycmllciBwcm92aWRlcnMifX0=")]
        public const string KDA_EstimateDeliveryPrice_SenderCity = "KDA_EstimateDeliveryPrice_SenderCity";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"US")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlbmRlciBDb3VudHJ5IiwiS2V5TmFtZSI6IktEQV9Fc3RpbWF0ZURlbGl2ZXJ5UHJpY2VfU2VuZGVyQ291bnRyeSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJVUyIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYXJyaWVyIHByb3ZpZGVycyJ9fQ==")]
        public const string KDA_EstimateDeliveryPrice_SenderCountry = "KDA_EstimateDeliveryPrice_SenderCountry";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"Kadena sender name")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlbmRlciBOYW1lIiwiS2V5TmFtZSI6IktEQV9Fc3RpbWF0ZURlbGl2ZXJ5UHJpY2VfU2VuZGVyTmFtZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJLYWRlbmEgc2VuZGVyIG5hbWUiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FycmllciBwcm92aWRlcnMifX0=")]
        public const string KDA_EstimateDeliveryPrice_SenderName = "KDA_EstimateDeliveryPrice_SenderName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"38017")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlbmRlciBQb3N0YWwiLCJLZXlOYW1lIjoiS0RBX0VzdGltYXRlRGVsaXZlcnlQcmljZV9TZW5kZXJQb3N0YWwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiMzgwMTciLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FycmllciBwcm92aWRlcnMifX0=")]
        public const string KDA_EstimateDeliveryPrice_SenderPostal = "KDA_EstimateDeliveryPrice_SenderPostal";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"TN")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlbmRlciBTdGF0ZSIsIktleU5hbWUiOiJLREFfRXN0aW1hdGVEZWxpdmVyeVByaWNlX1NlbmRlclN0YXRlIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IlROIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNhcnJpZXIgcHJvdmlkZXJzIn19")]
        public const string KDA_EstimateDeliveryPrice_SenderState = "KDA_EstimateDeliveryPrice_SenderState";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FAQ Management")]
        [DefaultValueAttribute(@"hidden")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkZBUSBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfRkFRTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJoaWRkZW4iLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IlJhZGlvQnV0dG9uc0NvbnRyb2wiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxPcHRpb25zPmVuYWJsZWQ7RW5hYmxlZFxyXG5kaXNhYmxlZDtEaXNhYmxlZFxyXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkZBUSBNYW5hZ2VtZW50In19")]
        public const string KDA_FAQModuleEnabled = "KDA_FAQModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FTP Artwork")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0ZUUEFXX0VuYWJsZWQiLCJLZXlUeXBlIjoiYm9vbGVhbiIsIktleURlZmF1bHRWYWx1ZSI6IkZhbHNlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkZUUCBBcnR3b3JrIn19")]
        public const string KDA_FTPAW_Enabled = "KDA_FTPAW_Enabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FTP Artwork")]
        [DefaultValueAttribute(@"QfyiOm75")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlBhc3N3b3JkIiwiS2V5TmFtZSI6IktEQV9GVFBBV19QYXNzd29yZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJRZnlpT203NSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJGVFAgQXJ0d29yayJ9fQ==")]
        public const string KDA_FTPAW_Password = "KDA_FTPAW_Password";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FTP Artwork")]
        [DefaultValueAttribute(@"Please include protocol in the url, i.e. ftp:// or ftps://")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkZUUCBVUkwiLCJLZXlOYW1lIjoiS0RBX0ZUUEFXX1VybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJQbGVhc2UgaW5jbHVkZSBwcm90b2NvbCBpbiB0aGUgdXJsLCBpLmUuIGZ0cDovLyBvciBmdHBzOi8vIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkZUUCBBcnR3b3JrIn19")]
        public const string KDA_FTPAW_Url = "KDA_FTPAW_Url";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FTP Artwork")]
        [DefaultValueAttribute(@"ecommerceqauser")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlVzZXJuYW1lIiwiS2V5TmFtZSI6IktEQV9GVFBBV19Vc2VybmFtZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJlY29tbWVyY2VxYXVzZXIiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiRlRQIEFydHdvcmsifX0=")]
        public const string KDA_FTPAW_Username = "KDA_FTPAW_Username";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Admin Management")]
        [DefaultValueAttribute(@"hidden")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkdlbmVyaWMgTW9kdWxlIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0dlbmVyaWNNb2R1bGVFbmFibGVkIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6ImhpZGRlbiIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXHJcbmRpc2FibGVkO0Rpc2FibGVkXHJcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQWRtaW4gTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_GenericModuleEnabled = "KDA_GenericModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(@"AAAA_CustomRole")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ikdsb2JhbCBBZG1pbiBSb2xlIiwiS2V5TmFtZSI6IktEQV9HbG9iYWxBbWluUm9sZU5hbWUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiQUFBQV9DdXN0b21Sb2xlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJVbmlfc2VsZWN0b3IiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBZGRHbG9iYWxPYmplY3ROYW1lUHJlZml4PkZhbHNlPC9BZGRHbG9iYWxPYmplY3ROYW1lUHJlZml4PjxBZGRHbG9iYWxPYmplY3RTdWZmaXg+RmFsc2U8L0FkZEdsb2JhbE9iamVjdFN1ZmZpeD48QWxsb3dBbGw+RmFsc2U8L0FsbG93QWxsPjxBbGxvd0RlZmF1bHQ+RmFsc2U8L0FsbG93RGVmYXVsdD48QWxsb3dFZGl0VGV4dEJveD5GYWxzZTwvQWxsb3dFZGl0VGV4dEJveD48QWxsb3dFbXB0eT5GYWxzZTwvQWxsb3dFbXB0eT48RGlhbG9nV2luZG93TmFtZT5TZWxlY3Rpb25EaWFsb2c8L0RpYWxvZ1dpbmRvd05hbWU+PEVkaXREaWFsb2dXaW5kb3dIZWlnaHQ+NzAwPC9FZGl0RGlhbG9nV2luZG93SGVpZ2h0PjxFZGl0RGlhbG9nV2luZG93V2lkdGg+MTAwMDwvRWRpdERpYWxvZ1dpbmRvd1dpZHRoPjxFZGl0V2luZG93TmFtZT5FZGl0V2luZG93PC9FZGl0V2luZG93TmFtZT48RW5jb2RlT3V0cHV0PlRydWU8L0VuY29kZU91dHB1dD48R2xvYmFsT2JqZWN0U3VmZml4IGlzbWFjcm89XCJ0cnVlXCI+eyRnZW5lcmFsLmdsb2JhbCR9PC9HbG9iYWxPYmplY3RTdWZmaXg+PEl0ZW1zUGVyUGFnZT4yNTwvSXRlbXNQZXJQYWdlPjxMb2NhbGl6ZUl0ZW1zPlRydWU8L0xvY2FsaXplSXRlbXM+PE1heERpc3BsYXllZEl0ZW1zPjI1PC9NYXhEaXNwbGF5ZWRJdGVtcz48TWF4RGlzcGxheWVkVG90YWxJdGVtcz41MDwvTWF4RGlzcGxheWVkVG90YWxJdGVtcz48T2JqZWN0VHlwZT5DTVMuUm9sZTwvT2JqZWN0VHlwZT48UmVtb3ZlTXVsdGlwbGVDb21tYXM+RmFsc2U8L1JlbW92ZU11bHRpcGxlQ29tbWFzPjxSZXR1cm5Db2x1bW5OYW1lPlJvbGVOYW1lPC9SZXR1cm5Db2x1bW5OYW1lPjxSZXR1cm5Db2x1bW5UeXBlPmlkPC9SZXR1cm5Db2x1bW5UeXBlPjxTZWxlY3Rpb25Nb2RlPjE8L1NlbGVjdGlvbk1vZGU+PFZhbHVlc1NlcGFyYXRvcj47PC9WYWx1ZXNTZXBhcmF0b3I+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYW1wYWlnbiBNYW5hZ2VtZW50In19")]
        public const string KDA_GlobalAminRoleName = "KDA_GlobalAminRoleName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkhlbHAgTW9kdWxlIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0hlbHBNb2R1bGVFbmFibGVkIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6ImVuYWJsZWQiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IlJhZGlvQnV0dG9uc0NvbnRyb2wiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxPcHRpb25zPmVuYWJsZWQ7RW5hYmxlZFxuZGlzYWJsZWQ7RGlzYWJsZWRcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTW9kdWxlIEFjY2VzcyJ9fQ==")]
        public const string KDA_HelpModuleEnabled = "KDA_HelpModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Admin Management")]
        [DefaultValueAttribute(@"hidden")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkhvbWUgTW9kdWxlIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0hvbWVNb2R1bGVFbmFibGVkIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6ImhpZGRlbiIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXHJcbmRpc2FibGVkO0Rpc2FibGVkXHJcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQWRtaW4gTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_HomeModuleEnabled = "KDA_HomeModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(@"hidden")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkluYm91bmQgVHJhY2tpbmcgTW9kdWxlIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0luYm91bmRUcmFja2luZ01vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaGlkZGVuIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSYWRpb0J1dHRvbnNDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48T3B0aW9ucz5lbmFibGVkO0VuYWJsZWRcclxuZGlzYWJsZWQ7RGlzYWJsZWRcclxuaGlkZGVuO0hpZGRlbjwvT3B0aW9ucz48UmVwZWF0RGlyZWN0aW9uPmhvcml6b250YWw8L1JlcGVhdERpcmVjdGlvbj48UmVwZWF0TGF5b3V0PkZsb3c8L1JlcGVhdExheW91dD48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYW1wYWlnbiBNYW5hZ2VtZW50In19")]
        public const string KDA_InboundTrackingModuleEnabled = "KDA_InboundTrackingModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Modules")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ikluc2lnaHRzIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0luc2lnaHRzRW5hYmxlZCIsIktleVR5cGUiOiJib29sZWFuIiwiS2V5RGVmYXVsdFZhbHVlIjoiRmFsc2UiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTW9kdWxlcyJ9fQ==")]
        public const string KDA_InsightsEnabled = "KDA_InsightsEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Home Page")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkludmVudG9yeSBDYXRhbG9nIEltYWdlIiwiS2V5TmFtZSI6IktEQV9JbnZlbnRvcnlDYXRhbG9nSW1hZ2UiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6Ik1lZGlhU2VsZWN0aW9uQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkhvbWUgUGFnZSJ9fQ==")]
        public const string KDA_InventoryCatalogImage = "KDA_InventoryCatalogImage";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Home Page")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkludmVudG9yeSBDYXRhbG9nIFVSTCIsIktleU5hbWUiOiJLREFfSW52ZW50b3J5Q2F0YWxvZ1VSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkhvbWUgUGFnZSJ9fQ==")]
        public const string KDA_InventoryCatalogURL = "KDA_InventoryCatalogURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Inventory")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkVkaXQgUHJvZHVjdCBQYWdlIFVSTCIsIktleU5hbWUiOiJLREFfSW52ZW50b3J5UHJvZHVjdEVkaXRQYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBJbnZlbnRvcnkifX0=")]
        public const string KDA_InventoryProductEditPageUrl = "KDA_InventoryProductEditPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Inventory")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkludmVudG9yeSBQYXRoIiwiS2V5TmFtZSI6IktEQV9JbnZlbnRvcnlQcm9kdWN0Rm9sZGVyUGF0aCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgSW52ZW50b3J5In19")]
        public const string KDA_InventoryProductFolderPath = "KDA_InventoryProductFolderPath";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Inventory")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik1lZGlhIEZvbGRlciBOYW1lIiwiS2V5TmFtZSI6IktEQV9JbnZlbnRvcnlQcm9kdWN0SW1hZ2VGb2xkZXJOYW1lIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgSW52ZW50b3J5In19")]
        public const string KDA_InventoryProductImageFolderName = "KDA_InventoryProductImageFolderName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Inventory")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWF0ZSBQcm9kdWN0IFBhZ2UgVVJMIiwiS2V5TmFtZSI6IktEQV9JbnZlbnRvcnlQcm9kdWN0UGFnZVVybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgSW52ZW50b3J5In19")]
        public const string KDA_InventoryProductPageUrl = "KDA_InventoryProductPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"https://c664f4qva8.execute-api.us-east-1.amazonaws.com/Qa")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlVwZGF0ZSBJbnZlbnRvcnkgU2VydmljZSBFbmRwb2ludCIsIktleU5hbWUiOiJLREFfSW52ZW50b3J5VXBkYXRlU2VydmljZUVuZHBvaW50IiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Imh0dHBzOi8vYzY2NGY0cXZhOC5leGVjdXRlLWFwaS51cy1lYXN0LTEuYW1hem9uYXdzLmNvbS9RYSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJPcmRlcnMifX0=")]
        public const string KDA_InventoryUpdateServiceEndpoint = "KDA_InventoryUpdateServiceEndpoint";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Cart Management")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByZSBCdXkgQ2FydCBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9Jc1ByZUJ1eUNhcnRFbmFibGVkIiwiS2V5VHlwZSI6ImJvb2xlYW4iLCJLZXlEZWZhdWx0VmFsdWUiOiJGYWxzZSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiQ2hlY2tCb3hDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ2FydCBNYW5hZ2VtZW50In19")]
        public const string KDA_IsPreBuyCartEnabled = "KDA_IsPreBuyCartEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkstREFNIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9LREFNTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJlbmFibGVkIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSYWRpb0J1dHRvbnNDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48T3B0aW9ucz5lbmFibGVkO0VuYWJsZWRcbmRpc2FibGVkO0Rpc2FibGVkXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1vZHVsZSBBY2Nlc3MifX0=")]
        public const string KDA_KDAMModuleEnabled = "KDA_KDAMModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkstSW5zaWdodHMgTW9kdWxlIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0tJbnNpZ2h0c01vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiZW5hYmxlZCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXG5kaXNhYmxlZDtEaXNhYmxlZFxuaGlkZGVuO0hpZGRlbjwvT3B0aW9ucz48UmVwZWF0RGlyZWN0aW9uPmhvcml6b250YWw8L1JlcGVhdERpcmVjdGlvbj48UmVwZWF0TGF5b3V0PkZsb3c8L1JlcGVhdExheW91dD48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNb2R1bGUgQWNjZXNzIn19")]
        public const string KDA_KInsightsModuleEnabled = "KDA_KInsightsModuleEnabled";

        /// <summary>
        /// KDA KListMapColumnsPageURL
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Mailing list")]
        [DefaultValueAttribute(@"/k-list/map-columns")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBLTGlzdE1hcENvbHVtbnNQYWdlVVJMIiwiS2V5TmFtZSI6IktEQV9LTGlzdE1hcENvbHVtbnNQYWdlVVJMIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9rLWxpc3QvbWFwLWNvbHVtbnMiLCJLZXlEZXNjcmlwdGlvbiI6IktEQSBLTGlzdE1hcENvbHVtbnNQYWdlVVJMIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1haWxpbmcgbGlzdCJ9fQ==")]
        public const string KDA_KListMapColumnsPageURL = "KDA_KListMapColumnsPageURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkstTGlzdCBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfS0xpc3RNb2R1bGVFbmFibGVkIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6ImVuYWJsZWQiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IlJhZGlvQnV0dG9uc0NvbnRyb2wiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxPcHRpb25zPmVuYWJsZWQ7RW5hYmxlZFxuZGlzYWJsZWQ7RGlzYWJsZWRcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTW9kdWxlIEFjY2VzcyJ9fQ==")]
        public const string KDA_KListModuleEnabled = "KDA_KListModuleEnabled";

        /// <summary>
        /// KDA KListNewPageURL
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Mailing list")]
        [DefaultValueAttribute(@"/k-list/new-k-list")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBLTGlzdE5ld1BhZ2VVUkwiLCJLZXlOYW1lIjoiS0RBX0tMaXN0TmV3UGFnZVVSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvay1saXN0L25ldy1rLWxpc3QiLCJLZXlEZXNjcmlwdGlvbiI6IktEQSBLTGlzdE5ld1BhZ2VVUkwiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWFpbGluZyBsaXN0In19")]
        public const string KDA_KListNewPageURL = "KDA_KListNewPageURL";

        /// <summary>
        /// KDA KListProcessedPageURL
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Mailing list")]
        [DefaultValueAttribute(@"/k-list/your-processed-list")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBLTGlzdFByb2Nlc3NlZFBhZ2VVUkwiLCJLZXlOYW1lIjoiS0RBX0tMaXN0UHJvY2Vzc2VkUGFnZVVSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvay1saXN0L3lvdXItcHJvY2Vzc2VkLWxpc3QiLCJLZXlEZXNjcmlwdGlvbiI6IktEQSBLTGlzdFByb2Nlc3NlZFBhZ2VVUkwiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWFpbGluZyBsaXN0In19")]
        public const string KDA_KListProcessedPageURL = "KDA_KListProcessedPageURL";

        /// <summary>
        /// KDA KListProcessingPageURL
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Mailing list")]
        [DefaultValueAttribute(@"/k-list/processing")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBLTGlzdFByb2Nlc3NpbmdQYWdlVVJMIiwiS2V5TmFtZSI6IktEQV9LTGlzdFByb2Nlc3NpbmdQYWdlVVJMIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9rLWxpc3QvcHJvY2Vzc2luZyIsIktleURlc2NyaXB0aW9uIjoiS0RBIEtMaXN0UHJvY2Vzc2luZ1BhZ2VVUkwiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWFpbGluZyBsaXN0In19")]
        public const string KDA_KListProcessingPageURL = "KDA_KListProcessingPageURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkstUHJvb2YgTW9kdWxlIEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX0tQcm9vZk1vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiZW5hYmxlZCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXG5kaXNhYmxlZDtEaXNhYmxlZFxuaGlkZGVuO0hpZGRlbjwvT3B0aW9ucz48UmVwZWF0RGlyZWN0aW9uPmhvcml6b250YWw8L1JlcGVhdERpcmVjdGlvbj48UmVwZWF0TGF5b3V0PkZsb3c8L1JlcGVhdExheW91dD48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNb2R1bGUgQWNjZXNzIn19")]
        public const string KDA_KProofModuleEnabled = "KDA_KProofModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkstU291cmNlIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9LU291cmNlTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJlbmFibGVkIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSYWRpb0J1dHRvbnNDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48T3B0aW9ucz5lbmFibGVkO0VuYWJsZWRcbmRpc2FibGVkO0Rpc2FibGVkXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1vZHVsZSBBY2Nlc3MifX0=")]
        public const string KDA_KSourceModuleEnabled = "KDA_KSourceModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"/k-source/new-request")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktTb3VyY2UgTmV3IFJlcXVlc3QgUGFnZSBVUkwiLCJLZXlOYW1lIjoiS0RBX0tTb3VyY2VOZXdSZXF1ZXN0UGFnZVVSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvay1zb3VyY2UvbmV3LXJlcXVlc3QiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJLLVNvdXJjZSJ9fQ==")]
        public const string KDA_KSourceNewRequestPageURL = "KDA_KSourceNewRequestPageURL";

        /// <summary>
        /// Interval in days after which expired mailing lists will be deleted.
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Mailing list")]
        [DefaultValueAttribute(@"1")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRlbGV0ZSBleHBpcmVkIGxpc3QgYWZ0ZXIiLCJLZXlOYW1lIjoiS0RBX01haWxpbmdMaXN0X0RlbGV0ZUV4cGlyZWRBZnRlciIsIktleVR5cGUiOiJpbnQiLCJLZXlEZWZhdWx0VmFsdWUiOiIxIiwiS2V5RGVzY3JpcHRpb24iOiJJbnRlcnZhbCBpbiBkYXlzIGFmdGVyIHdoaWNoIGV4cGlyZWQgbWFpbGluZyBsaXN0cyB3aWxsIGJlIGRlbGV0ZWQuIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiRHJvcERvd25MaXN0Q29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PERpc3BsYXlBY3R1YWxWYWx1ZUFzSXRlbT5GYWxzZTwvRGlzcGxheUFjdHVhbFZhbHVlQXNJdGVtPjxFZGl0VGV4dD5GYWxzZTwvRWRpdFRleHQ+PE9wdGlvbnM+O25ldmVyXG4xOzEgZGF5c1xuNjA7NjAgZGF5c1xuOTA7OTAgZGF5czwvT3B0aW9ucz48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNYWlsaW5nIGxpc3QifX0=")]
        public const string KDA_MailingList_DeleteExpiredAfter = "KDA_MailingList_DeleteExpiredAfter";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Mailing list")]
        [DefaultValueAttribute(@"https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa/Api/Mailing/ByFilter")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRlbGV0ZSBsaXN0IGJ5IGZpbHRlciBVUkwiLCJLZXlOYW1lIjoiS0RBX01haWxpbmdMaXN0X0RlbGV0ZUxpc3RCeUZpbHRlclVSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJodHRwczovL3dlamdwbm4wM2UuZXhlY3V0ZS1hcGkudXMtZWFzdC0xLmFtYXpvbmF3cy5jb20vUWEvQXBpL01haWxpbmcvQnlGaWx0ZXIiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiTWFpbGluZyBsaXN0In19")]
        public const string KDA_MailingList_DeleteListByFilterURL = "KDA_MailingList_DeleteListByFilterURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Forms")]
        [DefaultValueAttribute(@"andrey.konovka@actum.cz")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBLaXQgUmVxdWVzdCBSZWNpcGllbnQgRW1haWwgQWRkcmVzcyIsIktleU5hbWUiOiJLREFfTmV3S2l0UmVxdWVzdFJlY2lwaWVudEVtYWlsQWRkcmVzcyIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJhbmRyZXkua29ub3ZrYUBhY3R1bS5jeiIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJGb3JtcyJ9fQ==")]
        public const string KDA_NewKitRequestRecipientEmailAddress = "KDA_NewKitRequestRecipientEmailAddress";

        /// <summary>
        /// Email address/addresses of mailboxes for receive new product requests through new product request form.
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Forms")]
        [DefaultValueAttribute(@"andrey.konovka@actum.cz")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBQcm9kdWN0IFJlcXVlc3QgUmVjaXBpZW50IEVtYWlsIEFkZHJlc3MiLCJLZXlOYW1lIjoiS0RBX05ld1Byb2R1Y3RSZXF1ZXN0UmVjaXBpZW50RW1haWxBZGRyZXNzIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6ImFuZHJleS5rb25vdmthQGFjdHVtLmN6IiwiS2V5RGVzY3JpcHRpb24iOiJFbWFpbCBhZGRyZXNzL2FkZHJlc3NlcyBvZiBtYWlsYm94ZXMgZm9yIHJlY2VpdmUgbmV3IHByb2R1Y3QgcmVxdWVzdHMgdGhyb3VnaCBuZXcgcHJvZHVjdCByZXF1ZXN0IGZvcm0uIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJGb3JtcyJ9fQ==")]
        public const string KDA_NewProductRequestRecipientEmailAddress = "KDA_NewProductRequestRecipientEmailAddress";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"https://api.scd.noosh.com/api/v1/")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBOb29zaEFwaSIsIktleU5hbWUiOiJLREFfTm9vc2hBcGkiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaHR0cHM6Ly9hcGkuc2NkLm5vb3NoLmNvbS9hcGkvdjEvIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkstU291cmNlIn19")]
        public const string KDA_NooshApi = "KDA_NooshApi";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"1440")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5vb3NoIEV2ZW50IFJhdGUiLCJLZXlOYW1lIjoiS0RBX05vb3NoRXZlbnRSYXRlIiwiS2V5VHlwZSI6ImludCIsIktleURlZmF1bHRWYWx1ZSI6IjE0NDAiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiSy1Tb3VyY2UifX0=")]
        public const string KDA_NooshEventRate = "KDA_NooshEventRate";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"NooshEvent")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5vb3NoIGV2ZW50IHJ1bGUgbmFtZSIsIktleU5hbWUiOiJLREFfTm9vc2hFdmVudFJ1bGVOYW1lIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ik5vb3NoRXZlbnQiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiSy1Tb3VyY2UifX0=")]
        public const string KDA_NooshEventRuleName = "KDA_NooshEventRuleName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"Id255380460113")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5vb3NoIEV2ZW50IFRhcmdldCBJZCIsIktleU5hbWUiOiJLREFfTm9vc2hFdmVudFRhcmdldElkIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IklkMjU1MzgwNDYwMTEzIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkstU291cmNlIn19")]
        public const string KDA_NooshEventTargetId = "KDA_NooshEventTargetId";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"IEKdhpOWS5IEq2aQHAp1ivCtEnlW1f1Jf0JhbbHsbN1IToFoqo5SrInnH5Kw7cnb")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBOb29zaFRva2VuIiwiS2V5TmFtZSI6IktEQV9Ob29zaFRva2VuIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IklFS2RocE9XUzVJRXEyYVFIQXAxaXZDdEVubFcxZjFKZjBKaGJiSHNiTjFJVG9Gb3FvNVNySW5uSDVLdzdjbmIiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiSy1Tb3VyY2UifX0=")]
        public const string KDA_NooshToken = "KDA_NooshToken";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"/recent-orders/order-detail")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVyIGRldGFpbCBwYWdlIFVSTCIsIktleU5hbWUiOiJLREFfT3JkZXJEZXRhaWxVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiL3JlY2VudC1vcmRlcnMvb3JkZXItZGV0YWlsIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiT3JkZXJzIn19")]
        public const string KDA_OrderDetailUrl = "KDA_OrderDetailUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"hb00487436@techmahindra.com")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVyIEluZm8gRS1tYWlsIEFkZHJlc3MiLCJLZXlOYW1lIjoiS0RBX09yZGVyTm90aWZpY2F0aW9uRW1haWwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaGIwMDQ4NzQzNkB0ZWNobWFoaW5kcmEuY29tIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik9yZGVycyJ9fQ==")]
        public const string KDA_OrderNotificationEmail = "KDA_OrderNotificationEmail";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVycyBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfT3JkZXJzTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJlbmFibGVkIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJSYWRpb0J1dHRvbnNDb250cm9sIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48T3B0aW9ucz5lbmFibGVkO0VuYWJsZWRcbmRpc2FibGVkO0Rpc2FibGVkXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik1vZHVsZSBBY2Nlc3MifX0=")]
        public const string KDA_OrdersModuleEnabled = "KDA_OrdersModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"https://9chbp44h4j.execute-api.us-east-1.amazonaws.com/Qa")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVyIFN0YXRpc3RpY3MgU2VydmljZSIsIktleU5hbWUiOiJLREFfT3JkZXJTdGF0aXN0aWNzU2VydmljZUVuZHBvaW50IiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Imh0dHBzOi8vOWNoYnA0NGg0ai5leGVjdXRlLWFwaS51cy1lYXN0LTEuYW1hem9uYXdzLmNvbS9RYSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJPcmRlcnMifX0=")]
        public const string KDA_OrderStatisticsServiceEndpoint = "KDA_OrderStatisticsServiceEndpoint";

        /// <summary>
        /// Url of order submitted page
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"/Order-submitted")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVyIHN1Ym1pdHRlZCB1cmwiLCJLZXlOYW1lIjoiS0RBX09yZGVyU3VibWl0dGVkVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9PcmRlci1zdWJtaXR0ZWQiLCJLZXlEZXNjcmlwdGlvbiI6IlVybCBvZiBvcmRlciBzdWJtaXR0ZWQgcGFnZSIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJPcmRlcnMifX0=")]
        public const string KDA_OrderSubmittedUrl = "KDA_OrderSubmittedUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FY Setup")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlBPUyBCdWxrIFVwbG9hZCBQYWdlIiwiS2V5TmFtZSI6IktEQV9QT1NCdWxrVXBsb2FkUGFnZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkZZIFNldHVwIn19")]
        public const string KDA_POSBulkUploadPage = "KDA_POSBulkUploadPage";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FY Setup")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBQT1MgUGFnZSBVUkwiLCJLZXlOYW1lIjoiS0RBX1BPU0NyZWF0ZVBhZ2VVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJGWSBTZXR1cCJ9fQ==")]
        public const string KDA_POSCreatePageURL = "KDA_POSCreatePageURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FY Setup")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlBPUyBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfUE9TTW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXHJcbmRpc2FibGVkO0Rpc2FibGVkXHJcbmhpZGRlbjtIaWRkZW48L09wdGlvbnM+PFJlcGVhdERpcmVjdGlvbj5ob3Jpem9udGFsPC9SZXBlYXREaXJlY3Rpb24+PFJlcGVhdExheW91dD5GbG93PC9SZXBlYXRMYXlvdXQ+PFNvcnRJdGVtcz5GYWxzZTwvU29ydEl0ZW1zPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiRlkgU2V0dXAifX0=")]
        public const string KDA_POSModuleEnabled = "KDA_POSModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Home Page")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByZUJ1eSBDYXRhbG9nIEltYWdlIiwiS2V5TmFtZSI6IktEQV9QcmVidXlDYXRhbG9nSW1hZ2UiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6Ik1lZGlhU2VsZWN0aW9uQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkhvbWUgUGFnZSJ9fQ==")]
        public const string KDA_PrebuyCatalogImage = "KDA_PrebuyCatalogImage";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Home Page")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByZUJ1eSBDYXRhbG9nVVJMIiwiS2V5TmFtZSI6IktEQV9QcmVidXlDYXRhbG9nVVJMIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiSG9tZSBQYWdlIn19")]
        public const string KDA_PrebuyCatalogURL = "KDA_PrebuyCatalogURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(@"hidden")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByZWJ1eSBNb2R1bGUgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfUHJlYnV5TW9kdWxlRW5hYmxlZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJoaWRkZW4iLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IlJhZGlvQnV0dG9uc0NvbnRyb2wiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxPcHRpb25zPmVuYWJsZWQ7RW5hYmxlZFxyXG5kaXNhYmxlZDtEaXNhYmxlZFxyXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNhbXBhaWduIE1hbmFnZW1lbnQifX0=")]
        public const string KDA_PrebuyModuleEnabled = "KDA_PrebuyModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByb2R1Y3RzIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9Qcm9kdWN0c01vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiZW5hYmxlZCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXG5kaXNhYmxlZDtEaXNhYmxlZFxuaGlkZGVuO0hpZGRlbjwvT3B0aW9ucz48UmVwZWF0RGlyZWN0aW9uPmhvcml6b250YWw8L1JlcGVhdERpcmVjdGlvbj48UmVwZWF0TGF5b3V0PkZsb3c8L1JlcGVhdExheW91dD48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNb2R1bGUgQWNjZXNzIn19")]
        public const string KDA_ProductsModuleEnabled = "KDA_ProductsModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBQcm9kdWN0cyBQYWdlIFVSTCIsIktleU5hbWUiOiJLREFfUHJvZHVjdHNQYXRoIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3Rkb2N1bWVudCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNhbXBhaWduIE1hbmFnZW1lbnQifX0=")]
        public const string KDA_ProductsPath = "KDA_ProductsPath";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(@"https://twe.kadenatest.com/getmedia/509fdf93-47c8-4cf2-8afd-0d42cd17853e/default-placeholder-300x300")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBQcm9kdWN0cyBQbGFjZSBIb2xkZXIgSW1hZ2UiLCJLZXlOYW1lIjoiS0RBX1Byb2R1Y3RzUGxhY2VIb2xkZXJJbWFnZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJodHRwczovL3R3ZS5rYWRlbmF0ZXN0LmNvbS9nZXRtZWRpYS81MDlmZGY5My00N2M4LTRjZjItOGFmZC0wZDQyY2QxNzg1M2UvZGVmYXVsdC1wbGFjZWhvbGRlci0zMDB4MzAwIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkN1c3RvbSBDYXRhbG9nIFBERiBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_ProductsPlaceHolderImage = "KDA_ProductsPlaceHolderImage";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Program Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByb2dyYW1zIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9Qcm9ncmFtc01vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IlJhZGlvQnV0dG9uc0NvbnRyb2wiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxPcHRpb25zPmVuYWJsZWQ7RW5hYmxlZFxyXG5kaXNhYmxlZDtEaXNhYmxlZFxyXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IlByb2dyYW0gTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_ProgramsModuleEnabled = "KDA_ProgramsModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"50")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik9yZGVycyBwZXIgcGFnZSBmb3IgUmVjZW50IE9yZGVycyIsIktleU5hbWUiOiJLREFfUmVjZW50T3JkZXJzUGFnZUNhcGFjaXR5IiwiS2V5VHlwZSI6ImludCIsIktleURlZmF1bHRWYWx1ZSI6IjUwIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik9yZGVycyJ9fQ==")]
        public const string KDA_RecentOrdersPageCapacity = "KDA_RecentOrdersPageCapacity";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Module Access")]
        [DefaultValueAttribute(@"enabled")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNldHRpbmdzIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9TZXR0aW5nc01vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiZW5hYmxlZCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUmFkaW9CdXR0b25zQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PE9wdGlvbnM+ZW5hYmxlZDtFbmFibGVkXG5kaXNhYmxlZDtEaXNhYmxlZFxuaGlkZGVuO0hpZGRlbjwvT3B0aW9ucz48UmVwZWF0RGlyZWN0aW9uPmhvcml6b250YWw8L1JlcGVhdERpcmVjdGlvbj48UmVwZWF0TGF5b3V0PkZsb3c8L1JlcGVhdExheW91dD48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNb2R1bGUgQWNjZXNzIn19")]
        public const string KDA_SettingsModuleEnabled = "KDA_SettingsModuleEnabled";

        /// <summary>
        /// Used in "submit order" as default value when Customer has not filled in his "Company name"
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Carrier providers")]
        [DefaultValueAttribute(@"customer company name")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRlZmF1bHQgc2hpcCB0byBjb21wYW55IG5hbWUiLCJLZXlOYW1lIjoiS0RBX1NoaXBwaW5nQWRkcmVzc19EZWZhdWx0Q29tcGFueU5hbWUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiY3VzdG9tZXIgY29tcGFueSBuYW1lIiwiS2V5RGVzY3JpcHRpb24iOiJVc2VkIGluIFwic3VibWl0IG9yZGVyXCIgYXMgZGVmYXVsdCB2YWx1ZSB3aGVuIEN1c3RvbWVyIGhhcyBub3QgZmlsbGVkIGluIGhpcyBcIkNvbXBhbnkgbmFtZVwiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYXJyaWVyIHByb3ZpZGVycyJ9fQ==")]
        public const string KDA_ShippingAddress_DefaultCompanyName = "KDA_ShippingAddress_DefaultCompanyName";

        /// <summary>
        /// Roles to assign during self registration.
        /// </summary>
        [CategoryAttribute("Membership", "KDAMembershipSettingCategory", "Kadena")]
        [GroupAttribute("Registration")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRlZmF1bHQgcm9sZSIsIktleU5hbWUiOiJLREFfU2lnbnVwRGVmYXVsdFJvbGUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IlJvbGVzIHRvIGFzc2lnbiBkdXJpbmcgc2VsZiByZWdpc3RyYXRpb24uIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiUm9sZUNoZWNrYm94U2VsZWN0b3IiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6Ik1lbWJlcnNoaXAiLCJOYW1lIjoiS0RBTWVtYmVyc2hpcFNldHRpbmdDYXRlZ29yeSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkthZGVuYSJ9LCJEaXNwbGF5TmFtZSI6IlJlZ2lzdHJhdGlvbiJ9fQ==")]
        public const string KDA_SignupDefaultRole = "KDA_SignupDefaultRole";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Membership", "KDAMembershipSettingCategory", "Kadena")]
        [GroupAttribute("Registration")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlJlZ2lzdHJhdGlvbiBwYWdlIiwiS2V5TmFtZSI6IktEQV9SZWdpc3RyYXRpb25QYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJNZW1iZXJzaGlwIiwiTmFtZSI6IktEQU1lbWJlcnNoaXBTZXR0aW5nQ2F0ZWdvcnkiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJLYWRlbmEifSwiRGlzcGxheU5hbWUiOiJSZWdpc3RyYXRpb24ifX0=")]
        public const string KDA_RegistrationPageUrl = "KDA_RegistrationPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Membership", "KDAMembershipSettingCategory", "Kadena")]
        [GroupAttribute("Registration")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkVuYWJsZSByZWdpc3RyYXRpb24iLCJLZXlOYW1lIjoiS0RBX0VuYWJsZVJlZ2lzdHJhdGlvbiIsIktleVR5cGUiOiJib29sZWFuIiwiS2V5RGVmYXVsdFZhbHVlIjoiRmFsc2UiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJNZW1iZXJzaGlwIiwiTmFtZSI6IktEQU1lbWJlcnNoaXBTZXR0aW5nQ2F0ZWdvcnkiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJLYWRlbmEifSwiRGlzcGxheU5hbWUiOiJSZWdpc3RyYXRpb24ifX0=")]
        public const string KDA_EnableRegistration = "KDA_EnableRegistration";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Insights")]
        [DefaultValueAttribute(@"40001130")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkN1c3RvbWVySUQiLCJLZXlOYW1lIjoiS0RBX1Nwb3RmaXJlX0N1c3RvbWVySUQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiNDAwMDExMzAiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiSy1JbnNpZ2h0cyJ9fQ==")]
        public const string KDA_Spotfire_CustomerID = "KDA_Spotfire_CustomerID";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Insights")]
        [DefaultValueAttribute(@"https://spotfireqa.cenveo.com/spotfire/wp/GetJavaScriptApi.ashx?Version=7.5")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkdldEphdmFTY3JpcHRBcGkgc2NyaXB0IFVSTCIsIktleU5hbWUiOiJLREFfU3BvdGZpcmVfR2V0SmF2YVNjcmlwdEFwaVVSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJodHRwczovL3Nwb3RmaXJlcWEuY2VudmVvLmNvbS9zcG90ZmlyZS93cC9HZXRKYXZhU2NyaXB0QXBpLmFzaHg/VmVyc2lvbj03LjUiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiSy1JbnNpZ2h0cyJ9fQ==")]
        public const string KDA_Spotfire_GetJavaScriptApiURL = "KDA_Spotfire_GetJavaScriptApiURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Insights")]
        [DefaultValueAttribute(@"/Kadena/Standard Reports/Inventory and Sales order Report_Single graph")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ikluc2lnaHRzIC0gbWFpbiBwYWdlIFVSTCIsIktleU5hbWUiOiJLREFfU3BvdGZpcmVfSW5zaWdodHNNYWluUGFnZVVSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvS2FkZW5hL1N0YW5kYXJkIFJlcG9ydHMvSW52ZW50b3J5IGFuZCBTYWxlcyBvcmRlciBSZXBvcnRfU2luZ2xlIGdyYXBoIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkstSW5zaWdodHMifX0=")]
        public const string KDA_Spotfire_InsightsMainPageURL = "KDA_Spotfire_InsightsMainPageURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Insights")]
        [DefaultValueAttribute(@"https://spotfireqa.cenveo.com/spotfire/wp")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNlcnZlciBVUkwiLCJLZXlOYW1lIjoiS0RBX1Nwb3RmaXJlX1NlcnZlclVSTCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJodHRwczovL3Nwb3RmaXJlcWEuY2VudmVvLmNvbS9zcG90ZmlyZS93cCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJLLUluc2lnaHRzIn19")]
        public const string KDA_Spotfire_ServerURL = "KDA_Spotfire_ServerURL";
       
        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Templating")]
        [DefaultValueAttribute(@"/products/product-tools/product-editor")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByb2R1Y3QgZWRpdG9yIHVybCIsIktleU5hbWUiOiJLREFfVGVtcGxhdGluZ19Qcm9kdWN0RWRpdG9yVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9wcm9kdWN0cy9wcm9kdWN0LXRvb2xzL3Byb2R1Y3QtZWRpdG9yIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IlRlbXBsYXRpbmcifX0=")]
        public const string KDA_Templating_ProductEditorUrl = "KDA_Templating_ProductEditorUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Orders")]
        [DefaultValueAttribute(@"https://0v7afs259k.execute-api.us-east-1.amazonaws.com/Qa/")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlRlbXBsYXRpbmcgU2VydmljZSBFbmRwb2ludCIsIktleU5hbWUiOiJLREFfVGVtcGxhdGluZ1NlcnZpY2VFbmRwb2ludCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJodHRwczovLzB2N2FmczI1OWsuZXhlY3V0ZS1hcGkudXMtZWFzdC0xLmFtYXpvbmF3cy5jb20vUWEvIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik9yZGVycyJ9fQ==")]
        public const string KDA_TemplatingServiceEndpoint = "KDA_TemplatingServiceEndpoint";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("FY Setup")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlVzZXIgRlkgQnVkZ2V0IEVuYWJsZWQiLCJLZXlOYW1lIjoiS0RBX1VzZXJGWUJ1ZGdldEVuYWJsZWQiLCJLZXlUeXBlIjoiYm9vbGVhbiIsIktleURlZmF1bHRWYWx1ZSI6IkZhbHNlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkZZIFNldHVwIn19")]
        public const string KDA_UserFYBudgetEnabled = "KDA_UserFYBudgetEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("User Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlVzZXJzIE1vZHVsZSBFbmFibGVkIiwiS2V5TmFtZSI6IktEQV9Vc2Vyc01vZHVsZUVuYWJsZWQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6IlJhZGlvQnV0dG9uc0NvbnRyb2wiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxPcHRpb25zPmVuYWJsZWQ7RW5hYmxlZFxyXG5kaXNhYmxlZDtEaXNhYmxlZFxyXG5oaWRkZW47SGlkZGVuPC9PcHRpb25zPjxSZXBlYXREaXJlY3Rpb24+aG9yaXpvbnRhbDwvUmVwZWF0RGlyZWN0aW9uPjxSZXBlYXRMYXlvdXQ+RmxvdzwvUmVwZWF0TGF5b3V0PjxTb3J0SXRlbXM+RmFsc2U8L1NvcnRJdGVtcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IlVzZXIgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_UsersModuleEnabled = "KDA_UsersModuleEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Mailing list")]
        [DefaultValueAttribute(@"https://o31vibmca2.execute-api.us-east-1.amazonaws.com/Qa")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFkZHJlc3MgdmFsaWRhdG9yIFVSTCIsIktleU5hbWUiOiJLREFfVmFsaWRhdGVBZGRyZXNzVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Imh0dHBzOi8vbzMxdmlibWNhMi5leGVjdXRlLWFwaS51cy1lYXN0LTEuYW1hem9uYXdzLmNvbS9RYSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJNYWlsaW5nIGxpc3QifX0=")]
        public const string KDA_ValidateAddressUrl = "KDA_ValidateAddressUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("WebAPI settings")]
        [DefaultValueAttribute(@"god")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlBhc3N3b3JkIiwiS2V5TmFtZSI6IktEQV9XZWJBcGlQYXNzd29yZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJnb2QiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiV2ViQVBJIHNldHRpbmdzIn19")]
        public const string KDA_WebApiPassword = "KDA_WebApiPassword";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("WebAPI settings")]
        [DefaultValueAttribute(@"root")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlVzZXIgbmFtZSIsIktleU5hbWUiOiJLREFfV2ViQXBpVXNlciIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJyb290IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IldlYkFQSSBzZXR0aW5ncyJ9fQ==")]
        public const string KDA_WebApiUser = "KDA_WebApiUser";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("K-Source")]
        [DefaultValueAttribute(@"Polaris")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ildvcmtncm91cCBOYW1lIiwiS2V5TmFtZSI6IktEQV9Xb3JrZ3JvdXBOYW1lIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IlBvbGFyaXMiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiSy1Tb3VyY2UifX0=")]
        public const string KDA_WorkgroupName = "KDA_WorkgroupName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX1Rlcm1pbmFsSWRlbnRpZmllcl9NZXJjaGFudENvZGUiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfVGVybWluYWxJZGVudGlmaWVyX01lcmNoYW50Q29kZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXRDYXJkIDNkc2kifX0=")]
        public const string KDA_CreditCard_TerminalIdentifier_MerchantCode = "KDA_CreditCard_TerminalIdentifier_MerchantCode";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX1Rlcm1pbmFsSWRlbnRpZmllcl9UZXJtaW5hbENvZGUiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfVGVybWluYWxJZGVudGlmaWVyX1Rlcm1pbmFsQ29kZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXRDYXJkIDNkc2kifX0=")]
        public const string KDA_CreditCard_TerminalIdentifier_TerminalCode = "KDA_CreditCard_TerminalIdentifier_TerminalCode";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX1Rlcm1pbmFsSWRlbnRpZmllcl9Mb2NhdGlvbkNvZGUiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfVGVybWluYWxJZGVudGlmaWVyX0xvY2F0aW9uQ29kZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXRDYXJkIDNkc2kifX0=")]
        public const string KDA_CreditCard_TerminalIdentifier_LocationCode = "KDA_CreditCard_TerminalIdentifier_LocationCode";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(@"Kadena2.0")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWRpdCBDYXJkIE1lcmNoYW50IENvZGUiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfTWVyY2hhbnRDb2RlIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IkthZGVuYTIuMCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXQgQ2FyZCBQYXltZW50In19")]
        public const string KDA_CreditCard_MerchantCode = "KDA_CreditCard_MerchantCode";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(@"Kadena2.0")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWRpdCBDYXJkIExvY2F0aW9uIENvZGUiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfTG9jYXRpb25Db2RlIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IkthZGVuYTIuMCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXQgQ2FyZCBQYXltZW50In19")]
        public const string KDA_CreditCard_LocationCode = "KDA_CreditCard_LocationCode";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(@"yCode1234572")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNyZWRpdCBDYXJkIENvZGUiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfQ29kZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJ5Q29kZTEyMzQ1NzIiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ3JlZGl0IENhcmQgUGF5bWVudCJ9fQ==")]
        public const string KDA_CreditCard_Code = "KDA_CreditCard_Code";


        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX1BheW1lbnRSZXN1bHRQYWdlIiwiS2V5TmFtZSI6IktEQV9DcmVkaXRDYXJkX1BheW1lbnRSZXN1bHRQYWdlIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkNyZWRpdENhcmQgM2RzaSJ9fQ==")]
        public const string KDA_CreditCard_PaymentResultPage = "KDA_CreditCard_PaymentResultPage";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkXzNEU2lfQXV0aG9yaXplRW5kcG9pbnQiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfM0RTaV9BdXRob3JpemVFbmRwb2ludCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXRDYXJkIDNkc2kifX0=")]
        public const string KDA_CreditCard_3DSi_AuthorizeEndpoint = "KDA_CreditCard_3DSi_AuthorizeEndpoint";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX1NhdmVUb2tlblVybCIsIktleU5hbWUiOiJLREFfQ3JlZGl0Q2FyZF9TYXZlVG9rZW5VcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ3JlZGl0Q2FyZCAzZHNpIn19")]
        public const string KDA_CreditCard_SaveTokenUrl = "KDA_CreditCard_SaveTokenUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute("https://r0uhzqogra.execute-api.us-east-1.amazonaws.com/Stage")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9Vc2VyRGF0YVNlcnZpY2VVcmwiLCJLZXlOYW1lIjoiS0RBX1VzZXJEYXRhU2VydmljZVVybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXQgQ2FyZCBQYXltZW50In19")]
        public const string KDA_UserDataServiceUrl = "KDA_UserDataServiceUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX0VuYWJsZVNhdmVDYXJkIiwiS2V5TmFtZSI6IktEQV9DcmVkaXRDYXJkX0VuYWJsZVNhdmVDYXJkIiwiS2V5VHlwZSI6ImJvb2xlYW4iLCJLZXlEZWZhdWx0VmFsdWUiOiJGYWxzZSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXRDYXJkIDNkc2kifX0=")]
        public const string KDA_CreditCard_EnableSaveCard = "KDA_CreditCard_EnableSaveCard";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Forms")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9OZXdLaXRSZXF1ZXN0RW5hYmxlZE9uU2l0ZSIsIktleU5hbWUiOiJLREFfTmV3S2l0UmVxdWVzdEVuYWJsZWRPblNpdGUiLCJLZXlUeXBlIjoiYm9vbGVhbiIsIktleURlZmF1bHRWYWx1ZSI6IkZhbHNlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkZvcm1zIn19")]
        public const string KDA_NewKitRequestEnabledOnSite = "KDA_NewKitRequestEnabledOnSite";
        
        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Amazon S3", "AmazonS3", "Kadena")]
        [GroupAttribute("General")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkVudmlyb25tZW50IiwiS2V5TmFtZSI6IktEQV9FbnZpcm9ubWVudElkIiwiS2V5VHlwZSI6ImludCIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJDdXN0b21UYWJsZUl0ZW1TZWxlY3RvciIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEN1c3RvbVRhYmxlPktEQS5FbnZpcm9ubWVudDwvQ3VzdG9tVGFibGU+PERpc3BsYXlOYW1lRm9ybWF0PnslRGlzcGxheU5hbWUlfTwvRGlzcGxheU5hbWVGb3JtYXQ+PFJldHVybkNvbHVtbk5hbWU+SXRlbUlEPC9SZXR1cm5Db2x1bW5OYW1lPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJBbWF6b24gUzMiLCJOYW1lIjoiQW1hem9uUzMiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJLYWRlbmEifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIn19")]
        public const string KDA_EnvironmentId = "KDA_EnvironmentId";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Amazon S3", "AmazonS3", "Kadena")]
        [GroupAttribute("General")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFtYXpvbiBTMyBidWNrZXQgbmFtZSIsIktleU5hbWUiOiJLREFfQW1hem9uUzNCdWNrZXROYW1lIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiQW1hem9uIFMzIiwiTmFtZSI6IkFtYXpvblMzIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiS2FkZW5hIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCJ9fQ==")]
        public const string KDA_AmazonS3BucketName = "KDA_AmazonS3BucketName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX0JpbGxpbmdBZGRyZXNzX0FkZHJlc3NMaW5lMSIsIktleU5hbWUiOiJLREFfQ3JlZGl0Q2FyZF9CaWxsaW5nQWRkcmVzc19BZGRyZXNzTGluZTEiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ3JlZGl0Q2FyZCAzZHNpIn19")]
        public const string KDA_CreditCard_BillingAddress_AddressLine1 = "KDA_CreditCard_BillingAddress_AddressLine1";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Credit Card Payment")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DcmVkaXRDYXJkX0JpbGxpbmdBZGRyZXNzX1Bvc3RhbENvZGUiLCJLZXlOYW1lIjoiS0RBX0NyZWRpdENhcmRfQmlsbGluZ0FkZHJlc3NfUG9zdGFsQ29kZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDcmVkaXRDYXJkIDNkc2kifX0=")]
        public const string KDA_CreditCard_BillingAddress_PostalCode = "KDA_CreditCard_BillingAddress_PostalCode";

        /// <summary>
        /// Thumbprint of certificate which is trusted to sign incoming SAML object.
        /// </summary>
        [CategoryAttribute("Membership", "KDAMembershipSettingCategory", "Kadena")]
        [GroupAttribute("SAML 2.0")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlRydXN0ZWQgY2VydGlmaWNhdGUgdGh1bWJwcmludCIsIktleU5hbWUiOiJLREFfVHJ1c3RlZENlcnRpZmljYXRlVGh1bWJwcmludCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiVGh1bWJwcmludCBvZiBjZXJ0aWZpY2F0ZSB3aGljaCBpcyB0cnVzdGVkIHRvIHNpZ24gaW5jb21pbmcgU0FNTCBvYmplY3QuIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6Ik1lbWJlcnNoaXAiLCJOYW1lIjoiS0RBTWVtYmVyc2hpcFNldHRpbmdDYXRlZ29yeSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkthZGVuYSJ9LCJEaXNwbGF5TmFtZSI6IlNBTUwgMi4wIn19")]
        public const string KDA_TrustedCertificateThumbprint = "KDA_TrustedCertificateThumbprint";

        /// <summary>
        /// Audience URI for which SAML object allowed to.
        /// </summary>
        [CategoryAttribute("Membership", "KDAMembershipSettingCategory", "Kadena")]
        [GroupAttribute("SAML 2.0")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFsbG93ZWQgYXVkaWVuY2UgVVJJIiwiS2V5TmFtZSI6IktEQV9BbGxvd2VkQXVkaWVuY2VVcmkiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IkF1ZGllbmNlIFVSSSBmb3Igd2hpY2ggU0FNTCBvYmplY3QgYWxsb3dlZCB0by4iLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiTWVtYmVyc2hpcCIsIk5hbWUiOiJLREFNZW1iZXJzaGlwU2V0dGluZ0NhdGVnb3J5IiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiS2FkZW5hIn0sIkRpc3BsYXlOYW1lIjoiU0FNTCAyLjAifX0=")]
        public const string KDA_AllowedAudienceUri = "KDA_AllowedAudienceUri";

        /// <summary>
        /// Default LowRes settings guid for newly created templated products
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Templating")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRlZmF1bHQgbG93cmVzIHNldHRpbmdzIGlkIiwiS2V5TmFtZSI6IktEQV9EZWZhdWx0TG93UmVzU2V0dGluZ3NJZCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJUZW1wbGF0aW5nIn19")]
        public const string KDA_DefaultLowResSettingsId = "KDA_DefaultLowResSettingsId";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Campaign Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBDYW1wYWlnbiBQcm9kdWN0IEFkZGVkIFRlbXBsYXRlIiwiS2V5TmFtZSI6IktEQV9DYW1wYWlnblByb2R1Y3RBZGRlZFRlbXBsYXRlIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJFbWFpbF90ZW1wbGF0ZV9zZWxlY3RvciIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PFNob3dFZGl0QnV0dG9uPlRydWU8L1Nob3dFZGl0QnV0dG9uPjxTaG93TmV3QnV0dG9uPlRydWU8L1Nob3dOZXdCdXR0b24+PFNpdGVOYW1lPiMjY3VycmVudHNpdGUjIzwvU2l0ZU5hbWU+PFRlbXBsYXRlVHlwZT5nZW5lcmFsPC9UZW1wbGF0ZVR5cGU+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDYW1wYWlnbiBNYW5hZ2VtZW50In19")]
        public const string KDA_CampaignProductAddedTemplate = "KDA_CampaignProductAddedTemplate";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Pre-buy Cart Management")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkZhaWxlZCBPcmRlcnMgUGFnZSBVcmwiLCJLZXlOYW1lIjoiS0RBX0ZhaWxlZE9yZGVyc1BhZ2VVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6InNlbGVjdHNpbmdsZXBhdGgiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBbGxvd1NldFBlcm1pc3Npb25zPkZhbHNlPC9BbGxvd1NldFBlcm1pc3Npb25zPjxTZWxlY3RhYmxlUGFnZVR5cGVzPjA8L1NlbGVjdGFibGVQYWdlVHlwZXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJQcmUtYnV5IENhcnQgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_FailedOrdersPageUrl = "KDA_FailedOrdersPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("Pre-buy Cart Management")]
        [DefaultValueAttribute(@"Ground")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRlZmF1bHQgU2hpcHBpbmcgT3B0aW9uIiwiS2V5TmFtZSI6IktEQV9EZWZhdWx0U2hpcHBwaW5nT3B0aW9uIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ikdyb3VuZCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoiRHJvcERvd25MaXN0Q29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PERpc3BsYXlBY3R1YWxWYWx1ZUFzSXRlbT5GYWxzZTwvRGlzcGxheUFjdHVhbFZhbHVlQXNJdGVtPjxFZGl0VGV4dD5GYWxzZTwvRWRpdFRleHQ+PFF1ZXJ5PnNlbGVjdCBTaGlwcGluZ09wdGlvbk5hbWUgYXMgdmFsdWUsU2hpcHBpbmdPcHRpb25EaXNwbGF5TmFtZSBhcyB0ZXh0IGZyb20gQ09NX1NoaXBwaW5nT3B0aW9uPC9RdWVyeT48U29ydEl0ZW1zPkZhbHNlPC9Tb3J0SXRlbXM+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJQcmUtYnV5IENhcnQgTWFuYWdlbWVudCJ9fQ==")]
        public const string KDA_DefaultShipppingOption = "KDA_DefaultShipppingOption";
        
        /// <summary>
        /// Maximum side size (px) for generating thumbnails of product's/category's image.
        /// </summary>
        [CategoryAttribute("Product", "KDASettingProductCategory", "ECommerceSettings")]
        [GroupAttribute("Image settings")]
        [DefaultValueAttribute(@"200")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlRodW1ibmFpbCdzIG1heGltdW0gc2lkZSBzaXplIiwiS2V5TmFtZSI6IktEQV9UaHVtYm5haWxNYXhTaWRlU2l6ZSIsIktleVR5cGUiOiJpbnQiLCJLZXlEZWZhdWx0VmFsdWUiOiIyMDAiLCJLZXlEZXNjcmlwdGlvbiI6Ik1heGltdW0gc2lkZSBzaXplIChweCkgZm9yIGdlbmVyYXRpbmcgdGh1bWJuYWlscyBvZiBwcm9kdWN0J3MvY2F0ZWdvcnkncyBpbWFnZS4iLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiUHJvZHVjdCIsIk5hbWUiOiJLREFTZXR0aW5nUHJvZHVjdENhdGVnb3J5IiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiRUNvbW1lcmNlU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJJbWFnZSBzZXR0aW5ncyJ9fQ==")]
        public const string KDA_ThumbnailMaxSideSize = "KDA_ThumbnailMaxSideSize";        

        /// <summary>
        /// Hash salt used to keep secure HiRes Pdf link. If changed later, pdf links hashed using old salt will NOT work
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"4rt5yh7rt8ye7rth68y7erh68y74e")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9IaXJlc1BkZkxpbmtIYXNoU2FsdCIsIktleU5hbWUiOiJLREFfSGlyZXNQZGZMaW5rSGFzaFNhbHQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiNHJ0NXloN3J0OHllN3J0aDY4eTdlcmg2OHk3NGUiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_HiresPdfLinkHashSalt = "KDA_HiresPdfLinkHashSalt";
    }
}