using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Site.Responses
{
    public class ArtworkFtpResponseDto
    {
        [Required]
        public bool Enabled { get; set; }

        [Required]
        FtpCredentialsDto Ftp { get; set; }
    }
}