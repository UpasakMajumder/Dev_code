using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.CustomTables;
using System.Linq;
using Kadena.Models.CreditCard;

namespace Kadena.WebAPI.KenticoProviders
{
    public class SubmissionIdProvider : ISubmissionIdProvider
    {
        private readonly string SubmissionsTable = "KDA.Neco";

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

            submissionItem.SetValue("UserId", submission.UserId);
            submissionItem.SetValue("AlreadyUsed", submission.AlreadyUsed);
            submissionItem.Insert();
        }

        public Submission GetSubmission(Guid submissionId)
        {
            var submission = CustomTableItemProvider.GetItems(SubmissionsTable)
                .WhereEquals("SubmissionId", submissionId)
                .FirstOrDefault();

            if (submission == null)
                return null;

            return new Submission()
            {
                SubmissionId = submission.GetGuidValue("SubmissionId", Guid.Empty),
                AlreadyUsed = submission.GetBooleanValue("AlreadyUsed", true),
                UserId = submission.GetIntegerValue("UserId", 0)
            };            
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
    }
}
