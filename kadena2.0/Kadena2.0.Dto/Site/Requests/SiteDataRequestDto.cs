using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Site.Requests
{
    public class SiteDataRequestDto
    {
        [Required]
        public int SiteId { get; set; }

        public string SiteName { get; set; }
    }
}