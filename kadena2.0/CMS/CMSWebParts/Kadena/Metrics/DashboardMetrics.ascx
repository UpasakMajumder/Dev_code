<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardMetrics.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Metrics.DashboardMetrics" %>

<div class="dashboard__block">
    <div class="sent-statistics">
        <div class="sent-statistics__item">
            <p class="sent-statistics__title">
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.DashboardMetrics.OrdersToDatePerYear" />
            </p>
            <p class="sent-statistics__value">
                <asp:Literal ID="ltlOrdersToDatePerYear" runat="server" />
            </p>
        </div>
        <div class="sent-statistics__item">
            <p class="sent-statistics__title">
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.DashboardMetrics.TotalProductsAvailable" />
            </p>
            <p class="sent-statistics__value">
                <asp:Literal ID="ltlTotalProductsAvailable" runat="server" />
            </p>
        </div>
        <div class="sent-statistics__item">
            <p class="sent-statistics__title">
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.DashboardMetrics.NumberOfUsers" />
            </p>
            <p class="sent-statistics__value">
                <asp:Literal ID="ltlNumberOfusers" runat="server" />
            </p>
        </div>
        <div class="sent-statistics__item">
            <p class="sent-statistics__title">
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.DashboardMetrics.AverageProductionTime" />
            </p>
            <p class="sent-statistics__value">
                <asp:Literal ID="ltlAverageProductionTime" runat="server" />
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.DashboardMetrics.Days" />
            </p>
        </div>
    </div>
</div>
