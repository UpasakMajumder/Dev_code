<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddToCartButton.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.AddToCartButton" %>

<div class="input__wrapper">
    <span runat="server" id="txtQuantity" class="input__label">
        <cms:LocalizedLiteral runat="server" ResourceString="Kadena.Product.AddToCartQuantity" EnableViewState="false" />
    </span>
    <input id="inpNumberOfItems" runat="server" type="number" class="input__text js-add-to-cart-error js-add-to-cart-quantity" value="1" min="1">
    <span id="lblNumberOfItemsError" class="input__error input__error--noborder js-add-to-cart-message" />
</div>

<asp:Label ID="lblNumberOfItemsInPackageInfo" runat="server" EnableViewState="false" CssClass="add-to-cart__right-label" />

<button type="button" class="btn-action js-chili-addtocart js-add-to-cart">
    <cms:LocalizedLiteral runat="server" ResourceString="Kadena.Product.AddToCart" EnableViewState="false" />
</button>
