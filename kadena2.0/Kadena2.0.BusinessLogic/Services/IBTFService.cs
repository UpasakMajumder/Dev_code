﻿using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using Kadena.Models.IBTF;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using System.Linq;
using Kadena.Models.Product;

namespace Kadena.BusinessLogic.Services
{
    public class IBTFService : IIBTFService
    {
        private readonly IKenticoIBTFProvider kenticoIBTF;
        private readonly IKenticoProductsProvider kenticoProductsProvider;
        private readonly IKenticoCampaignsProvider kenticoCampaignsProvider;
        private readonly IKenticoProgramsProvider programsProvider;
        private readonly IKenticoUserBudgetProvider kenticoUserBudgetProvider;

        public IBTFService(IKenticoIBTFProvider kenticoIBTF, IKenticoProductsProvider kenticoProductsProvider, IKenticoCampaignsProvider kenticoCampaignsProvider, 
            IKenticoUserBudgetProvider kenticoUserBudgetProvider, IKenticoProgramsProvider programsProvider)
        {
            this.kenticoIBTF = kenticoIBTF ?? throw new ArgumentNullException(nameof(kenticoIBTF));
            this.kenticoProductsProvider = kenticoProductsProvider ?? throw new ArgumentNullException(nameof(kenticoProductsProvider));
            this.kenticoCampaignsProvider = kenticoCampaignsProvider ?? throw new ArgumentNullException(nameof(kenticoCampaignsProvider));
            this.kenticoUserBudgetProvider = kenticoUserBudgetProvider ?? throw new ArgumentNullException(nameof(kenticoUserBudgetProvider));
            this.programsProvider = programsProvider ?? throw new ArgumentNullException(nameof(programsProvider));
        }

        public void InsertIBTFAdjustmentRecord(OrderDTO order)
        {
            order.Items.ToList().ForEach(o =>
            {
                kenticoIBTF.InsertIBTFAdjustmentRecord(new IBTFAdjustment()
                {
                    SKUID = o.SKU.KenticoSKUID,
                    UserID = order.Customer.KenticoUserID,
                    CampaignID = order.Campaign != null ? order.Campaign.ID : 0,
                    OrderedQuantity = o.UnitCount,
                    OrderedProductPrice = o.TotalPrice
                });
            });
        }

        public void UpdateRemainingBudget(int campaignID)
        {
            var programs = programsProvider.GetProgramIDsByCampaign(campaignID);
            List<CampaignsProduct> products = kenticoProductsProvider.GetCampaignsProductSKUIDs(programs);
            List<int> SKUIDs = products.Select(x => x.SKUID).ToList();
            List<IBTF> IBTFRecords = kenticoIBTF.GetIBTFRecords().Where(x => SKUIDs.Contains(x.SKUID))?.ToList();
            List<IBTFAdjustment> IBTFAdjustmentRecords = kenticoIBTF.GetIBTFAdjustmentRecords().Where(x => SKUIDs.Contains(x.SKUID) && x.CampaignID.Equals(campaignID))?.ToList();
            string campaignFiscalYear = kenticoCampaignsProvider.GetCampaignFiscalYear(campaignID);
            if (IBTFRecords != null && IBTFRecords.Count > 0 && IBTFAdjustmentRecords != null && IBTFAdjustmentRecords.Count > 0)
            {
                IBTFRecords.ForEach(i =>
                {
                    IBTFAdjustmentRecords.Where(a => a.SKUID.Equals(i.SKUID))?.ToList().ForEach(a =>
                    {
                        decimal adjustment = a.OrderedProductPrice - (a.OrderedQuantity * i.ActualPrice);
                        kenticoUserBudgetProvider.AdjustUserRemainingBudget(campaignFiscalYear, a.UserID, adjustment);
                    });
                });
            }
        }
    }
}
