using System;

namespace Kadena.Models.CreditCard
{
    public class Submission
    {
        public Guid SubmissionId { get; set; }

        /// <summary>
        /// Indicates if this submission was already used for verifying.
        /// Default is set to true to ensure proper intended initialization in the code
        /// </summary>
        public bool AlreadyUsed { get; set; } = true;

        public int SiteId { get; set; }

        /// <summary>
        /// ID of user who requested to create this submission
        /// </summary>
        public int UserId { get; set; }

        public int CustomerId { get; set; }

        public bool TokenSavedAndAuthorized { get; set; }
    }
}
