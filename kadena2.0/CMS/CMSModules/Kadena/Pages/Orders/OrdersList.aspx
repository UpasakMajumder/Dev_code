<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdersList.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Orders.OrdersList"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Order list" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UniGrid runat="server" ID="grdOrders">
        <GridColumns>
        </GridColumns>
    </cms:UniGrid>
</asp:Content>
