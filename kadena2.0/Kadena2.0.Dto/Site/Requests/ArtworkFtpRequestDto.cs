using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Site.Requests
{
    public class ArtworkFtpRequestDto
    {
        [Required]
        public int SiteId { get; set; }
    }
}