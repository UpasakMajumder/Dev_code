<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Users.Import"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Users - Send E-mail" Theme="Default" %>

<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        <div class="form-group" style="margin-bottom: 3rem">
            <label class="control-label" for="siteSelector" style="text-align: left">Site:</label>
            <cms:SiteSelector ClientIDMode="Static" ID="siteSelector" runat="server" IsLiveSite="false" AllowAll="false" />
        </div>

        <h4>Download template</h4>
        <div class="form-group" style="margin-bottom: 3rem">
            <asp:Button Text="Download" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnDownloadTemplate" OnClick="btnDownloadTemplate_Click" runat="server" />
        </div>

        <h4>Upload template with user data</h4>
        <div class="form-group">
            <div class="editing-form-value-cell">
                <div class="editing-form-control-nested-control">
                    <div class="control-group-inline">
                        <label class="control-label" style="text-align: left">File:</label>
                        <input type="text" id="importFileName" class="form-control" readonly="readonly" />
                        <label for="importFile" class="btn btn-default">Select file (XLS, XLSX)</label>
                        <asp:FileUpload ClientIDMode="Static" ID="importFile" name="importFile" accept=".xls, .xlsx" Style="display: none" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <cms:CMSUpdatePanel ID="pnlTemplate" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="form-group">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblEmailTemplate" runat="server" EnableViewState="false"
                        ResourceString="Kadena.Email.SelectTemplate" DisplayColon="true" style="text-align: left" />
                    <div class="editing-form-value-cell">
                        <cms:UniSelector ID="selEmailTemplate" runat="server" IsLiveSite="false" ObjectType="cms.emailtemplate"
                            SelectionMode="SingleTextBox" OrderBy="EmailTemplateDisplayName" ReturnColumnName="EmailTemplateName"
                            ResourcePrefix="emailtemplateselect" />
                    </div>
                </div>

            </ContentTemplate>
        </cms:CMSUpdatePanel>

        <div class="form-group">
            <div class="editing-form-value-cell">
                <asp:Button Text="Upload" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnUploadUserList" OnClick="btnUploadUserList_Click" runat="server" />
            </div>
        </div>

        <asp:PlaceHolder runat="server" ID="errorMessageContainer" Visible="false">
            <div class="alert-dismissable alert-error alert">
                <span class="alert-icon">
                    <i class="icon-times-circle"></i>
                    <span class="sr-only">Error</span>
                </span>
                <div class="alert-label">
                    <asp:Literal runat="server" ID="errorMessage"></asp:Literal></div>
            </div>
        </asp:PlaceHolder>
    </div>
    <script>
        window.document.getElementById('btnDownloadTemplate').addEventListener('click', function () {
            // hide loader
            window.setTimeout(function () { window.Loader.hide(); }, 2000);
        }, false);
    </script>
</asp:Content>