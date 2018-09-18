<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="MigrationTool.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Deployment.MigrationTool" 
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Theme="Default"%>
<%@ Register Src="~/CMSAdminControls/AsyncLogDialog.ascx" TagName="AsyncLog"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
   <asp:Panel runat="server" ID="pnlLog" Visible="false">
        <cms:AsyncLog ID="ctlAsyncLog" runat="server" ProvideLogContext="true" />
    </asp:Panel>
</asp:Content>
