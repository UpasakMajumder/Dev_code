using System;
using System.Web;

using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Imports.POS;
using Kadena.Old_App_Code.Kadena.Imports;
using CMS.EventLog;
using System.IO;
using CMS.Helpers;
using Kadena.Models.Common;

public partial class CMSWebParts_Kadena_POS_POSBulkImport : CMSAbstractWebPart
{
    #region "Methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (this.StopProcessing) { } else { }
    }


    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }

    #endregion

    protected void llbtnDownloadTemplate_Click(object sender, EventArgs e)
    {
        var bytes = new POSTemplateService().GetTemplateFile<POSDto>(CurrentSite.SiteID);
        var templateFileName = "pos-upload-template.xlsx";
        WriteFileToResponse(templateFileName, bytes);
    }

    protected void llbtnUploadPOS_Click(object sender, EventArgs e)
    {
        Import(importFile, new POSImportService());
    }

    private void ShowSuccessMessage(string message)
    {
        successMessageContainer.Visible = true;
        successMessage.Text = message;
    }

    private void ShowErrorMessage(string message)
    {
        errorMessageContainer.Visible = true;
        errorMessage.Text = message;
    }

    private void Import(System.Web.UI.WebControls.FileUpload fileControl, ImportServiceBase importer)
    {
        if (fileControl == null || importer == null)
        {
            EventLogProvider.LogEvent(EventType.ERROR, GetType().Name, "IMPORT", "Improper usage of import method.");
            return;
        }

        var file = fileControl.PostedFile;
        if (string.IsNullOrWhiteSpace(file.FileName))
        {
            ShowErrorMessage(ResHelper.GetString("Kadena.POS.BulkImport.NoImportFileSelected"));
            return;
        }
        var excelType = ImportHelper.GetExcelTypeFromFileName(file.FileName);

        var fileData = ReadBytes(file);
        if (fileData == null)
        {
            ShowErrorMessage(ResHelper.GetString("Kadena.POS.BulkImport.ImportFileHasNoData"));
            return;
        }

        try
        {
            var result = importer.Process(fileData, excelType, CurrentSite.SiteID);
            if (result.ErrorMessages.Length > 0)
            {
                ShowErrorMessage(FormatImportResult(result));
            }
            else
            {
                ShowSuccessMessage(ResHelper.GetString("Kadena.POS.BulkImport.SuccessMessage"));
            }

        }
        catch (Exception ex)
        {
            EventLogProvider.LogException(GetType().Name, "EXCEPTION", ex);
            ShowErrorMessage(ResHelper.GetString("Kadena.POS.BulkImport.ErrorMessage"));
        }
    }

    private string FormatImportResult(ImportResult result)
    {
        var headline = $"There was {result.AllMessagesCount} error(s) while processing the request. Make sure all mandatory fields are filled in sheet and/or see Event log for details.<br /><br />First {result.ErrorMessages?.Length ?? 0} errors:<br /><br />";
        return headline + string.Join("<br/>", result.ErrorMessages);
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
        Response.ContentType = ContentTypes.Binary;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

        Response.OutputStream.Write(data, 0, data.Length);
        Response.Flush();

        Response.Close();
    }
}



