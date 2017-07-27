using CMS.DataEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Dto.MailingList;
using Kadena.Old_App_Code.Helpers;
using Kadena2.MicroserviceClients.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class AddressViewer : CMSAbstractWebPart
    {
        private Guid _containerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["containerId"]))
            {
                _containerId = new Guid(Request.QueryString["containerId"]);
            }
            LoadData();
        }

        private void LoadData()
        {
            if (_containerId != Guid.Empty)
            {
                var getAddressUrl = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_GetMailingAddressesUrl");
                var client = new MailingListClient();

                var addresses = client.GetAddresses(getAddressUrl, _containerId).Result.Payload;
                var badAddresses = addresses.Where(a => a.Error != null);
                var goodAddresses = addresses.Where(a => a.Error == null);

                var setting = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var config = new
                {
                    ModifyMailingList = new
                    {
                        ErrorList = new
                        {
                            Header = ResHelper.GetStringFormat("Kadena.MailingList.BadAddressesFound", badAddresses.Count()),
                            Tip = ResHelper.GetString("Kadena.MailingList.ToCorrectErrorsGoTo"),
                            Btns = new
                            {
                                Reupload = new
                                {
                                    Text = ResHelper.GetString("Kadena.MailingList.ReuploadList"),
                                    Url = URLHelper.AddParameterToUrl(GetStringValue("ReuploadListPageUrl", string.Empty), "containerId", _containerId.ToString())
                                },
                                Correct = ResHelper.GetString("Kadena.MailingList.CorrectErrors")
                            },
                            Items = badAddresses.Count() > 0 ? badAddresses.Select(a => new UpdateAddressDto
                            {
                                Id = a.Id,
                                FullName = a.FirstName,
                                FirstAddressLine = a.Address1,
                                SecondAddressLine = a.Address2,
                                City = a.City,
                                State = a.State,
                                PostalCode = a.Zip,
                                ErrorMessage = a.Error
                            })
                            : null
                        },
                        SuccessList = new
                        {
                            Header = ResHelper.GetStringFormat("Kadena.MailingList.GoodAddressesFound", goodAddresses.Count()),
                            Btns = new
                            {
                                Use = new
                                {
                                    Text = ResHelper.GetString("Kadena.MailingList.UseOnlyCorrect"),
                                    Url = "/klist/useonlycorrect"
                                }
                            },
                            Items = goodAddresses.Count() > 0 ? goodAddresses.Select(a => new UpdateAddressDto
                            {
                                Id = a.Id,
                                FullName = a.FirstName,
                                FirstAddressLine = a.Address1,
                                SecondAddressLine = a.Address2,
                                City = a.City,
                                State = a.State,
                                PostalCode = a.Zip,
                                ErrorMessage = a.Error
                            })
                            : null
                        },
                        FormInfo = new
                        {
                            Title = ResHelper.GetString("Kadena.MailingList.EditorTitle"),
                            DownloadErrorFile = new
                            {
                                Url = string.Empty,
                                Text = string.Empty
                            },
                            DiscardChanges = ResHelper.GetString("Kadena.MailingList.DiscardChanges"),
                            ConfirmChanges = new
                            {
                                Text = ResHelper.GetString("Kadena.MailingList.ConfirmChanges"),
                                Redirect = "/k-list/processing",
                                Request = "/klist/update"
                            },
                            Message = new
                            {
                                Required = ResHelper.GetString("Kadena.MailingList.EnterValidValue")
                            },
                            Fields = new
                            {
                                FullName = new
                                {
                                    Required = true,
                                    Header = ResHelper.GetString("Kadena.MailingList.Name", string.Empty)
                                },
                                FirstAddressLine = new
                                {
                                    Required = true,
                                    Header = ResHelper.GetString("Kadena.MailingList.Address1", string.Empty)
                                },
                                SecondAddressLine = new
                                {
                                    Header = ResHelper.GetString("Kadena.MailingList.Address2", string.Empty)
                                },
                                City = new
                                {
                                    Required = true,
                                    Header = ResHelper.GetString("Kadena.MailingList.City", string.Empty)
                                },
                                State = new
                                {
                                    Required = true,
                                    Header = ResHelper.GetString("Kadena.MailingList.State", string.Empty),
                                    Value = StateInfoProvider
                                                    .GetStates()
                                                    .Column("StateCode")
                                                    .Select(s => s["StateCode"].ToString())
                                },
                                PostalCode = new
                                {
                                    Required = true,
                                    Header = ResHelper.GetString("Kadena.MailingList.Zip", string.Empty)
                                }
                            }
                        }
                    }
                };
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ui", $"<script>config.localization.ui = {JsonConvert.SerializeObject(config, setting)}</script>");
            }
        }
    }
}