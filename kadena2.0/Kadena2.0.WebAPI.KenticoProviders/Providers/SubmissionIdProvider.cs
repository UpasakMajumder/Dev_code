using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.CustomTables;
using System.Linq;
using Kadena.Models.CreditCard;
using System.Collections.Generic;
using AutoMapper;

namespace Kadena.WebAPI.KenticoProviders
{
    public class SubmissionIdProvider : ISubmissionIdProvider
    {
        private readonly string SubmissionsTable = "KDA.Submissions";

        private readonly IMapper mapper;

        public SubmissionIdProvider(IMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
        }

        public void SaveSubmission(Submission submission)
        {
            if (submission == null || submission.SubmissionId == Guid.Empty)
            {
                return;
            }

            var submissionItem = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SubmissionId", submission.SubmissionId)
                .FirstOrDefault();

            if (submissionItem == null)
            {
                submissionItem = CustomTableItem.New(SubmissionsTable);
                submissionItem.SetValue("SubmissionId", submission.SubmissionId);
                
            }

            submissionItem.SetValue("SiteId", submission.SiteId);
            submissionItem.SetValue("UserId", submission.UserId);
            submissionItem.SetValue("CustomerId", submission.CustomerId);
            submissionItem.SetValue("AlreadyUsed", submission.AlreadyUsed);
            submissionItem.SetValue("TokenSavedAndAuthorized", submission.TokenSavedAndAuthorized);
            submissionItem.Insert();
        }

        public Submission GetSubmission(Guid submissionId)
        {
            var submission = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SubmissionId", submissionId)
                .FirstOrDefault();

            return mapper.Map<Submission>(submission);
        }

        public void DeleteSubmission(Guid submissionId)
        {
            var submission = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SubmissionId", submissionId)
                .FirstOrDefault();

            if (submission != null)
            {
                CustomTableItemProvider.DeleteItem(submission);
            }
        }

        public IEnumerable<Submission> GetSubmissions(int siteId, int userId, int customerId)
        {
            var submissions = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SiteId", siteId)
                .WhereEquals("UserId", userId)
                .WhereEquals("CustomerId", customerId)
                .ToArray();

            return submissions.Select(s => mapper.Map<Submission>(s));
        }
    }
}
