using Kadena.Models.SiteSettings.Attributes;

namespace Kadena.Models.SiteSettings
{
    public partial class Settings
    {
        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"271")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkFkZHJlc3MgRGVmYXVsdCBDb3VudHJ5IiwiS2V5TmFtZSI6IktEQV9BZGRyZXNzRGVmYXVsdENvdW50cnkiLCJLZXlUeXBlIjoiaW50IiwiS2V5RGVmYXVsdFZhbHVlIjoiMjcxIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJjb3VudHJ5U2VsZWN0b3IiLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjoiPHNldHRpbmdzPjxBZGRBbGxJdGVtc1JlY29yZD5GYWxzZTwvQWRkQWxsSXRlbXNSZWNvcmQ+PEFkZE5vbmVSZWNvcmQ+RmFsc2U8L0FkZE5vbmVSZWNvcmQ+PEFkZFNlbGVjdENvdW50cnlSZWNvcmQ+VHJ1ZTwvQWRkU2VsZWN0Q291bnRyeVJlY29yZD48RW5hYmxlU3RhdGVTZWxlY3Rpb24+RmFsc2U8L0VuYWJsZVN0YXRlU2VsZWN0aW9uPjxSZXR1cm5UeXBlPjE8L1JldHVyblR5cGU+PC9zZXR0aW5ncz4iLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_AddressDefaultCountry = "KDA_AddressDefaultCountry";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/K-Source/New-request/Your-message-was-sent")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkJpZCByZXF1ZXN0IHNlbnQgcGFnZSBVUkwiLCJLZXlOYW1lIjoiS0RBX0JpZFJlcXVlc3RTZW50UGFnZVVybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvSy1Tb3VyY2UvTmV3LXJlcXVlc3QvWW91ci1tZXNzYWdlLXdhcy1zZW50IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_BidRequestSentPageUrl = "KDA_BidRequestSentPageUrl";

