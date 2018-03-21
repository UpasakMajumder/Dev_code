using CMS.EventLog;
using CMS.UIControls;
using Kadena.Old_App_Code.Kadena.Imports.Users;
using System;
using System.IO;
using System.Web;
using Kadena.Old_App_Code.Kadena.Imports;
using System.Linq;
using Kadena.Models.Common;

namespace Kadena.CMSModules.Kadena.Pages.Users
{
    public partial class Addresses : CMSPage
    {
        public int[] SelectedUserIds
        {
            get
            {
                var value = userSelector.Value as string;
                if (!string.IsNullOrEmpty(value))
                {
                    var values = value.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    return values.Select(v => int.Parse(v)).ToArray();
                }

                return new int[0];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HideResultMessage();

            siteSelector.UniSelector.OnSelectionChanged += Site_Changed;
            siteSelector.DropDownSingleSelect.AutoPostBack = true;
        }

        private void Site_Changed(object sender, EventArgs e)
        {
            userSelector.Value = null;
        }

        private int SelectedSiteID => Convert.ToInt32(siteSelector.Value);

        protected void btnUploadUserList_Click(object sender, EventArgs e)
        {
            var file = importFile.PostedFile;
            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                ShowErrorMessage("You need to choose the import file.");
                return;
            }

            var selectedUsers = SelectedUserIds;
            if ((selectedUsers?.Length ?? 0) == 0)
            {
                ShowErrorMessage("You need to select at least one user.");
                return;
            }


            var fileData = ReadFileFromRequest(file);
            var excelType = ImportHelper.GetExcelTypeFromFileName(file.FileName);

            try
            {
                var result = new AddressImporter { UserIds = selectedUsers }
                    .Process(fileData, excelType, SelectedSiteID);
                if (result.ErrorMessages.Length > 0)
                {
                    ShowErrorMessage(FormatImportResult(result));
                }
                else
                {
                    ShowSuccessMessage("Operation completed successfully");
                }

            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Import addresses", "EXCEPTION", ex);
                ShowErrorMessage("There was an error while processing the request. Detailed information was placed in Event log.");
            }
        }

        private string FormatImportResult(ImportResult result)
        {
            var headline = $"There was {result.AllMessagesCount} error while processing the request. First {result.ErrorMessages?.Length ?? 0} error details:<br /><br />";
            return headline + string.Join("<br />", result.ErrorMessages);
        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            var bytes = new TemplateServiceBase().GetTemplateFile<AddressDto>(SelectedSiteID);
            var templateFileName = "addresses-upload-template.xlsx";

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

        private void HideResultMessage()
        {
            errorMessageContainer.Visible = false;
            successMessageContainer.Visible = false;
        }
    }
}