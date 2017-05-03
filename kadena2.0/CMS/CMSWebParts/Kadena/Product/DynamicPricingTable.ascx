<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicPricingTable.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.DynamicPricingTable" %>

<table class="table">
    <thead>
        <tr>
            <th colspan="2">
                <svg class="icon icon-dollar">
                    <use xlink:href="/gfx/svg/sprites/icons.svg#money" />
                </svg>                
                <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.Product.UnitPrice" />
            </th>
        </tr>
    </thead>
    <tbody>
        <asp:Literal ID="ltlTableContent" runat="server" EnableViewState="false" />
    </tbody>
</table>
