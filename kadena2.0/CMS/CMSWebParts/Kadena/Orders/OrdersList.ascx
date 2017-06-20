<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdersList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Orders.OrdersList" %>

<asp:Repeater ID="repOrderList" runat="server" EnableViewState="false">
    <HeaderTemplate>
        <table class="show-table">
            <tbody>
                <tr>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderNumber" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderDate" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderedItems" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderStatus" />
                    </th>
                    <th></th>
                </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <%# Eval("id") %>                
            </td>
            <td>
                <%# Eval<DateTime>("createDate").ToString("MMM dd yyyy") %>
            </td>
            <td class="show-table__text-appear">
                <%# Eval("ItemsString") %>
            </td>
            <td class="show-table__will-hide">
                <%# Eval("status") %>
            </td>
            <td class="show-table__will-appear">
                <a href="/recent-orders/order-detail?orderID=<%# Eval("id") %>" target="_blank" class="btn-action">
                    <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.View" />
                </a>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
</table>
    </FooterTemplate>
</asp:Repeater>

<cms:LocalizedLabel ID="lblNoOrderItems" runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.NoOrderItems" Visible="false" />

<%--<asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
    <HeaderTemplate>
        <table class="show-table">
            <tbody>
                <tr>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderDate" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderedItems" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderStatus" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon" style="display: none;">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.DeliveryDate" />
                    </th>
                    <th></th>
                </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <%# Eval<DateTime>("createDate").ToString("MMM dd yyyy") %>
            </td>
            <td class="show-table__text-appear">
                <%# Eval("ItemsString") %>
            </td>
            <td class="show-table__will-hide">
                <%# Eval("status") %>
            </td>
            <td class="show-table__will-hide">
                <%# Eval<DateTime>("deliveryDate").ToString("MMM dd yyyy") %>
            </td>
            <td class="show-table__will-appear">
                <button type="button" class="btn-action" style="display:none;">
                    <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.View" />
                </button>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
</table>
    </FooterTemplate>
</asp:Repeater>--%>
