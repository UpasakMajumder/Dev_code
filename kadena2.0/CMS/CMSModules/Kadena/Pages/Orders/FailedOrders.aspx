<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="FailedOrders.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Orders.FailedOrders"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Order list" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">

    <asp:PlaceHolder runat="server" ID="messageContainer" Visible="false">
        <div class="alert-dismissable alert-info alert">
            <span class="alert-icon">
                <i class="icon-i-circle"></i>
            </span>
            <div class="alert-label">
                <asp:Literal runat="server" ID="message"></asp:Literal>
            </div>
        </div>
    </asp:PlaceHolder>

    <cms:UniGrid runat="server" ID="grdOrders">

        <GridOptions DisplayFilter="false" AllowSorting="false" />

        <PagerConfig DisplayPager="true" />

        <GridActions Width="100px">
            <%-- FailedOrders.ActionResubmit --%>
            <ug:Action Name="resubmitOrder" CommandArgument="Id" Caption="Resubmit order"
                FontIconClass="icon-rotate-right" FontIconStyle="Allow" />
        </GridActions>

        <GridColumns>
            <ug:Column runat="server" Wrap="false"
                Source="Id" Name="Id" Caption="Id" />
            <ug:Column runat="server" Wrap="false"
                Source="SiteName" Name="SiteName" Caption="Site" />
            <ug:Column runat="server" Wrap="false"
                Source="OrderDate" Name="OrderDate" Caption="Submission date" />
            <ug:Column runat="server" Wrap="false"
                Source="TotalPrice" Name="TotalPrice" Caption="Total price" />
            <ug:Column runat="server" Wrap="false"
                Source="SubmissionAttemptsCount" Name="SubmissionAttemptsCount" Caption="Submission attempts" />
            <ug:Column runat="server" Wrap="false"
                Source="OrderStatus" Name="OrderStatus" Caption="Order status" />
        </GridColumns>

    </cms:UniGrid>
</asp:Content>
