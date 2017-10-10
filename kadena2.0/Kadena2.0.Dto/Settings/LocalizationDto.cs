using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Settings
{
    public class LocalizationDto
    {
        [Required]
        public string Language { get; set; }
    }
}
