<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Users.Import"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Users - Send E-mail" Theme="Default" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        <div class="form-group" style="margin-bottom: 2rem">
            <h4>Download template</h4>
            <asp:HyperLink Text="Download" CssClass="btn btn-default" ID="btnDownloadTemplate" Target="_blank" runat="server" style="color: black" />
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
        function onImportFileSelected(e) {
            var files = e.target.files;
            if (files) {
                window.document.getElementById('importFileName').value = files[0].name;
                window.document.getElementById('btnUploadUserList').removeAttribute('disabled');
            }
        }
    </script>
</asp:Content>