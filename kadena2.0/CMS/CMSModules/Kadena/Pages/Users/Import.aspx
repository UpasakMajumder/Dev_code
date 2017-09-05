<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Users.Import"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Users - Send E-mail" Theme="Default" %>
<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        <div class="form-group" style="margin-bottom: 2rem">
            <label class="control-label" for="siteSelector" style="text-align:left">site:</label>
            <cms:SiteSelector ClientIDMode="Static" ID="siteSelector" runat="server" IsLiveSite="false" AllowAll="false" />
        </div>
        <div class="form-group" style="margin-bottom: 2rem">
            <h4>Download template</h4>
            <asp:Button Text="Download" CssClass="btn btn-default" ClientIDMode="Static" ID="btnDownloadTemplate" OnClick="btnDownloadTemplate_Click" runat="server" />
        </div>
        <div class="form-group">
            <h4>Upload file</h4>
            <div class="editing-form-value-cell">
                <div class="editing-form-control-nested-control">
                    <div class="control-group-inline">
                        <input type="text" id="importFileName" class="form-control" />
                        <label for="importFile" class="btn btn-default">Select file (XLS, XLSX)</label>
                        <asp:FileUpload ClientIDMode="Static" id="importFile" name="importFile" accept=".xls, .xlsx" style="display: none" onchange="onImportFileSelected(event)" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-value-cell">
                <asp:Button Text="Upload" CssClass="btn btn-default" ClientIDMode="Static" Enabled="false" ID="btnUploadUserList" OnClick="btnUploadUserList_Click" runat="server" />
            </div>
        </div>
    </div>
    <script>
        window.document.getElementById('btnDownloadTemplate').addEventListener('click', function () {
            // hide loader
            window.setTimeout(function () { window.Loader.hide(); }, 2000);
        }, false);

        // display name of selected file in textbox and enable upload
        function onImportFileSelected(e) {
            var files = e.target.files;
            if (files) {
                window.document.getElementById('importFileName').value = files[0].name;
                window.document.getElementById('btnUploadUserList').removeAttribute('disabled');
            }
        }
    </script>
</asp:Content>