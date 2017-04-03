<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit_Hierarchy.aspx.cs" Inherits="CMSApp.CMSModules.Kadena.Pages.Users.User_Edit_Hierarchy"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Theme="Default" Title="User Edit - Hierarchy" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector"
    TagPrefix="cms" %>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="plcContent">
    <div class="form-horizontal">
        <cms:LocalizedHeading ID="headParent" runat="server" EnableViewState="false" Level="4" CssClass="listing-title" ResourceString="Kadena.ParentUsers" />
        <cms:UniSelector ID="selParent" runat="server" IsLiveSite="false" ObjectType="cms.user" SelectionMode="Multiple" ValuesSeparator=","/>
    </div>
    <div class="form-horizontal">
        <cms:LocalizedHeading ID="headChild" runat="server" EnableViewState="false" Level="4" CssClass="listing-title" ResourceString="Kadena.ChildUsers" />
        <cms:UniSelector ID="selChild" runat="server" IsLiveSite="false" ObjectType="cms.user" SelectionMode="Multiple" ValuesSeparator=","/>
    </div>
</asp:Content>
