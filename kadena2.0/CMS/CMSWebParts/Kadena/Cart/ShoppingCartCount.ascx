<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartCount.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.ShoppingCartCount" %>
<li class="nav-item ">
    <a class="nav-link" runat="server" id="linkCheckoutPage">
        <svg class="icon icon-navigation icon-dashboard">
            <use xlink:href="/gfx/svg/sprites/icons.svg#cart" />
        </svg>
        <asp:Label runat="server" ID="lblCartName"></asp:Label>
        <div data-reactroot="" class="nav-badge">
            <asp:Label runat="server" CssClass="nav-badge__text" ID="lblCount"></asp:Label>
        </div>
    </a>
</li>