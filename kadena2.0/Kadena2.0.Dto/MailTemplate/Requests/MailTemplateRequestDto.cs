﻿using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.MailTemplate.Requests
{
    public class MailTemplateRequestDto
    {
        [Required(ErrorMessage = "TemplateName is mandatory parameter")]
        public string TemplateName { get; set; }

        /// <summary>
        /// Not used yet. Use if you need to distinguish between sites
        /// </summary>
        public int SiteId { get; set; }
    }
}