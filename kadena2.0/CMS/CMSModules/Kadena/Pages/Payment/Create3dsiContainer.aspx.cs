using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;
using CMS.UIControls;
using Kadena.Dto.General;
using Kadena.Dto.Payment.CreditCard.MicroserviceRequests;
using Kadena2.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using Newtonsoft.Json;
using System;

namespace Kadena.CMSModules.Kadena.Pages.Payment
{
    public partial class Create3dsiContainer : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ltlCode.Text = GetSettingsKey("KDA_CreditCard_Code");
            this.ltlCreatePayload.Text = JsonConvert.SerializeObject(CreateRequestData());
            this.ltlUpdatePayload.Text = JsonConvert.SerializeObject(CreateRequestData());
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            var requestData = CreateRequestData();
            var client = DIContainer.Resolve<ICreditCardManagerClient>();
            var result = client.CreateCustomerContainer(requestData).Result;

            DisplayResult(result, "Create");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var requestData = UpdateRequestData();
            var client = DIContainer.Resolve<ICreditCardManagerClient>();
            var result = client.UpdateCustomerContainer(requestData).Result;

            DisplayResult(result, "Update");
        }

        private void DisplayResult(BaseResponseDto<object> result, string method)
        {
            if (result == null || !result.Success)
            {
                var message = string.IsNullOrEmpty(result?.ErrorMessages) ? "Error calling Credit card manager microservice" : result.ErrorMessages;
                EventLogProvider.LogInformation($"{method} 3DSi Container", "ERROR", eventDescription: message);
                LocalizedLabelResult.Text = $"Error - {message}";
            }
            else
            {
                EventLogProvider.LogInformation($"{method} 3DSi Container", "Success", "Credit card manager microservice call to create container was successful");
                LocalizedLabelResult.Text = "Succesfully submitted";
            }
        }

        private CreateCustomerContainerRequestDto CreateRequestData()
        {
            var merchantCode = GetSettingsKey("KDA_CreditCard_MerchantCode");
            var locationCode = GetSettingsKey("KDA_CreditCard_LocationCode");
            var additionalData = new { MerchantCode = merchantCode, LocationCode = locationCode };

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
                AdditionalData = JsonConvert.SerializeObject(additionalData)
            };
        }

        private UpdateCustomerContainerRequestDto UpdateRequestData()
        {
            var merchantCode = GetSettingsKey("KDA_CreditCard_MerchantCode");
            var locationCode = GetSettingsKey("KDA_CreditCard_LocationCode");
            var additionalData = new { MerchantCode = merchantCode, LocationCode = locationCode };

            return new UpdateCustomerContainerRequestDto()
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
                AdditionalData = JsonConvert.SerializeObject(additionalData)
            };
        }

        private string GetSettingsKey(string key)
        {
            var site = new SiteInfoIdentifier(SiteContext.CurrentSiteName);
            return SettingsKeyInfoProvider.GetValue(key, site);
        }
    }
}