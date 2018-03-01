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

        public void UpdateSubmissionId(Guid oldId, Guid newId)
        {
            var submissionItem = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SubmissionId", oldId)
                .FirstOrDefault();

            if (submissionItem == null)
            {
                throw new ArgumentOutOfRangeException(nameof(oldId), "Couldn't find existing submission to update");
            }

            submissionItem.SetValue("SubmissionId", newId);

            submissionItem.Update();
        }

        public void SaveSubmission(Submission submission)
        {
            if (submission == null || submission.SubmissionId == Guid.Empty)
            {
                return;
            }

            var originalSubmissionItem = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SubmissionId", submission.SubmissionId)
                .FirstOrDefault();

            var submissionItem = originalSubmissionItem;

            if (submissionItem == null)
            {
                submissionItem = CustomTableItem.New(SubmissionsTable);
                submissionItem.SetValue("SubmissionId", submission.SubmissionId);                
            }

            submissionItem.SetValue("SiteId", submission.SiteId);
            submissionItem.SetValue("UserId", submission.UserId);
            submissionItem.SetValue("CustomerId", submission.CustomerId);
            submissionItem.SetValue("AlreadyVerified", submission.AlreadyVerified);
            submissionItem.SetValue("Processed", submission.Processed);
            submissionItem.SetValue("RedirectUrl", submission.RedirectUrl);
            submissionItem.SetValue("OrderJson", submission.OrderJson);
            submissionItem.SetValue("SaveCardJson", submission.SaveCardJson);
            submissionItem.SetValue("Success", submission.Success);
            submissionItem.SetValue("Error", submission.Error);

            if (originalSubmissionItem == null)
            {
                submissionItem.Insert();
            }
            else
            {
                submissionItem.Update();
            }
        }

        public Submission GetSubmission(Guid submissionId)
        {
            var submission = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SubmissionId", submissionId)
                .SingleOrDefault();

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
