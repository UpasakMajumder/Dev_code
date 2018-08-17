using CMS.CustomTables;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Dto.MailingList;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using static Kadena.Helpers.SerializerConfig;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class AddressViewer : CMSAbstractWebPart
    {
        private Guid _containerId;

        public string ConfirmedPageUrl
        {
            get
            {
                return GetStringValue("ConfirmedPageUrl", string.Empty);
            }
        }

        public string ReuploadListPageUrl
        {
            get
            {
                return GetStringValue("ReuploadListPageUrl", string.Empty);
            }
        }

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

                var config = new
                {
                    ErrorList = CreateErrorList(badAddresses),
                    SuccessList = CreateSuccessList(goodAddresses),
                    FormInfo = CreateFormInfo()
                };

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "ui", $"<script>config.localization.modifyMailingList = {JsonConvert.SerializeObject(config, CamelCaseSerializer)}</script>");
            }
        }

        private IEnumerable<MailingAddressDto> GetAddresses()
        {
            var client = DIContainer.Resolve<IMailingListClient>();
            return client.GetAddresses(_containerId).Result.Payload;
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
                        Url = URLHelper.AddParameterToUrl(ReuploadListPageUrl, "containerId", _containerId.ToString())
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
                    ErrorMessage = ConvertErrorCodeToMessage(a.ErrorMessage)
                })
                : null
            };
        }

        private string ConvertErrorCodeToMessage(string errorCode) => ResHelper.GetString("Kadena.MailingList.Errors." + errorCode);

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
                    Redirect = ConfirmedPageUrl,
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
                    },
                    Error = new
                    {
                        Header = ResHelper.GetString("Kadena.MailingList.Error", string.Empty)
                    }
                }
            };
        }
    }
}