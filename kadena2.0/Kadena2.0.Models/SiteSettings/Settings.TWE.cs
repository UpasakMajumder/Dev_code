using Kadena.Models.SiteSettings.Attributes;

namespace Kadena.Models.SiteSettings
{
    public partial class Settings
    {
        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByb2R1Y3RzIFBERkhlYWRlciIsIktleU5hbWUiOiJQcm9kdWN0c1BERkhlYWRlciIsIktleVR5cGUiOiJsb25ndGV4dCIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkN1c3RvbSBDYXRhbG9nIFBERiBTZXR0aW5ncyJ9fQ==")]
        public const string ProductsPDFHeader = "ProductsPDFHeader";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(@" <p style=""font-size: 22px; color: #b5161a; padding: 0 180px;"">PROGRAMFOOTERTEXT</p>")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBQcm9ncmFtIEZvb3RlciBUZXh0IiwiS2V5TmFtZSI6IktEQV9Qcm9ncmFtRm9vdGVyVGV4dCIsIktleVR5cGUiOiJsb25ndGV4dCIsIktleURlZmF1bHRWYWx1ZSI6IiA8cCBzdHlsZT1cImZvbnQtc2l6ZTogMjJweDsgY29sb3I6ICNiNTE2MWE7IHBhZGRpbmc6IDAgMTgwcHg7XCI+UFJPR1JBTUZPT1RFUlRFWFQ8L3A+IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOiJMYXJnZVRleHRBcmVhIiwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ3VzdG9tIENhdGFsb2cgUERGIFNldHRpbmdzIn19")]
        public const string KDA_ProgramFooterText = "KDA_ProgramFooterText";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(@"<meta charset=""utf-8"" /><meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"" />
<title></title>
<div style=""margin: 60px 0px; width: 1000px;"">
<div style=""text-align: center; margin-bottom: 220px;"">
<div style=""text-align: center;""><a href=""#""><img src=""https://twe.kadenatest.com/getmedia/bf90ae55-db34-4246-adf1-777e99efe5e5/2line-brandmark-vert-color?width=250&amp;height=192"" /></a><br />
&nbsp;</div>

<h2 style=""color: rgb(181, 22, 26); text-transform: uppercase; margin-bottom: 40px;"">General Inventory 2018 Catalog</h2>
</div>
</div>
")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktEQSBHZW5lcmFsIEludmVudG9yeSBDb3ZlciIsIktleU5hbWUiOiJLREFfR2VuZXJhbEludmVudG9yeUNvdmVyIiwiS2V5VHlwZSI6Imxvbmd0ZXh0IiwiS2V5RGVmYXVsdFZhbHVlIjoiPG1ldGEgY2hhcnNldD1cInV0Zi04XCIgLz48bWV0YSBuYW1lPVwidmlld3BvcnRcIiBjb250ZW50PVwid2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aWFsLXNjYWxlPTEsIHNocmluay10by1maXQ9bm9cIiAvPlxyXG48dGl0bGU+PC90aXRsZT5cclxuPGRpdiBzdHlsZT1cIm1hcmdpbjogNjBweCAwcHg7IHdpZHRoOiAxMDAwcHg7XCI+XHJcbjxkaXYgc3R5bGU9XCJ0ZXh0LWFsaWduOiBjZW50ZXI7IG1hcmdpbi1ib3R0b206IDIyMHB4O1wiPlxyXG48ZGl2IHN0eWxlPVwidGV4dC1hbGlnbjogY2VudGVyO1wiPjxhIGhyZWY9XCIjXCI+PGltZyBzcmM9XCJodHRwczovL3R3ZS5rYWRlbmF0ZXN0LmNvbS9nZXRtZWRpYS9iZjkwYWU1NS1kYjM0LTQyNDYtYWRmMS03NzdlOTllZmU1ZTUvMmxpbmUtYnJhbmRtYXJrLXZlcnQtY29sb3I/d2lkdGg9MjUwJmFtcDtoZWlnaHQ9MTkyXCIgLz48L2E+PGJyIC8+XHJcbiZuYnNwOzwvZGl2PlxyXG5cclxuPGgyIHN0eWxlPVwiY29sb3I6IHJnYigxODEsIDIyLCAyNik7IHRleHQtdHJhbnNmb3JtOiB1cHBlcmNhc2U7IG1hcmdpbi1ib3R0b206IDQwcHg7XCI+R2VuZXJhbCBJbnZlbnRvcnkgMjAxOCBDYXRhbG9nPC9oMj5cclxuPC9kaXY+XHJcbjwvZGl2PlxyXG4iLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6Ikh0bWxBcmVhQ29udHJvbCIsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOiI8c2V0dGluZ3M+PERpYWxvZ3NfQ29udGVudF9IaWRlPkZhbHNlPC9EaWFsb2dzX0NvbnRlbnRfSGlkZT48SGVpZ2h0VW5pdFR5cGU+UFg8L0hlaWdodFVuaXRUeXBlPjxNZWRpYURpYWxvZ0NvbmZpZ3VyYXRpb24+VHJ1ZTwvTWVkaWFEaWFsb2dDb25maWd1cmF0aW9uPjxTaG93QWRkU3RhbXBCdXR0b24+RmFsc2U8L1Nob3dBZGRTdGFtcEJ1dHRvbj48V2lkdGhVbml0VHlwZT5QWDwvV2lkdGhVbml0VHlwZT48L3NldHRpbmdzPiIsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkN1c3RvbSBDYXRhbG9nIFBERiBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_GeneralInventoryCover = "KDA_GeneralInventoryCover";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlBERkJyYW5kIiwiS2V5TmFtZSI6IlBERkJyYW5kIiwiS2V5VHlwZSI6Imxvbmd0ZXh0IiwiS2V5RGVmYXVsdFZhbHVlIjpudWxsLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ3VzdG9tIENhdGFsb2cgUERGIFNldHRpbmdzIn19")]
        public const string PDFBrand = "PDFBrand";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlByb2dyYW1zQ29udGVudCIsIktleU5hbWUiOiJQcm9ncmFtc0NvbnRlbnQiLCJLZXlUeXBlIjoibG9uZ3RleHQiLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDdXN0b20gQ2F0YWxvZyBQREYgU2V0dGluZ3MifX0=")]
        public const string ProgramsContent = "ProgramsContent";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlBERklubmVySFRNTCIsIktleU5hbWUiOiJQREZJbm5lckhUTUwiLCJLZXlUeXBlIjoibG9uZ3RleHQiLCJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJDdXN0b20gQ2F0YWxvZyBQREYgU2V0dGluZ3MifX0=")]
        public const string PDFInnerHTML = "PDFInnerHTML";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IlBkZkVuZGluZ1RhZ3MiLCJLZXlOYW1lIjoiUGRmRW5kaW5nVGFncyIsIktleVR5cGUiOiJsb25ndGV4dCIsIktleURlZmF1bHRWYWx1ZSI6bnVsbCwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6IkN1c3RvbSBDYXRhbG9nIFBERiBTZXR0aW5ncyJ9fQ==")]
        public const string PdfEndingTags = "PdfEndingTags";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("NReco Settings")]
        [DefaultValueAttribute(@"PDF_Generator_Src_Examples_Pack_206443366898")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6Ik93bmVyIiwiS2V5TmFtZSI6IktEQV9OUmVjb093bmVyIiwiS2V5VHlwZSI6InN0cmluZyIsIktleURlZmF1bHRWYWx1ZSI6IlBERl9HZW5lcmF0b3JfU3JjX0V4YW1wbGVzX1BhY2tfMjA2NDQzMzY2ODk4IiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik5SZWNvIFNldHRpbmdzIn19")]
        public const string KDA_NRecoOwner = "KDA_NRecoOwner";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("NReco Settings")]
        [DefaultValueAttribute(@"False")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkxvdyBRdWFsaXR5IiwiS2V5TmFtZSI6IktEQV9OUmVjb0xvd1F1YWxpdHkiLCJLZXlUeXBlIjoiYm9vbGVhbiIsIktleURlZmF1bHRWYWx1ZSI6IkZhbHNlIiwiS2V5RGVzY3JpcHRpb24iOiIiLCJLZXlFZGl0aW5nQ29udHJvbFBhdGgiOm51bGwsIktleUV4cGxhbmF0aW9uVGV4dCI6IiIsIktleUZvcm1Db250cm9sU2V0dGluZ3MiOm51bGwsIktleVZhbGlkYXRpb24iOm51bGwsIkdyb3VwIjp7IkNhdGVnb3J5Ijp7IkRpc3BsYXlOYW1lIjoiS2FkZW5hIiwiTmFtZSI6IkthZGVuYSIsIkNhdGVnb3J5UGFyZW50TmFtZSI6IkNNUy5TZXR0aW5ncyJ9LCJEaXNwbGF5TmFtZSI6Ik5SZWNvIFNldHRpbmdzIn19")]
        public const string KDA_NRecoLowQuality = "KDA_NRecoLowQuality";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("NReco Settings")]
        [DefaultValueAttribute(@"ZY6RpF4uqHLQfQEcY8SetuBverdBLHGGFabv2xJL41o2d0XsORPQboV5rgl0fzMcdUnu5uH7cpCs3ThxZ8fdfsrYLQ5+Zq055UG3tbpQVDKSgnOf1QLDIoiddcbTKiqoe8VDIlrRMv69fyZXlt/T78hnVUDr/jQXfLarAu3iEHY=")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IktleSIsIktleU5hbWUiOiJLREFfTlJlY29LZXkiLCJLZXlUeXBlIjoic3RyaW5nIiwiS2V5RGVmYXVsdFZhbHVlIjoiWlk2UnBGNHVxSExRZlFFY1k4U2V0dUJ2ZXJkQkxIR0dGYWJ2MnhKTDQxbzJkMFhzT1JQUWJvVjVyZ2wwZnpNY2RVbnU1dUg3Y3BDczNUaHhaOGZkZnNyWUxRNStacTA1NVVHM3RicFFWREtTZ25PZjFRTERJb2lkZGNiVEtpcW9lOFZESWxyUk12NjlmeVpYbHQvVDc4aG5WVURyL2pRWGZMYXJBdTNpRUhZPSIsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlWYWxpZGF0aW9uIjpudWxsLCJHcm91cCI6eyJDYXRlZ29yeSI6eyJEaXNwbGF5TmFtZSI6IkthZGVuYSIsIk5hbWUiOiJLYWRlbmEiLCJDYXRlZ29yeVBhcmVudE5hbWUiOiJDTVMuU2V0dGluZ3MifSwiRGlzcGxheU5hbWUiOiJOUmVjbyBTZXR0aW5ncyJ9fQ==")]
        public const string KDA_NRecoKey = "KDA_NRecoKey";

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("TWE", "KDATWESettingCategory", "Kadena")]
        [GroupAttribute("Custom Catalog PDF Settings")]
        [DefaultValueAttribute(@"</div>")]
        [EncodedDefinitionAttribute("eyJLZXlEaXNwbGF5TmFtZSI6IkNsb3NpbmcgRElWIiwiS2V5TmFtZSI6IkNsb3NpbmdESVYiLCJLZXlUeXBlIjoibG9uZ3RleHQiLCJLZXlEZWZhdWx0VmFsdWUiOiI8L2Rpdj4iLCJLZXlEZXNjcmlwdGlvbiI6IiIsIktleUVkaXRpbmdDb250cm9sUGF0aCI6bnVsbCwiS2V5RXhwbGFuYXRpb25UZXh0IjoiIiwiS2V5Rm9ybUNvbnRyb2xTZXR0aW5ncyI6bnVsbCwiS2V5VmFsaWRhdGlvbiI6bnVsbCwiR3JvdXAiOnsiQ2F0ZWdvcnkiOnsiRGlzcGxheU5hbWUiOiJLYWRlbmEiLCJOYW1lIjoiS2FkZW5hIiwiQ2F0ZWdvcnlQYXJlbnROYW1lIjoiQ01TLlNldHRpbmdzIn0sIkRpc3BsYXlOYW1lIjoiQ3VzdG9tIENhdGFsb2cgUERGIFNldHRpbmdzIn19")]
        public const string ClosingDIV = "ClosingDIV";
    }
}
