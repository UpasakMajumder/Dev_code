<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdersList.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Orders.OrdersList"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Order list" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UniGrid runat="server" ID="grdOrders">
        <GridColumns>
            <ug:Column runat="server" Name="Id" Wrap="false"
                Source="Id" 
                Caption="$Kadena.OrdersList.OrderNumber$" />
            <ug:Column runat="server" Name="CustomerName" Wrap="false"
                Source="CustomerName"
                Caption="$Kadena.OrdersList.CustomerName$" />
            <ug:Column runat="server" Name="Created" Wrap="false"
                Source="CreateDate"
                Caption="$Kadena.OrdersList.CreatedDate$" />
            <ug:Column runat="server" Name="TotalPrice" Wrap="false"
                Source="TotalPrice"
                Caption="$Kadena.OrdersList.TotalPrice$" />
            <ug:Column runat="server" Name="Status" Wrap="false"
                Source="Status"
                Caption="$Kadena.OrdersList.OrderStatus$" />
        </GridColumns>
    </cms:UniGrid>
</asp:Content>
