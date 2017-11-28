using CMS.EventLog;
using CMS.UIControls;
using Kadena.Old_App_Code.Kadena.Imports.Users;
using System;
using System.IO;
using System.Web;
using Kadena.Old_App_Code.Kadena.Imports;
using CMS.DataEngine;

namespace Kadena.CMSModules.Kadena.Pages.Users
{
    public partial class Import : CMSPage
    {
        private readonly string _templateType = "membershipchangepassword";

        protected void Page_Load(object sender, EventArgs e)
        {
            HideResultMessage();

            siteSelector.UniSelector.OnSelectionChanged += Site_Changed;
            siteSelector.DropDownSingleSelect.AutoPostBack = true;

            SetUpEmailTemplateSelector();
        }

        private void SetUpEmailTemplateSelector()
        {
            var where = selEmailTemplate.WhereCondition;
            where = SqlHelper.AddWhereCondition(where, "EmailTemplateSiteId = " + SelectedSiteID);
            where = SqlHelper.AddWhereCondition(where, $"EmailTemplateType = '{_templateType}'");
            selEmailTemplate.WhereCondition = where;
        }

        private void Site_Changed(object sender, EventArgs e)
        {
            selEmailTemplate.Value = null;
            selEmailTemplate.Reload(true);
            SetUpEmailTemplateSelector();
            pnlTemplate.Update();
        }

        private int SelectedSiteID => Convert.ToInt32(siteSelector.Value);

        protected void btnUploadUserList_Click(object sender, EventArgs e)
        {
            var emailTemplateName = selEmailTemplate.Value.ToString();
            if (string.IsNullOrWhiteSpace(emailTemplateName))
            {
                ShowErrorMessage(GetString("Kadena.Email.TemplateNotSelected"));
                return;
            }

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
                var result = new UserImportService { PasswordEmailTemplateName = emailTemplateName }
                    .Process(fileData, excelType, SelectedSiteID);
                if (result.ErrorMessages.Length > 0)
                {
                    ShowErrorMessage(FormatImportResult(result));
                }
                else
                {
                    ShowSuccessMessage("operation completed sucessfully");
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Import users", "EXCEPTION", ex);
                ShowErrorMessage("There was an error while processing the request. Detailed information was placed in Event log.");
            }
        }

        private string FormatImportResult(ImportResult result)
        {
            var headline = $"There was {result.AllMessagesCount} errors while processing the request. First {result.ErrorMessages?.Length ?? 0} error details:<br /><br />";
            return headline + string.Join("<br />", result.ErrorMessages);
        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            var bytes = new UserTemplateService().GetTemplateFile<UserDto>(SelectedSiteID);
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