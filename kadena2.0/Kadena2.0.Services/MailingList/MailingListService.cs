using Newtonsoft.Json;
using System.Net.Http;
using CMS.DataEngine;
using CMS.SiteProvider;

namespace Kadena.Services.MailingList
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MailingListService" in both code and config file together.
    public class MailingListService : IMailingListService
    {
        private readonly string _customerNameSettingKey = "";
        private readonly string _mailingServiceSettingKey = "";

        public ResponseMessage UploadFile(UploadFileData data)
        {
            if (string.IsNullOrWhiteSpace(data.FileName))
            {
                return new ResponseMessage { IsSuccess = false, Message = "File name can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.MailType))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Mail type can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.Product))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Product can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.Validity))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Validity can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.FileUrl))
            {
                return new ResponseMessage { IsSuccess = false, Message = "File url can not be empty." };
            }

            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");
            string mailingServiceAddress = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");

            if(string.IsNullOrWhiteSpace(customerName))
            {
                return new ResponseMessage { IsSuccess = false, Message = "CustomerName not specified." };
            }
            if (string.IsNullOrWhiteSpace(mailingServiceAddress))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Mailing service address not specified." };
            }


            // Create container
            // Upload file
            // Return headers


            return new ResponseMessage { IsSuccess = true, Message = "Method called" };
        }
    }
}