        /// <summary>
        /// KDA BidServiceUrl
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"https://acdfsip0c6.execute-api.us-east-1.amazonaws.com/Qa/")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBCaWRTZXJ2aWNlVXJsIiwiS2V5TmFtZSI6IktEQV9CaWRTZXJ2aWNlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Imh0dHBzOi8vYWNkZnNpcDBjNi5leGVjdXRlLWFwaS51cy1lYXN0LTEuYW1hem9uYXdzLmNvbS9RYS8iLCJLZXlEZXNjcmlwdGlvbiI6IktEQSBCaWRTZXJ2aWNlVXJsIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_BidServiceUrl = "KDA_BidServiceUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBDaGVja291dCBTaG93UHJvZHVjdGlvbkFuZFNoaXBwaW5nIiwiS2V5TmFtZSI6IktEQV9DaGVja291dF9TaG93UHJvZHVjdGlvbkFuZFNoaXBwaW5nIiwiS2V5VHlwZSI6ImJvb2xlYW4iLCJLZXlEZWZhdWx0VmFsdWUiOiJGYWxzZSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_Checkout_ShowProductionAndShipping = "KDA_Checkout_ShowProductionAndShipping";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Checkout")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9DaGVja291dFBhZ2VVcmwiLCJLZXlOYW1lIjoiS0RBX0NoZWNrb3V0UGFnZVVybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvQ2hlY2tvdXQiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_CheckoutPageUrl = "KDA_CheckoutPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Help/Contact-us")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNvbnRhY3QgVXMgVVJMIiwiS2V5TmFtZSI6IktEQV9Db250YWN0VXNVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiL0hlbHAvQ29udGFjdC11cyIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_ContactUsURL = "KDA_ContactUsURL";

        /// <summary>
        /// Continue Shopping --> go to Products landing page (configurable link)
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/products")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNvbnRpbnVlIFNob3BwaW5nIFVSTCIsIktleU5hbWUiOiJLREFfQ29udGludWVTaG9wcGluZ1VybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvcHJvZHVjdHMiLCJLZXlEZXNjcmlwdGlvbiI6IkNvbnRpbnVlIFNob3BwaW5nIC0tPiBnbyB0byBQcm9kdWN0cyBsYW5kaW5nIHBhZ2UgKGNvbmZpZ3VyYWJsZSBsaW5rKSIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_ContinueShoppingUrl = "KDA_ContinueShoppingUrl";

        /// <summary>
        /// Full customer name. Used for "Company name" in users settings/profile.
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"Nortek")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkN1c3RvbWVyIEZ1bGwgTmFtZSIsIktleU5hbWUiOiJLREFfQ3VzdG9tZXJGdWxsTmFtZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiJOb3J0ZWsiLCJLZXlEZXNjcmlwdGlvbiI6IkZ1bGwgY3VzdG9tZXIgbmFtZS4gVXNlZCBmb3IgXCJDb21wYW55IG5hbWVcIiBpbiB1c2VycyBzZXR0aW5ncy9wcm9maWxlLiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_CustomerFullName = "KDA_CustomerFullName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"John Smith")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkN1c3RvbWVyIGRlZmF1bHQgcGVyc29uYWwgbmFtZSIsIktleU5hbWUiOiJLREFfQ3VzdG9tZXJQZXJzb25hbE5hbWUiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiSm9obiBTbWl0aCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_CustomerPersonalName = "KDA_CustomerPersonalName";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/dashboard")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9FbXB0eUNhcnRfRGFzaGJvYXJkVXJsIiwiS2V5TmFtZSI6IktEQV9FbXB0eUNhcnRfRGFzaGJvYXJkVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9kYXNoYm9hcmQiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_EmptyCart_DashboardUrl = "KDA_EmptyCart_DashboardUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/products")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9FbXB0eUNhcnRfUHJvZHVjdHNVcmwiLCJLZXlOYW1lIjoiS0RBX0VtcHR5Q2FydF9Qcm9kdWN0c1VybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvcHJvZHVjdHMiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_EmptyCart_ProductsUrl = "KDA_EmptyCart_ProductsUrl";

        /// <summary>
        /// KDA FileServiceUrl
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"http://generalfileservice.kadenatest.com")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBGaWxlU2VydmljZVVybCIsIktleU5hbWUiOiJLREFfRmlsZVNlcnZpY2VVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaHR0cDovL2dlbmVyYWxmaWxlc2VydmljZS5rYWRlbmF0ZXN0LmNvbSIsIktleURlc2NyaXB0aW9uIjoiS0RBIEZpbGVTZXJ2aWNlVXJsIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_FileServiceUrl = "KDA_FileServiceUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/special-pages/pdf-not-found")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBIaXJlcyBQZGYgTGluayBGYWlsIiwiS2V5TmFtZSI6IktEQV9IaXJlc1BkZkxpbmtGYWlsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9zcGVjaWFsLXBhZ2VzL3BkZi1ub3QtZm91bmQiLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_HiresPdfLinkFail = "KDA_HiresPdfLinkFail";

        /// <summary>
        /// KDA KitRequestPageUrl
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/products/Product-tools/New-kit-request")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBLaXRSZXF1ZXN0UGFnZVVybCIsIktleU5hbWUiOiJLREFfS2l0UmVxdWVzdFBhZ2VVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiL3Byb2R1Y3RzL1Byb2R1Y3QtdG9vbHMvTmV3LWtpdC1yZXF1ZXN0IiwiS2V5RGVzY3JpcHRpb24iOiJLREEgS2l0UmVxdWVzdFBhZ2VVcmwiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_KitRequestPageUrl = "KDA_KitRequestPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Products/Product-tools/New-kit-request/Your-message-was-sent")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktpdCBSZXF1ZXN0IFNlbnQgUGFnZSBVUkwiLCJLZXlOYW1lIjoiS0RBX0tpdFJlcXVlc3RTZW50UGFnZVVybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvUHJvZHVjdHMvUHJvZHVjdC10b29scy9OZXcta2l0LXJlcXVlc3QvWW91ci1tZXNzYWdlLXdhcy1zZW50IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_KitRequestSentPageUrl = "KDA_KitRequestSentPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/k-list")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IksgTGlzdCBtYWluIHBhZ2UgVVJMIiwiS2V5TmFtZSI6IktEQV9LTGlzdFBhZ2VVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiL2stbGlzdCIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_KListPageURL = "KDA_KListPageURL";

        /// <summary>
        /// KDA MailingServiceUrl
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBNYWlsaW5nU2VydmljZVVybCIsIktleU5hbWUiOiJLREFfTWFpbGluZ1NlcnZpY2VVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaHR0cHM6Ly93ZWpncG5uMDNlLmV4ZWN1dGUtYXBpLnVzLWVhc3QtMS5hbWF6b25hd3MuY29tL1FhIiwiS2V5RGVzY3JpcHRpb24iOiJLREEgTWFpbGluZ1NlcnZpY2VVcmwiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_MailingServiceUrl = "KDA_MailingServiceUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"3")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik1heGltdW0gTm90aWZpY2F0aW9uIEVtYWlscyBPbiBDaGVja291dCIsIktleU5hbWUiOiJLREFfTWF4aW11bU5vdGlmaWNhdGlvbkVtYWlsc09uQ2hlY2tvdXQiLCJLZXlUeXBlIjoiaW50IiwiS2V5RGVmYXVsdFZhbHVlIjoiMyIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_MaximumNotificationEmailsOnCheckout = "KDA_MaximumNotificationEmailsOnCheckout";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Help/Contact-us/Your-message-was-sent")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik1lc3NhZ2Ugc2VudCBwYWdlIFVSTCIsIktleU5hbWUiOiJLREFfTWVzc2FnZVNlbnRQYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9IZWxwL0NvbnRhY3QtdXMvWW91ci1tZXNzYWdlLXdhcy1zZW50IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_MessageSentPageUrl = "KDA_MessageSentPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"True")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBwcm9kdWN0IHJlcXVlc3QgRW5hYmxlZCIsIktleU5hbWUiOiJLREFfTmV3UHJvZHVjdFJlcXVlc3RfRW5hYmxlZCIsIktleVR5cGUiOiJib29sZWFuIiwiS2V5RGVmYXVsdFZhbHVlIjoiVHJ1ZSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_NewProductRequest_Enabled = "KDA_NewProductRequest_Enabled";

        /// <summary>
        /// KDA_ProductRequestPageUrl
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/products/Product-tools/Request-new-product")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBQcm9kdWN0UmVxdWVzdFBhZ2VVcmwiLCJLZXlOYW1lIjoiS0RBX1Byb2R1Y3RSZXF1ZXN0UGFnZVVybCIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvcHJvZHVjdHMvUHJvZHVjdC10b29scy9SZXF1ZXN0LW5ldy1wcm9kdWN0IiwiS2V5RGVzY3JpcHRpb24iOiJLREFfUHJvZHVjdFJlcXVlc3RQYWdlVXJsIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_ProductRequestPageUrl = "KDA_ProductRequestPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Products/Product-tools/Request-new-product/Your-message-was-sent")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByb2R1Y3QgUmVxdWVzdCBTZW50IFBhZ2UgVVJMIiwiS2V5TmFtZSI6IktEQV9Qcm9kdWN0UmVxdWVzdFNlbnRQYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9Qcm9kdWN0cy9Qcm9kdWN0LXRvb2xzL1JlcXVlc3QtbmV3LXByb2R1Y3QvWW91ci1tZXNzYWdlLXdhcy1zZW50IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_ProductRequestSentPageUrl = "KDA_ProductRequestSentPageUrl";

        /// <summary>
        /// Number of days products are flagged as new
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"30")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik5ldyBwcm9kdWN0IHBlcmlvZCIsIktleU5hbWUiOiJLREFfUHJvZHVjdHNfTmV3UHJvZHVjdFBlcmlvZCIsIktleVR5cGUiOiJpbnQiLCJLZXlEZWZhdWx0VmFsdWUiOiIzMCIsIktleURlc2NyaXB0aW9uIjoiTnVtYmVyIG9mIGRheXMgcHJvZHVjdHMgYXJlIGZsYWdnZWQgYXMgbmV3IiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_Products_NewProductPeriod = "KDA_Products_NewProductPeriod";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Products")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByb2R1Y3RzIFBhZ2UgVXJsIiwiS2V5TmFtZSI6IktEQV9Qcm9kdWN0c1BhZ2VVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiL1Byb2R1Y3RzIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_ProductsPageUrl = "KDA_ProductsPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/SERP")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNFUlAgUGFnZSBVcmwiLCJLZXlOYW1lIjoiS0RBX1NlcnBQYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9TRVJQIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_SerpPageUrl = "KDA_SerpPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Settings")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNldHRpbmdzIFBhZ2UgVXJsIiwiS2V5TmFtZSI6IktEQV9TZXR0aW5nc1BhZ2VVcmwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiL1NldHRpbmdzIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_SettingsPageUrl = "KDA_SettingsPageUrl";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"http://default.kadenatest.com/Special-pages/Initial-Password-Setting")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNldCB1cCBwYXNzd29yZCBVUkwiLCJLZXlOYW1lIjoiS0RBX1NldFVwUGFzc3dvcmRVUkwiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiaHR0cDovL2RlZmF1bHQua2FkZW5hdGVzdC5jb20vU3BlY2lhbC1wYWdlcy9Jbml0aWFsLVBhc3N3b3JkLVNldHRpbmciLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_SetUpPasswordURL = "KDA_SetUpPasswordURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"3")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBTaGlwcGluZ0FkZHJlc3NNYXhMaW1pdCIsIktleU5hbWUiOiJLREFfU2hpcHBpbmdBZGRyZXNzTWF4TGltaXQiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiMyIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_ShippingAddressMaxLimit = "KDA_ShippingAddressMaxLimit";

        /// <summary>
        /// KDA Templating_SelectListPageUrl
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/products/Product-tools/Select-a-mailing-list-to-use")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBUZW1wbGF0aW5nX1NlbGVjdExpc3RQYWdlVXJsIiwiS2V5TmFtZSI6IktEQV9UZW1wbGF0aW5nX1NlbGVjdExpc3RQYWdlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9wcm9kdWN0cy9Qcm9kdWN0LXRvb2xzL1NlbGVjdC1hLW1haWxpbmctbGlzdC10by11c2UiLCJLZXlEZXNjcmlwdGlvbiI6IktEQSBUZW1wbGF0aW5nX1NlbGVjdExpc3RQYWdlVXJsIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_Templating_SelectListPageUrl = "KDA_Templating_SelectListPageUrl";

        /// <summary>
        /// KDA Terms And Condition Page
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Special-pages/TermsAndConditions")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBUZXJtcyBBbmQgQ29uZGl0aW9uIFBhZ2UiLCJLZXlOYW1lIjoiS0RBX1Rlcm1zQW5kQ29uZGl0aW9uUGFnZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIvU3BlY2lhbC1wYWdlcy9UZXJtc0FuZENvbmRpdGlvbnMiLCJLZXlEZXNjcmlwdGlvbiI6IktEQSBUZXJtcyBBbmQgQ29uZGl0aW9uIFBhZ2UiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJzZWxlY3RzaW5nbGVwYXRoIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6IjxzZXR0aW5ncz48QWxsb3dTZXRQZXJtaXNzaW9ucz5GYWxzZTwvQWxsb3dTZXRQZXJtaXNzaW9ucz48U2VsZWN0YWJsZVBhZ2VUeXBlcz4wPC9TZWxlY3RhYmxlUGFnZVR5cGVzPjwvc2V0dGluZ3M+IiwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_TermsAndConditionPage = "KDA_TermsAndConditionPage";

        /// <summary>
        /// KDA Terms And Conditions Login
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBUZXJtcyBBbmQgQ29uZGl0aW9ucyBMb2dpbiIsIktleU5hbWUiOiJLREFfVGVybXNBbmRDb25kaXRpb25zTG9naW4iLCJLZXlUeXBlIjoiYm9vbGVhbiIsIktleURlZmF1bHRWYWx1ZSI6IkZhbHNlIiwiS2V5RGVzY3JpcHRpb24iOiJLREEgVGVybXMgQW5kIENvbmRpdGlvbnMgTG9naW4iLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_TermsAndConditionsLogin = "KDA_TermsAndConditionsLogin";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"/Terms-and-Conditions")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlRlcm1zIGFuZCBDb25kaXRpb25zIFVSTCIsIktleU5hbWUiOiJLREFfVGVybXNBbmRDb25kaXRpb25zVVJMIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6Ii9UZXJtcy1hbmQtQ29uZGl0aW9ucyIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoic2VsZWN0c2luZ2xlcGF0aCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PEFsbG93U2V0UGVybWlzc2lvbnM+RmFsc2U8L0FsbG93U2V0UGVybWlzc2lvbnM+PFNlbGVjdGFibGVQYWdlVHlwZXM+MDwvU2VsZWN0YWJsZVBhZ2VUeXBlcz48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_TermsAndConditionsURL = "KDA_TermsAndConditionsURL";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlVzZSBOb3RpZmljYXRpb24gRW1haWxzIE9uIENoZWNrb3V0IiwiS2V5TmFtZSI6IktEQV9Vc2VOb3RpZmljYXRpb25FbWFpbHNPbkNoZWNrb3V0IiwiS2V5VHlwZSI6ImJvb2xlYW4iLCJLZXlEZWZhdWx0VmFsdWUiOiJGYWxzZSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJHZW5lcmFsIFNpdGUgU2V0dGluZ3MifX0=")]
        public const string KDA_UseNotificationEmailsOnCheckout = "KDA_UseNotificationEmailsOnCheckout";

        /// <summary>
        /// CSS fragment with product border style
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"1px solid black")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9Qcm9kdWN0VGh1bWJuYWlsQm9yZGVyU3R5bGUiLCJLZXlOYW1lIjoiS0RBX1Byb2R1Y3RUaHVtYm5haWxCb3JkZXJTdHlsZSIsIktleVR5cGUiOiJzdHJpbmciLCJLZXlEZWZhdWx0VmFsdWUiOiIxcHggc29saWQgYmxhY2siLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_ProductThumbnailBorderStyle = "KDA_ProductThumbnailBorderStyle";

        /// <summary>
        /// Enables product thumbnail borders on site level
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQV9Qcm9kdWN0VGh1bWJuYWlsQm9yZGVyRW5hYmxlZCIsIktleU5hbWUiOiJLREFfUHJvZHVjdFRodW1ibmFpbEJvcmRlckVuYWJsZWQiLCJLZXlUeXBlIjoiYm9vbGVhbiIsIktleURlZmF1bHRWYWx1ZSI6IkZhbHNlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkdlbmVyYWwgU2l0ZSBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_ProductThumbnailBorderEnabled = "KDA_ProductThumbnailBorderEnabled";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Kadena", "Kadena", "CMS.Settings")]
        [GroupAttribute("General Site Settings")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlNvbGQgVG8gR2VuZXJhbCBJbnZlbnRvcnkiLCJLZXlOYW1lIjoiS0RBX1NvbGRUb0dlbmVyYWxJbnZlbnRvcnkiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCBTaXRlIFNldHRpbmdzIn19")]
        public const string KDA_SoldToGeneralInventory = "KDA_SoldToGeneralInventory";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("E-Commerce", "ECommerceSettings", "Kadena")]
        [GroupAttribute("General")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkRlZmF1bHQgQXBwcm92ZXIiLCJLZXlOYW1lIjoiS0RBX0RlZmF1bHRBcHByb3ZlciIsIktleVR5cGUiOiJpbnQiLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjoifi9jbXNmb3JtY29udHJvbHMva2FkZW5hL29yZGVyYXBwcm92ZXJzZWxlY3Rvci5hc2N4IiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJFLUNvbW1lcmNlIiwiTmFtZSI6IkVDb21tZXJjZVNldHRpbmdzIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiS2FkZW5hIn0sIkRpc3BsYXlOYW1lIjoiR2VuZXJhbCJ9fQ==")]
        public const string KDA_DefaultApprover = "KDA_DefaultApprover";

    }
}
