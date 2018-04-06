<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="~/CMSModules/Kadena/Pages/Products/CampaignProductsMigration.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Products.CampaignProductsMigration"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Campaign Products Migration" Theme="Default" %>

<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/MediaLibrary/FormControls/MediaLibrarySelector.ascx" TagPrefix="cms" TagName="MediaLibrarySelector" %>


<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">

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

        <!-- Product block -->
        <h4 class="anchor">Move SKU Image To Product Image</h4>
        <div class="form-group" style="margin-bottom: 3rem">
            <asp:Button Text="Move" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnMoveSKUToPageType"
                OnClick="btnMoveSKUToPageType_Click" runat="server" />
        </div>

        <h4 class="anchor">Migrate To S3 Images and move to Product Image</h4>
        <h4>Select target media library to store S3 images</h4>
        <div class="form-group">
            <div class="editing-form-value-cell">
                <div class="editing-form-control-nested-control">
                    <div class="control-group-inline">
                        <cms:MediaLibrarySelector runat="server" ID="MediaLibrarySelector" AllowEmpty="false" />
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="editing-form-value-cell">
                <asp:Button Text="Move to S3" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnMoveToS3"
                    OnClick="btnMoveToS3_Click" runat="server" />
            </div>
        </div>

    </div>
</asp:Content>
