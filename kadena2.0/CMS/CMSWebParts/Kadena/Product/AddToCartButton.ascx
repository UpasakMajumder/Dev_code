<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddToCartButton.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.AddToCartButton" %>

<div class="input__wrapper">
    <input id="inpNumberOfItems" runat="server" type="number" class="input__text js-add-to-cart-error js-add-to-cart-quantity" value="1">
    <asp:Label ID="lblNumberOfItemsError" runat="server" CssClass="input__error input__error--noborder js-add-to-cart-message" Visible="false" />
</div>

<asp:Label ID="lblNumberOfItemsInPackageInfo" runat="server" EnableViewState="false" CssClass="add-to-cart__right-label" />

<asp:Button ID="btnAddToCart" runat="server" OnClick="btnAddToCart_Click" CssClass="btn-action js-add-to-cart" />
