using CMS.EventLog;
using CMS.UIControls;
using System;
using System.IO;
using System.Web;
using Kadena.Old_App_Code.Kadena.Imports;
using Kadena.Old_App_Code.Kadena.Imports.Products;
using CMS.DocumentEngine.Types.KDA;
using Kadena.Models.Common;

namespace Kadena.CMSModules.Kadena.Pages.Products
{
    public partial class ProductImagesImport : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HideResultMessages();
        }

        private int SelectedSiteID => Convert.ToInt32(siteSelector.Value);
        private string SelectedPageType => ddlProductPageType.SelectedValue;

        protected void btnUploadProductList_Click(object sender, EventArgs e)
        {
            var file = importFile.PostedFile;
            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                ShowErrorMessage("You need to choose the import file.");
                return;
            }

            if (SelectedSiteID == 0)
            {
                ShowErrorMessage("You need to choose the Site.");
                return;
            }

            var fileData = ReadFileFromRequest(file);
            var excelType = ImportHelper.GetExcelTypeFromFileName(file.FileName);

            try
            {
                ImportResult result = null;
                if (SelectedPageType.Equals(CampaignsProduct.CLASS_NAME))
                {
                    result = new CampaignProductImportService().ProcessProductImagesImportFile(fileData, excelType, SelectedSiteID);
                }
                else
                {
                    result = new ProductImportService().ProcessProductImagesImportFile(fileData, excelType, SelectedSiteID);
                }
                if (result.ErrorMessages.Length > 0)
                {
                    ShowErrorMessage(FormatImportResult(result));
                }
                else
                {
                    ShowSuccessMessage("Operation successfully completed");
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Import product images", "EXCEPTION", ex);
                ShowErrorMessage("There was an error while processing the request. Detailed information was placed in log.");
            }
        }

        private string FormatImportResult(ImportResult result)
        {
            var headline = $"There was {result.AllMessagesCount} error(s) while processing the request. First {result.ErrorMessages.Length} errors: <br /><br />";
            return headline + string.Join("<br />", result.ErrorMessages);
        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            byte[] bytes;
            if (SelectedPageType.Equals(CampaignsProduct.CLASS_NAME))
            {
                bytes = new TemplateServiceBase().GetTemplateFile<CampaignProductImageDto>(SelectedSiteID);
            }
            else
            {
                bytes = new TemplateServiceBase().GetTemplateFile<ProductImageDto>(SelectedSiteID);
            }
            var templateFileName = "productimages-upload-template.xlsx";
            WriteFileToResponse(templateFileName, bytes);
        }

        private byte[] ReadFileFromRequest(HttpPostedFile fileRequest)
        {
            using (var binaryReader = new BinaryReader(fileRequest.InputStream))
            {
                return binaryReader.ReadBytes(fileRequest.ContentLength);
            }
        }

        private void WriteFileToResponse(string filename, byte[] data)
        {
            Response.Clear();
            Response.ContentType = ContentTypes.Binary;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

            Response.OutputStream.Write(data, 0, data.Length);
            Response.Flush();

            Response.Close();
        }

        private void ShowErrorMessage(string message)
        {
            errorMessageContainer.Visible = true;
            errorMessage.Text = message;
        }

        private void ShowSuccessMessage(string message)
        {
            successMessageContainer.Visible = true;
            successMessage.Text = message;
        }

        private void HideResultMessages()
        {
            errorMessageContainer.Visible = false;
            successMessageContainer.Visible = false;
        }
    }
}