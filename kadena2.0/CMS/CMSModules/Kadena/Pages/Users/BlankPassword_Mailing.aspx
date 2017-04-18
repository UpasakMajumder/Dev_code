<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlankPassword_Mailing.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Users.BlankPassword_Mailing"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Users - Send E-mail"
    Theme="Default" %>

<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel ID="lblSite" runat="server" CssClass="control-label" EnableViewState="false"
                    ResourceString="general.site" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:SiteSelector ID="siteSelector" runat="server" IsLiveSite="false" AllowAll="true" />
            </div>
        </div>
        <cms:CMSUpdatePanel ID="pnlTemplate" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblEmailTemplate" runat="server" EnableViewState="false"
                            ResourceString="Kadena.Email.SelectTemplate" DisplayColon="true" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:UniSelector ID="usBlankPasswords" runat="server" IsLiveSite="false" ObjectType="cms.emailtemplate"
                            SelectionMode="SingleTextBox" OrderBy="EmailTemplateDisplayName" ReturnColumnName="EmailTemplateName"
                            ResourcePrefix="emailtemplateselect" AllowEmpty="true" AllowEditTextBox="false" />
                    </div>
                </div>
            </ContentTemplate>
        </cms:CMSUpdatePanel>
    </div>
</asp:Content>
