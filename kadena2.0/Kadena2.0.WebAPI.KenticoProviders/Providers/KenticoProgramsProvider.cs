using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using Kadena.Models.Program;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProgramsProvider : IKenticoProgramsProvider
    {
        private readonly string PageTypeClassName = "KDA.Program";
        private readonly IKenticoCampaignsProvider campaignsProvider;

        public KenticoProgramsProvider(IKenticoCampaignsProvider campaignsProvider)
        {
            this.campaignsProvider = campaignsProvider ?? throw new ArgumentNullException(nameof(campaignsProvider));
        }

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
            TreeNode campaign = campaignsProvider.GetCampaign(campaignID);
            if (campaign != null)
            {
                var programNodes = new TreeProvider(MembershipContext.AuthenticatedUser).SelectNodes(PageTypeClassName)
                                    .Where("CampaignID", QueryOperator.Equals, campaignID)
                                    .OnCurrentSite();
                if (programNodes != null && programNodes.HasResults() && programNodes.TypedResult.Items.Count > 0)
                {
                    programIDs = programNodes.TypedResult.Items.ToList().Select(x => x.GetIntegerValue("ProgramID", default(int))).ToList();
                }
            }
            return programIDs;
        }

        public CampaignProgram GetProgram(int programID)
        {
            TreeNode program = new TreeProvider(MembershipContext.AuthenticatedUser).SelectNodes(PageTypeClassName)
                                    .Where("ProgramID", QueryOperator.Equals, programID)
                                    .OnCurrentSite();
            if (program != null)
            {
                return new CampaignProgram()
                {
                    ProgramID = program.GetIntegerValue("ProgramID", default(int)),
                    ProgramName = program.DocumentName,
                    BrandID = program.GetIntegerValue("BrandID", default(int)),
                    CampaignID = program.GetIntegerValue("CampaignID", default(int)),
                    GlobalAdminNotified= program.GetBooleanValue("GlobalAdminNotified", false)
                };
            }
            else
            {
                return null;
            }
        }
    }
}
