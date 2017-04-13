<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlankPassword_Mailing.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Users.BlankPassword_Mailing"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Users - Send E-mail"
    Theme="Default" %>

<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/EmailTemplates/FormControls/EmailTemplateSelector.ascx"
    TagName="EmailTemplateSelector" TagPrefix="cms" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel ID="lblSite" runat="server" CssClass="control-label" EnableViewState="false" ResourceString="general.site" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:SiteSelector ID="siteSelector" runat="server" IsLiveSite="false" AllowAll="true" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblEmailTemplate" runat="server" EnableViewState="false"
                    ResourceString="workflow.customtemplate" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:EmailTemplateSelector ID="etBlankPasswords" runat="server" IsLiveSite="false" />
            </div>
        </div>
    </div>
</asp:Content>
