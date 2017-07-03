using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Site
{
    public class SiteDataRequestDto
    {
        [Required]
        public string SiteName { get; set; }
    }
}