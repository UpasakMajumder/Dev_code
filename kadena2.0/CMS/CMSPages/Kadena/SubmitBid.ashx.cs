using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.Localization;
using CMS.OnlineForms;
using CMS.SiteProvider;
using Kadena.Dto.General;
using Kadena.Models.Common;
using Kadena.Old_App_Code.Kadena.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Kadena.CMSPages.Kadena
{
    public class SubmitBid : IHttpHandler
    {
        #region static members

        public const string _BidFormCodeName = "KDA_BidForm";

        #endregion

        #region Public methods

        public void ProcessRequest(HttpContext context)
        {
            var result = new GeneralResultDTO();
            context.Response.ContentType = ContentTypes.Json;

            string name = context.Request.Form["name"];
            string description = context.Request.Form["description"];
            string requestType = context.Request.Form["requestType"];
            string biddingWay = context.Request.Form["biddingWay"];
            int biddingWayNumber = ValidationHelper.GetInteger(context.Request.Form["biddingWayNumber"], 0);

            DateTime productionDate;
            if (!DateTime.TryParseExact(context.Request.Form["productionDateText"],
                                   "MM/dd/yyyy",
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   System.Globalization.DateTimeStyles.None,
                                   out productionDate))
            {
                result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.ProductionDateInvalidMessage", LocalizationContext.CurrentCulture.CultureCode) };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            DateTime? selectionDate = null;
            if (context.Request.Form["selectionDateText"] != null)
            {
                DateTime selectionDateTemp;
                if (!DateTime.TryParseExact(context.Request.Form["selectionDateText"],
                                  "MM/dd/yyyy",
                                  System.Globalization.CultureInfo.InvariantCulture,
                                  System.Globalization.DateTimeStyles.None,
                                  out selectionDateTemp))
                {
                    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.SelectionDateInvalidMessage", LocalizationContext.CurrentCulture.CultureCode) };
                    context.Response.Write(JsonConvert.SerializeObject(result));
                    return;
                }
                else
                {
                    selectionDate = new DateTime?(selectionDateTemp);
                }
            }                                               
            var files = new List<HttpPostedFile>();
            if (context.Request.Files.Count > 0)
            {
                for (int i = 0; context.Request.Files.Count > i; i++)
                {
                    files.Add(context.Request.Files[i]);
                }
            }

            #region Validation

            if (string.IsNullOrWhiteSpace(name))
            {
                result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.NameIsMandatory", LocalizationContext.CurrentCulture.CultureCode) };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.DescriptionIsMandatory", LocalizationContext.CurrentCulture.CultureCode) };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            if (files.Count > 0)
            {
                if (files.Count > 4)
                {
                    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.NumberOfAttachmentsIsTooBig", LocalizationContext.CurrentCulture.CultureCode) };
                    context.Response.Write(JsonConvert.SerializeObject(result));
                    return;
                }
                int filesTotalSize = 0;

                foreach (var requestFile in files)
                {
                    if (requestFile.ContentType != ContentTypes.Png 
                        && requestFile.ContentType != ContentTypes.Jpeg 
                        && requestFile.ContentType != ContentTypes.Pdf)
                    {
                        result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.FileExtensionIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
                        context.Response.Write(JsonConvert.SerializeObject(result));
                        return;
                    }
                    filesTotalSize += requestFile.ContentLength;
                }
                if (filesTotalSize > 10000000)
                {
                    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.TotalAttachmentsSizeIsTooBig", LocalizationContext.CurrentCulture.CultureCode) };
                    context.Response.Write(JsonConvert.SerializeObject(result));
                    return;
                }
            }
            //if (productionDate == DateTime.MinValue)
            //{
            //    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.ProductionDateInvalidMessage", LocalizationContext.CurrentCulture.CultureCode) };
            //    context.Response.Write(JsonConvert.SerializeObject(result));
            //    return;
            //}

            #endregion

            result = SubmitBidInternal(name, description, requestType, biddingWay, biddingWayNumber, files, productionDate, selectionDate);
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Private methods

        private GeneralResultDTO SubmitBidInternal(string name, string description, string requestType, string biddingWay, int numberOfBidings, List<HttpPostedFile> files, DateTime productionDate, DateTime? selectionDate)
        {
            var bidForm = BizFormInfoProvider.GetBizFormInfo(_BidFormCodeName, SiteContext.CurrentSiteID);
            if (bidForm != null)
            {
                var bidFormClass = DataClassInfoProvider.GetDataClassInfo(bidForm.FormClassID);
                string bidFormClassName = bidFormClass.ClassName;

                BizFormItem newFormItem = BizFormItem.New(bidFormClassName);

                newFormItem.SetValue("Name", name);
                newFormItem.SetValue("Description", description);
                newFormItem.SetValue("RequestType", requestType);
                newFormItem.SetValue("BiddingWayText", biddingWay);
                newFormItem.SetValue("BiddingWayNumber", numberOfBidings);

                newFormItem.SetValue("ProductionDate", productionDate);
                if (selectionDate.HasValue)
                {
                    newFormItem.SetValue("SelectionDate", selectionDate);
                }

                newFormItem.SetValue("FormInserted", DateTime.Now);
                newFormItem.SetValue("FormUpdated", DateTime.Now);

                newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);

                if (files.Count > 0)
                {
                    for (int i = 0; files.Count > i; i++)
                    {
                        string extension = System.IO.Path.GetExtension(files[i].FileName);
                        string fileName = new FormsHelper().GetNewGuidName(extension);
                        string formFilesFolderPath = FormHelper.GetBizFormFilesFolderPath(SiteContext.CurrentSiteName);
                        string fileNameString = fileName + "/" + Path.GetFileName(files[i].FileName);
                        new FormsHelper().SaveFileToDisk(files[i], fileName, formFilesFolderPath);
                        newFormItem.SetValue(string.Format("File{0}", i + 1), (object)fileNameString);
                    }
                }
                newFormItem.Insert();

                new FormsHelper().SendFormEmail(newFormItem, files.Count);

                return new GeneralResultDTO { success = true };
            }
            else
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewBidRequest.RepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        #endregion
    }
}