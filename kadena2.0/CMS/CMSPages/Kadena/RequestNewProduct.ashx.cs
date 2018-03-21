using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.Localization;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.SiteProvider;
using Kadena.Dto.General;
using Kadena.Models.Common;
using Kadena.Old_App_Code.Kadena.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.CMSPages.Kadena
{
    public class RequestNewProduct : IHttpHandler
    {
        #region static members

        public const string _RequestNewProductFormCodeName = "KDA_NewProductRequestForm";

        #endregion

        #region Public methods

        public void ProcessRequest(HttpContext context)
        {
            var result = new GeneralResultDTO();
            context.Response.ContentType = ContentTypes.Json;

            string description = context.Request.Form["description"];
            var files = new List<HttpPostedFile>();
            if (context.Request.Files.Count > 0)
            {
                for (int i = 0; context.Request.Files.Count > i; i++)
                {
                    files.Add(context.Request.Files[i]);
                }
            }

            #region Validation

            if (string.IsNullOrWhiteSpace(description))
            {
                result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewProductForm.DescriptionIsMandatory", LocalizationContext.CurrentCulture.CultureCode) };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            if (files.Count > 0)
            {
                if (files.Count > 4)
                {
                    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewProductForm.NumberOfAttachmentsIsTooBig", LocalizationContext.CurrentCulture.CultureCode) };
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
                        result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewProductForm.FileExtensionIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
                        context.Response.Write(JsonConvert.SerializeObject(result));
                        return;
                    }
                    filesTotalSize += requestFile.ContentLength;
                }
                if (filesTotalSize > 10000000)
                {
                    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewProductForm.TotalAttachmentsSizeIsTooBig", LocalizationContext.CurrentCulture.CultureCode) };
                    context.Response.Write(JsonConvert.SerializeObject(result));
                    return;
                }
            }

            #endregion

            result = RequestNewProductInternal(description, files);
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

        private GeneralResultDTO RequestNewProductInternal(string description, List<HttpPostedFile> files)
        {
            var newProductForm = BizFormInfoProvider.GetBizFormInfo(_RequestNewProductFormCodeName, SiteContext.CurrentSiteID);

            if (newProductForm != null)
            {
                var newProductFormClass = DataClassInfoProvider.GetDataClassInfo(newProductForm.FormClassID);
                string newProductFormClassName = newProductFormClass.ClassName;

                BizFormItem newFormItem = BizFormItem.New(newProductFormClassName);

                newFormItem.SetValue("Description", description);

                newFormItem.SetValue("FormInserted", DateTime.Now);
                newFormItem.SetValue("FormUpdated", DateTime.Now);

                newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
                newFormItem.SetValue("UserName", MembershipContext.AuthenticatedUser.UserName);

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
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewProductForm.RepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        #endregion
    }
}