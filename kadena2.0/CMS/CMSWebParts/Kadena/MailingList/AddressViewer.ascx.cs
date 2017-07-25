using CMS.Globalization;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Dto.MailingList;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.Old_App_Code.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class AddressViewer : CMSAbstractWebPart
    {
        private Guid _containerId;
        private IEnumerable<MailingAddressDto> _badAddresses;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PrevPageUrl"] = Request.UrlReferrer?.ToString();
            }

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
                var addresses = ServiceHelper.GetMailingAddresses(_containerId);
                _badAddresses = addresses.Where(a => a.Error != null);
                var goodAddresses = addresses.Where(a => a.Error == null);

                //if (_badAddresses.Count() > 0)
                //{
                //    pnlBadAddresses.Visible = true;
                //    textBadAddresses.InnerText = ResHelper.GetStringFormat("Kadena.MailingList.BadAddressesFound",
                //        _badAddresses.Count());
                //    FillTable(tblBadAddresses, _badAddresses.Take(4));
                //}
                //else
                //{
                //    pnlBadAddresses.Visible = false;
                //    btnUseOnlyGoodAddresses.Visible = false;
                //}

                //if (goodAddresses.Count() > 0)
                //{
                //    pnlGoodAddresses.Visible = true;
                //    textGoodAddresses.InnerText = ResHelper.GetStringFormat("Kadena.MailingList.GoodAddressesFound",
                //        goodAddresses.Count());
                //    FillTable(tblGoodAddresses, goodAddresses.Take(4));
                //}
                //else
                //{
                //    pnlGoodAddresses.Visible = false;
                //}

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
                            Header = ResHelper.GetStringFormat("Kadena.MailingList.BadAddressesFound", _badAddresses.Count()),
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
                            Items = _badAddresses.Select(a => new UpdateAddressDto
                            {
                                Id = a.Id,
                                FullName = a.firstName,
                                FirstAddressLine = a.address1,
                                SecondAddressLine = a.address2,
                                City = a.city,
                                State = a.state,
                                PostalCode = a.zip,
                                ErrorMessage = a.Error
                            })
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
                            Items = goodAddresses.Select(a => new UpdateAddressDto
                            {
                                Id = a.Id,
                                FullName = a.firstName,
                                FirstAddressLine = a.address1,
                                SecondAddressLine = a.address2,
                                City = a.city,
                                State = a.state,
                                PostalCode = a.zip,
                                ErrorMessage = a.Error
                            })
                        },
                        FormInfo = new
                        {
                            Title = string.Empty,
                            DownloadErrorFile = new
                            {
                                Url = string.Empty,
                                Text = ResHelper.GetString("Kadena.MailingList.DownloadErrorFile")
                            },
                            DiscardChanges = ResHelper.GetString("Kadena.MailingList.DiscardChange"),
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

        private static void FillTable(Table table, IEnumerable<MailingAddressDto> addresses)
        {
            table.Rows.Clear();
            var haveTitle = addresses.Count(a => a.Title != null) > 0;
            var haveLastName = addresses.Count(a => a.LastName != null) > 0;

            #region Add Header
            var header = new TableRow();
            if (haveTitle)
            {
                header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.Title", string.Empty) });
            }
            header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.Name", string.Empty) });
            if (haveLastName)
            {
                header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.LastName", string.Empty) });
            }
            header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.Address1", string.Empty) });
            header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.Address2", string.Empty) });
            header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.City", string.Empty) });
            header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.State", string.Empty) });
            header.Cells.Add(new TableHeaderCell { Text = ResHelper.GetString("Kadena.MailingList.Zip", string.Empty) });
            table.Rows.Add(header);
            #endregion

            #region Add Data
            foreach (var a in addresses)
            {
                var row = new TableRow();
                if (haveTitle)
                {
                    row.Cells.Add(new TableCell { Text = a.Title ?? "-" });
                }
                row.Cells.Add(new TableCell { Text = a.firstName ?? "-" });
                if (haveLastName)
                {
                    row.Cells.Add(new TableCell { Text = a.LastName ?? "-" });
                }
                row.Cells.Add(new TableCell { Text = a.address1 ?? "-" });
                row.Cells.Add(new TableCell { Text = a.address2 ?? "-" });
                row.Cells.Add(new TableCell { Text = a.city ?? "-" });
                row.Cells.Add(new TableCell { Text = a.state ?? "-" });
                row.Cells.Add(new TableCell { Text = a.zip ?? "-" });
                table.Rows.Add(row);
            }
            #endregion
        }

        protected void btnSaveList_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Session["PrevPageUrl"]?.ToString()))
            {
                Response.Redirect(Session["PrevPageUrl"].ToString());
            }
        }

        protected void btnReupload_ServerClick(object sender, EventArgs e)
        {
            var url = URLHelper.AddParameterToUrl(GetStringValue("ReuploadListPageUrl", string.Empty)
                , "containerId", _containerId.ToString());
            Response.Redirect(url);
        }

        protected void btnUseOnlyGoodAddresses_ServerClick(object sender, EventArgs e)
        {
            if (_containerId != Guid.Empty && _badAddresses != null && _badAddresses.Count() > 0)
            {
                ServiceHelper.RemoveAddresses(_containerId, _badAddresses.Select(a => a.Id).ToArray());
                ServiceHelper.ValidateAddresses(_containerId);
                LoadData();
            }
        }
    }
}