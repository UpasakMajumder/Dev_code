<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Global_Admin_Dashboard_GlobalAdminDashboard" CodeBehind="~/CMSWebParts/Kadena/Global Admin Dashboard/GlobalAdminDashboard.ascx.cs" %>
<script type="text/javascript" src="~/CMSScripts/kadena/KadenaExtension.js"></script>
<div class="Summary_content">
    <span><label runat="server" id="lblTimeHeader"></label></span>
</div>
<table class="show-table">
    <tbody>
        <tr>
            <th></th>
            <th><cms:LocalizedLabel runat="server" ID="lblWeekHeading" ResourceString="KDA.Dashboard.WeekHeading"></cms:LocalizedLabel></th>
            <th><cms:LocalizedLabel runat="server" ID="lblMonthHeading" ResourceString="KDA.Dashboard.WeekHeading"></cms:LocalizedLabel></th>
            <th><cms:LocalizedLabel runat="server" ID="lblYearHeading" ResourceString="KDA.Dashboard.WeekHeading"></cms:LocalizedLabel></th>
        </tr>
        <tr>
            <td><cms:LocalizedLabel runat="server" ID="lblOpenordersHeading" ResourceString="KDA.Dashboard.OpenOrdersSideHeading"></cms:LocalizedLabel></td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><a href="#"><label runat="server"  id="lblCurrentWeekOpenOrder">0</label></a></div>
                    <div class="summ_right"><a href="#"><label runat="server" id="lblCurrentWeekOpenMoney">0</label></a></div>
                </div>
            </td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><a href="#"><label id="lblCurrentMonthOpenOrder" runat="server">0</label></a></div>
                    <div class="summ_right"><a href="#"><label id="lblCurrentMonthOpenMoney" runat="server">0</label></a></div>
                </div>
            </td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><a href="#"><label id="lblCurrentYearOpenOrdersCount" runat="server">0</label></a></div>
                    <div class="summ_right"><a href="#"><label id="lblCurrentYearOpenOrdersMoney" runat="server">0</label></a></div>
                </div>
            </td>
        </tr>
        <tr>
            <td><cms:LocalizedLabel runat="server" ID="LocalizedLabel2" ResourceString="KDA.Dashboard.OrdersPlacedHeading"></cms:LocalizedLabel></td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><a href="#"><label id="lblCurrentWeekOrdersPlacedCount" runat="server">0</label></a></div>
                    <div class="summ_right"><span><label id="lblCurrentWeekOrdersPlacedMoney" runat="server">0</label></span></div>

                </div>
            </td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><a href="#"><label id="lblCurrentMonthOrdersPlacedCount" runat="server">0</label></a></div>
                    <div class="summ_right">
                        <span><label id="lblCurrentMonthOrdersPlacedMoney" runat="server">0</label></span>
                    </div>
                </div>
            </td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><a href="#"><label id="lblcurrentYearordersPlacedMoneyCount" runat="server">0</label></a></div>
                    <div class="summ_right"><span><label id="lblcurrentYearordersPlacedMoney" runat="server">0</label></span></div>
                </div>
            </td>
        </tr>
        <tr>
            <td><cms:LocalizedLabel runat="server" ID="lbluserCount" ResourceString="KDA.Dashboard.UsersHeading"></cms:LocalizedLabel></td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><span><label runat="server" id="lblCurrentWeekUserCount">0</label></span></div>
                    <div class="summ_right"><span>&nbsp;</span></div>
                </div>
            </td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><span><label runat="server" id="lblCurrentMonthUserCount">0</label></span></div>
                    <div class="summ_right"><span>&nbsp;</span></div>
                </div>
            </td>
            <td>
                <div class="summary_block">
                    <div class="summ_left"><span><label runat="server" id="lblCurrentYearUserCount">0</label></span></div>
                    <div class="summ_right"><span>&nbsp;</span></div>
                </div>
            </td>
        </tr>
    </tbody>
</table>
