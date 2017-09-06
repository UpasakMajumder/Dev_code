using CMS.EventLog;
using CMS.UIControls;
using Kadena.Old_App_Code.Kadena.Imports.Users;
using System;
using System.IO;
using System.Web;

namespace Kadena.CMSModules.Kadena.Pages.Users
{
    public partial class Import : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HideErrorMessage();
        }

        private int SelectedSiteID => Convert.ToInt32(siteSelector.Value);

        protected void btnUploadUserList_Click(object sender, EventArgs e)
        {
            var importService = new UserImportService();

            var file = importFile.PostedFile;
            var fileData = ReadFileFromRequest(file);
            var excelType = importService.GetExcelTypeFromFileName(file.FileName);

            try
            {
                importService.ProcessImportFile(fileData, excelType, SelectedSiteID);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Import users", "EXCEPTION", ex);
                ShowErrorMessage("There was an error while processing the request. Detailed information was placed in log.");
            }
        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            var bytes = new UserImportService().GetTemplateFile(SelectedSiteID);
            var templateFileName = "users-upload-template.xlsx";

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