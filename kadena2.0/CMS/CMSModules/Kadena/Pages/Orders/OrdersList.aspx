<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdersList.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Orders.OrdersList"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Order list" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UniGrid runat="server" ID="grdOrders">
        <GridColumns>
            <ug:Column Name="IDAndInvoice" Source="OrderNumber" 
                Caption="$Kadena.OrdersList.OrderNumber$" Wrap="false" />
            <ug:Column Name="CustomerName"
                Caption="$Kadena.OrdersList.CustomerName$" Wrap="false" />
            <ug:Column Name="Created"
                Caption="$Kadena.OrdersList.CreatedDate$" Wrap="false" />
            <ug:Column Name="TotalPrice"
                Caption="$Kadena.OrdersList.TotalPrice$" Wrap="false" />
            <ug:Column Name="Status"
                Caption="$Kadena.OrdersList.OrderStatus$" Wrap="false" />
        </GridColumns>
    </cms:UniGrid>
</asp:Content>
