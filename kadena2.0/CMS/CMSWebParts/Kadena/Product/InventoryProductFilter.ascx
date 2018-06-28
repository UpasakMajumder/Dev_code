<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InventoryProductFilter.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.InventoryProductFilter" %>
<div class="search__recent search__recent--icon">
    <asp:TextBox ID="txtProductSearch" runat="server" placeholder="Search" CssClass="input__text"></asp:TextBox>
    <button class="search__submit btn--off" type="submit">
        <svg class="icon icon-dollar">
            <use xlink:href="/gfx/svg/sprites/icons.svg#search"></use>
        </svg>
    </button>
</div>
