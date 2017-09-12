using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Site.Requests
{
    public class SiteDataRequestDto
    {
        [Required]
        public string SiteName { get; set; }
    }
}