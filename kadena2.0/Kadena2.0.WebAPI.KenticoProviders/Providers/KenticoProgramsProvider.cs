using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProgramsProvider : IKenticoProgramsProvider
    {
        private readonly string PageTypeClassName = "KDA.Program";

        public void DeleteProgram(int programID)
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode program = tree.SelectNodes(PageTypeClassName)
                                    .Where("ProgramID", QueryOperator.Equals, programID)
                                    .OnCurrentSite();
            if (program != null)
            {
                program.Delete();
            }
        }

        public List<int> GetProgramIDsByCampaign(int campaignID)
        {
            List<int> programIDs = new List<int>();
            TreeNode campaign = new KenticoCampaignsProvider().GetCampaign(campaignID);
            if (campaign != null)
            {
                var programNodes = new TreeProvider(MembershipContext.AuthenticatedUser).SelectNodes(PageTypeClassName)
                                    .Where("CampaignID", QueryOperator.Equals, campaignID)
                                    .Column("ProgramID")
                                    .OnCurrentSite();
                if (programNodes != null && programNodes.HasResults() && programNodes.TypedResult.Items.Count > 0)
                {
                    programIDs = programNodes.TypedResult.Items.ToList().Select(x => x.GetIntegerValue("ProgramID", default(int))).ToList();
                }
            }
            return programIDs;
        }
    }
}
