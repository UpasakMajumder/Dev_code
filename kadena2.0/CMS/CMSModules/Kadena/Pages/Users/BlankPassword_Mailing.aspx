<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlankPassword_Mailing.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Users.BlankPassword_Mailing" 
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Users - Send E-mail"
    Theme="Default"%>

<%@ Register Src="~/CMSFormControls/Inputs/EmailInput.ascx" TagName="EmailInput"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>

<asp:Content ID="cntSite" ContentPlaceHolderID="plcSiteSelector" runat="server">
    <div class="form-horizontal form-filter">
        <div class="form-group">
            <div class="filter-form-label-cell">
                <cms:LocalizedLabel ID="lblSite" runat="server" CssClass="control-label" EnableViewState="false" ResourceString="general.site" DisplayColon="true" />
            </div>
            <div class="filter-form-value-cell-wide">
                <cms:SiteSelector ID="siteSelector" runat="server" IsLiveSite="false" AllowAll="true" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblFrom" runat="server" AssociatedControlID="emailSender" EnableViewState="false" ResourceString="general.fromemail" DisplayColon="true" ShowRequiredMark="True" />
            </div>
            <div class="editing-form-value-cell">
                <cms:EmailInput ID="emailSender" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblSubject" runat="server" AssociatedControlID="txtSubject" EnableViewState="false" ResourceString="general.subject" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextBox ID="txtSubject" runat="server" MaxLength="450" />
            </div>
        </div>
    </div>
    <cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-horizontal">
                <cms:LocalizedHeading Level="4" runat="server" EnableViewState="false"
                    ResourceString="general.message" />
                <asp:PlaceHolder runat="server" ID="plcText">
                    <div class="form-group">
                        <div class="editing-form-label-cell label-full-width">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblText" runat="server" EnableViewState="false"
                                ResourceString="general.text" DisplayColon="True" AssociatedControlID="htmlText" />
                        </div>
                        <div class="editing-form-value-cell textarea-full-width">
                            <cms:CMSHtmlEditor ID="htmlText" runat="server" Height="400px" />
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Content>