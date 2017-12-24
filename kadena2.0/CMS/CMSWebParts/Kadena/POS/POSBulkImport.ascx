<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_POS_POSBulkImport" CodeBehind="~/CMSWebParts/Kadena/POS/POSBulkImport.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector" TagPrefix="cms" %>

<div class="form-horizontal">

    <!-- Product block -->
    <h4 class="anchor">
        <cms:LocalizedLiteral runat="server" ResourceString="Kadena.POS.BulkImport.POSTemplateTitle"></cms:LocalizedLiteral></h4>
    <div class="form-group" style="margin-bottom: 3rem">
        <cms:LocalizedLinkButton runat="server" ID="llbtnDownloadTemplate" ClientIDMode="Static" ResourceString="Kadena.POS.BulkImport.DownloadTemplate" CssClass="btn-action" OnClick="llbtnDownloadTemplate_Click"></cms:LocalizedLinkButton>
    </div>

    <h4>
        <cms:LocalizedLiteral runat="server" ResourceString="Kadena.POS.BulkImport.UploadSheetCaption"></cms:LocalizedLiteral></h4>
    <div class="form-group">
        <div class="editing-form-value-cell">
            <div class="editing-form-control-nested-control">
                <div class="control-group-inline">
                    <input type="text" id="importFileName" class="input__text" style="width: 250px;" readonly="readonly" />
                    <label for="importFile" class="btn-action">Select file (XLS, XLSX)</label>
                    <br />
                    <asp:FileUpload ClientIDMode="Static" ID="importFile" name="importFile"
                        accept=".xls, .xlsx" Style="display: none" onchange="onImportFileSelected(event)" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <div class="form-group">
        <div class="editing-form-value-cell">
            <cms:LocalizedLinkButton runat="server" ID="llbtnUploadPOS" ClientIDMode="Static" CssClass="btn-action" ResourceString="Kadena.POS.BulkImport.Upload" OnClick="llbtnUploadPOS_Click"></cms:LocalizedLinkButton>
        </div>
    </div>
    <br />
    <asp:PlaceHolder runat="server" ID="errorMessageContainer" Visible="false">
        <div class="notification">
            <div class="notification__body">
                <asp:Literal runat="server" ID="errorMessage"></asp:Literal>
            </div>
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="successMessageContainer" Visible="false">
        <div class="notification">
            <div class="notification__body">
                <asp:Literal runat="server" ID="successMessage"></asp:Literal>
            </div>
        </div>
    </asp:PlaceHolder>

</div>
<script>
    window.document.getElementById('llbtnDownloadTemplate').addEventListener('click', function () {
        // hide loader
        window.setTimeout(function () { window.Loader.hide(); }, 2000);
    }, false);

    // display name of selected file in textbox
    function onImportFileSelected(e) {
        var files = e.target.files;
        if (files) {
            window.document.getElementById('importFileName').value = files[0].name;
        }
    }
</script>
