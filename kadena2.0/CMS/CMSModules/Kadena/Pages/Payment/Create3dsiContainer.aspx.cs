using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;
using CMS.UIControls;
using Kadena.Dto.Payment.CreditCard.MicroserviceRequests;
using Kadena.Helpers;
using Kadena.WebAPI.KenticoProviders;
using Kadena2.MicroserviceClients.Clients;
using System;

namespace Kadena.CMSModules.Kadena.Pages.Payment
{
    public partial class Create3dsiContainer : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.tbCode.Text = GetSettingsKey("KDA_CreditCard_Code");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var requestData = CreateRequestData();
            var client = new CreditCardManagerClient(new MicroProperties(new KenticoResourceService()));
            var result = client.CreateCustomerContainer(requestData).Result;

            if (result == null || !result.Success)
            {
                var message = string.IsNullOrEmpty(result?.ErrorMessages) ? "Error calling Credit card manager microservice" : result.ErrorMessages;
                EventLogProvider.LogInformation("Create 3DSi Container", "ERROR", eventDescription: message);
                LocalizedLabelResult.Text = $"Error - {message}";
            }
            else
            {
                EventLogProvider.LogInformation("Create 3DSi Container", "Success", "Credit card manager microservice call to create container was successful");
                LocalizedLabelResult.Text = "Succesfully submitted";
            }
        }

        private CreateCustomerContainerRequestDto CreateRequestData()
        {
            var merchantCode = GetSettingsKey("KDA_CreditCard_MerchantCode");
            var locationCode = GetSettingsKey("KDA_CreditCard_LocationCode");

            return new CreateCustomerContainerRequestDto()
            {
                Code = GetSettingsKey("KDA_CreditCard_Code"),
                Name = GetSettingsKey("KDA_CustomerFullName"),
                Url = SiteContext.CurrentSite.DomainName,
                BillingAddress = new AddressDto()
                {
                    AddressLine1 = GetSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                    AddressLine2 = GetSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine2"),
                    City = GetSettingsKey("KDA_EstimateDeliveryPrice_SenderCity"),
                    PostalCode = GetSettingsKey("KDA_EstimateDeliveryPrice_SenderPostal")
                },
                AdditionalData = "{\"MerchantCode\":\""+merchantCode+"\",\"LocationCode\":\""+locationCode+"\"}"
            };
        }

        private string GetSettingsKey(string key)
        {
            var site = new SiteInfoIdentifier(SiteContext.CurrentSiteName);
            return SettingsKeyInfoProvider.GetValue(key, site);
        }
    }
}