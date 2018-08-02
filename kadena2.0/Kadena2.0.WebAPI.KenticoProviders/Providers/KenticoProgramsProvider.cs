using AutoMapper;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Localization;
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
        private readonly IMapper mapper;

        public KenticoProgramsProvider(IMapper mapper, IKenticoCampaignsProvider campaignsProvider)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
                                    .WhereEquals("ProgramID", programID)
                                    .OnCurrentSite();
            return mapper.Map<CampaignProgram>(program);
        }

        public IEnumerable<CampaignProgram> GetProgramsForCampaign(int campaignId)
        {
            var programs = new TreeProvider(MembershipContext.AuthenticatedUser)
                .SelectNodes(PageTypeClassName)
                .WhereEquals("CampaignID", campaignId)
                .OnCurrentSite()
                .ToList();

            return mapper.Map<IEnumerable<CampaignProgram>>(programs);
        }
    }
}
