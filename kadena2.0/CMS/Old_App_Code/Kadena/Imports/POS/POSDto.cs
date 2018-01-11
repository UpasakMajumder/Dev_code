using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.POS
{
    public class POSDto
    {
        [Header(0, "Brand *")]
        [Required]
        public string Brand { get; set; }

        [Header(1, "Year *")]
        [Required]
        [MaxLength(4)]
        public string Year { get; set; }

        [Header(2, "POS Code *")]
        [Required]
        public string POSCode { get; set; }

        [Header(3, "POS Category *")]
        [Required]
        public string POSCategory { get; set; }
    }
}