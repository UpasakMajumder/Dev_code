using CMS.EventLog;
using CMS.UIControls;
using System;
using System.IO;
using System.Web;
using Kadena.Old_App_Code.Kadena.Imports;
using Kadena.Old_App_Code.Kadena.Imports.Products;

namespace Kadena.CMSModules.Kadena.Pages.Products
{
    public partial class ProductsImport : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HideErrorMessage();
        }

        private void Site_Changed(object sender, EventArgs e)
        {
            
        }

        private int SelectedSiteID => Convert.ToInt32(siteSelector.Value);

        protected void btnUploadProductList_Click(object sender, EventArgs e)
        {
            var file = importFile.PostedFile;
            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                ShowErrorMessage("You need to choose the import file.");
                return;
            }


            var fileData = ReadFileFromRequest(file);
            var excelType = ImportHelper.GetExcelTypeFromFileName(file.FileName);

            try
            {
                /*var result = new UserImportService().ProcessAddressImportFile(fileData, excelType, SelectedSiteID, selectedUsers);
                if (result.ErrorMessages.Length > 0)
                {
                    ShowErrorMessage(FormatImportResult(result));
                }
                */
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Import products", "EXCEPTION", ex);
                ShowErrorMessage("There was an error while processing the request. Detailed information was placed in log.");
            }
        }

        private string FormatImportResult(ImportResult result)
        {
            var headline = "There was an error while processing the request. Error details:<br /><br />";
            return headline + string.Join("<br />", result.ErrorMessages);
        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            var bytes = new ProductTemplateService().GetProductTemplateFile(SelectedSiteID);
            var templateFileName = "products-upload-template.xlsx";
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

        private void HideErrorMessage()
        {
            errorMessageContainer.Visible = false;
        }
    }
}