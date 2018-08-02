﻿using AutoMapper;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.CampaignData;
using Kadena.Models.RecentOrders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoCampaignsProvider : IKenticoCampaignsProvider
    {
        private readonly string PageTypeClassName = "KDA.Campaign";
        private readonly string orderTypePreBuy = "prebuy";
        private readonly IMapper mapper;

        public KenticoCampaignsProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void DeleteCampaign(int campaignID)
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode campaign = tree.SelectNodes(PageTypeClassName)
                                    .Where("CampaignID", QueryOperator.Equals, campaignID)
                                    .OnCurrentSite();
            if (campaign != null)
            {
                campaign.Delete();
            }
        }

        public TreeNode GetCampaign(int campaignID)
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode campaign = tree.SelectNodes(PageTypeClassName)
                                    .Where("CampaignID", QueryOperator.Equals, campaignID)
                                    .OnCurrentSite();
            return campaign;
        }

        public OrderCampaginHead GetCampaigns(string orderType)
        {
            List<OrderCampaginItem> campaigns = new List<OrderCampaginItem>();
            if (orderType.Equals(orderTypePreBuy))
            {
                var campaignDocuments = DocumentHelper.GetDocuments(PageTypeClassName).OnSite(SiteContext.CurrentSiteID).WhereTrue("OpenCampaign");
                if (!DataHelper.DataSourceIsEmpty(campaignDocuments))
                {
                    foreach (TreeNode item in campaignDocuments.TypedResult.Items)
                    {
                        campaigns.Add(new OrderCampaginItem()
                        {
                            id = item.GetIntegerValue("CampaignID", 0),
                            name = item.GetStringValue("Name", string.Empty)
                        });
                    }
                }
            }
            return new OrderCampaginHead()
            {
                placeholder = ResHelper.GetString("Kadena.RecentOrders.Filter.SelectCampaign"),
                items = campaigns
            };
        }

        public bool CloseCampaignIBTF(int campaignID)
        {
            var selectedCampaign = DocumentHelper.GetDocuments(PageTypeClassName).OnSite(SiteContext.CurrentSiteID).WhereEquals("CampaignID", campaignID).FirstOrDefault();
            if (selectedCampaign != null)
            {
                selectedCampaign.SetValue("IBTFFinalized", true);
                selectedCampaign.Update();
                return true;
            }
            return false;
        }
        public int GetOpenCampaignID()
        {
            var openCampaign = DocumentHelper.GetDocuments(PageTypeClassName)
                .Columns("CampaignID,Name,StartDate,EndDate")
                .OnSite(SiteContext.CurrentSiteID)
                .WhereEquals("OpenCampaign", true)
                .Where(new WhereCondition().WhereEquals("CloseCampaign", false).Or().WhereEquals("CloseCampaign", null))
                .FirstOrDefault();
            return openCampaign != null
                ? openCampaign.GetIntegerValue("CampaignID", default(int))
                : default(int);
        }
        public string GetCampaignFiscalYear(int campaignID)
        {
            var campaign = DocumentHelper.GetDocuments(PageTypeClassName).OnSite(SiteContext.CurrentSiteID).WhereEquals("CampaignID", campaignID).FirstOrDefault();
            return campaign != null ? campaign.GetValue("FiscalYear", string.Empty) : null;
        }

        public CampaignData GetOpenCampaign()
        {
            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            var document = tree
                .SelectNodes(PageTypeClassName)
                .OnCurrentSite()
                .Culture(LocalizationContext.CurrentCulture.CultureCode)
                .WhereTrue("OpenCampaign")
                .WhereEqualsOrNull("CloseCampaign", false)
                .Columns("CampaignID, Name, StartDate, EndDate")
                .FirstOrDefault();

            return mapper.Map<CampaignData>(document);
        }
    }
}
