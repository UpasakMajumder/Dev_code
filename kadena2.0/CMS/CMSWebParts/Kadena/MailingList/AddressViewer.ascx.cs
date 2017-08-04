using CMS.CustomTables;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Dto.MailingList;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class AddressViewer : CMSAbstractWebPart
    {
        private Guid _containerId;

        public int NumberOfItems
        {
            get
            {
                return int.Parse(GetStringValue("NumberOfItems", "4"));
            }
        }

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
                var addresses = GetAddresses();
                var badAddresses = addresses.Where(a => a.ErrorMessage != null);
                var goodAddresses = addresses.Where(a => a.ErrorMessage == null);

                PopulateErrors(badAddresses);

                var config = new
                {
                    ModifyMailingList = new
                    {
                        ErrorList = CreateErrorList(badAddresses),
                        SuccessList = CreateSuccessList(goodAddresses),
                        FormInfo = CreateFormInfo()
                    }
                };

                var setting = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "ui", $"<script>config.localization.ui = {JsonConvert.SerializeObject(config, setting)}</script>");
            }
        }

        private void PopulateErrors(IEnumerable<MailingAddressDto> badAddresses)
        {
            var errorsDictionary = CustomTableItemProvider
                .GetItems("KDA.AddressErrors")
                .ToDictionary(i => i["ErrorCode"].ToString(), i => i["ErrorDescription"].ToString());
            var missingCodes = new HashSet<string>();
            foreach (var address in badAddresses)
            {
                string errorDescription;
                if (errorsDictionary.TryGetValue(address.ErrorMessage, out errorDescription))
                {
                    address.ErrorMessage = errorDescription;
                }
                else
                {
                    if (!missingCodes.Contains(address.ErrorMessage))
                    {
                        missingCodes.Add(address.ErrorMessage);
                    }
                }
            }
            if (missingCodes.Any())
            {
                var exc = new KeyNotFoundException($"The error description is not found for following key(s): {string.Join(", ", missingCodes)}");
                EventLogProvider.LogWarning("Mailing Addresses Load", "WARNING", exc, CurrentSite.SiteID, string.Empty);
            }
        }

        private IEnumerable<MailingAddressDto> GetAddresses()
        {
            var getAddressUrl = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_GetMailingAddressesUrl");
            var client = new MailingListClient();
            return client.GetAddresses(getAddressUrl, _containerId).Result.Payload;
        }

        private object CreateErrorList(IEnumerable<MailingAddressDto> addresses)
        {
            return new
            {
                Header = ResHelper.GetStringFormat("Kadena.MailingList.BadAddressesFound", addresses.Count()),
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
                Items = addresses.Any() ? addresses.Select(a => new UpdateAddressDto
                {
                    Id = a.Id,
                    FullName = a.FirstName,
                    FirstAddressLine = a.Address1,
                    SecondAddressLine = a.Address2,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.Zip,
                    ErrorMessage = a.ErrorMessage
                })
                : null
            };
        }

        private object CreateSuccessList(IEnumerable<MailingAddressDto> addresses)
        {
            return new
            {
                Header = ResHelper.GetStringFormat("Kadena.MailingList.GoodAddressesFound", addresses.Count()),
                Btns = new
                {
                    Use = new
                    {
                        Text = ResHelper.GetString("Kadena.MailingList.UseOnlyCorrect"),
                        Url = "/klist/useonlycorrect"
                    }
                },
                Items = addresses.Any() ? addresses.Select(a => new UpdateAddressDto
                {
                    Id = a.Id,
                    FullName = a.FirstName,
                    FirstAddressLine = a.Address1,
                    SecondAddressLine = a.Address2,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.Zip,
                })
                .Take(NumberOfItems)
                : null
            };
        }

        private object CreateFormInfo()
        {
            return new
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
            };
        }
    }
}