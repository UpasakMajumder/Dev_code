<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ProductImagesImport.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Products.ProductImagesImport"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Products import" Theme="Default" %>

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

        <div class="form-group" style="margin-bottom: 3rem">
            <label class="control-label" for="siteSelector" style="text-align: left">Product Page Type:</label>
            <cms:CMSDropDownList runat="server" ID="ddlProductPageType">
                <asp:ListItem Value="KDA.Product" Text="KDA Product"></asp:ListItem>
                <asp:ListItem Value="KDA.CampaignsProduct" Text="KDA Campaign Product"></asp:ListItem>
            </cms:CMSDropDownList>
        </div>

        <h4>Download template sheet for Product images</h4>
        <div class="form-group" style="margin-bottom: 3rem">
            <asp:Button Text="Download" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnDownloadTemplate" OnClick="btnDownloadTemplate_Click" runat="server" />
        </div>

        <h4>Upload sheet with Product images</h4>
        <div class="form-group">
            <div class="editing-form-value-cell">
                <div class="editing-form-control-nested-control">
                    <div class="control-group-inline">
                        <label class="control-label" style="text-align: left">File:</label>
                        <input type="text" id="importFileName" class="form-control" readonly="readonly" />
                        <label for="importFile" class="btn btn-default">Select file (XLS, XLSX)</label>
                        <br />
                        <asp:FileUpload ClientIDMode="Static" ID="importFile" name="importFile" accept=".xls, .xlsx" Style="display: none" onchange="onImportFileSelected(event)" runat="server" />                       
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="editing-form-value-cell">
                <asp:Button Text="Upload" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnUploadProductList" OnClick="btnUploadProductList_Click" runat="server" />
            </div>
        </div>

        <asp:PlaceHolder runat="server" ID="errorMessageContainer" Visible="false">
            <div class="alert-dismissable alert-error alert">
                <span class="alert-icon">
                    <i class="icon-times-circle"></i>
                    <span class="sr-only">Error</span>
                </span>
                <div class="alert-label">
                    <asp:Literal runat="server" ID="errorMessage"></asp:Literal>
                </div>
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="successMessageContainer" Visible="false">
            <div class="alert-dismissable alert-info alert">
                <span class="alert-icon">
                    <i class="icon-check-circle"></i>
                    <span class="sr-only">OK</span>
                </span>
                <div class="alert-label">
                    <asp:Literal runat="server" ID="successMessage"></asp:Literal>
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
    <script>
        window.document.getElementById('btnDownloadTemplate').addEventListener('click', function () {
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
</asp:Content>