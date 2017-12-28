using CMS.EventLog;
using CMS.UIControls;
using System;
using System.IO;
using System.Web;
using Kadena.Old_App_Code.Kadena.Imports;
using Kadena.Old_App_Code.Kadena.Imports.Products;

namespace Kadena.CMSModules.Kadena.Pages.Products
{
    public partial class CampaignProductsImport : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HideResultMessage();
        }

        private int SelectedSiteID => Convert.ToInt32(siteSelector.Value);

        protected void btnUploadProductList_Click(object sender, EventArgs e)
        {
            Import(importFile, new CampaignProductImportService());
        }

        private string FormatImportResult(ImportResult result)
        {
            var headline = $"There was {result.AllMessagesCount} error(s) while processing the request. Make sure all mandatory fields are filled in sheet and/or see Event log for details.<br /><br />First {result.ErrorMessages?.Length ?? 0} errors:<br /><br />";
            return headline + string.Join("<br/>", result.ErrorMessages);
        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            var bytes = new CampaignProductImagesTemplateService().GetTemplateFile<CampaignProductDto>(SelectedSiteID);
            var templateFileName = "campaigns-products-upload-template.xlsx";
            WriteFileToResponse(templateFileName, bytes);
        }

        private byte[] ReadBytes(HttpPostedFile fileRequest)
        {
            using (var binaryReader = new BinaryReader(fileRequest.InputStream))
            {
                return binaryReader.ReadBytes(fileRequest.ContentLength);
            }
        }

        private void WriteFileToResponse(string filename, byte[] data)
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
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

        private void HideResultMessage()
        {
            errorMessageContainer.Visible = false;
            successMessageContainer.Visible = false;
        }

        private void Import(System.Web.UI.WebControls.FileUpload fileControl, ImportServiceBase importer)
        {
            if (fileControl == null || importer == null)
            {
                EventLogProvider.LogEvent(EventType.ERROR, GetType().Name, "IMPORT", "Improper usage of import method.");
                return;
            }

            if (SelectedSiteID == 0)
            {
                ShowErrorMessage("You need to choose the Site.");
                return;
            }

            var file = fileControl.PostedFile;
            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                ShowErrorMessage("You need to choose the import file.");
                return;
            }
            var excelType = ImportHelper.GetExcelTypeFromFileName(file.FileName);

            var fileData = ReadBytes(file);
            if (fileData == null)
            {
                ShowErrorMessage("Selected file doesn't have any data.");
                return;
            }

            try
            {
                var result = importer.Process(fileData, excelType, SelectedSiteID);
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
                EventLogProvider.LogException(GetType().Name, "EXCEPTION", ex);
                ShowErrorMessage("There was an error while processing the request. Detailed information was placed in Event log.");
            }
        }
    }
}