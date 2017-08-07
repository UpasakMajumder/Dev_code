<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddToCartEdit.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Chili.AddToCartExtended" %>

<div class="input__wrapper js-add-to-cart-error">
    <span class="input__label">
        <cms:LocalizedLiteral runat="server" ResourceString="Kadena.Product.AddToCartQuantity" EnableViewState="false" />
    </span>
    <input id="inpNumberOfItems" runat="server" type="number" class="input__text js-add-to-cart-error js-add-to-cart-quantity" value="1" enableviewstate="false">
    <span class="input__error input__error--noborder js-add-to-cart-message" />

</div>
<button type="button" class="btn-action js-chili-addtocart js-add-to-cart">
    <cms:LocalizedLiteral runat="server" ResourceString="Kadena.Product.AddToCart" EnableViewState="false" />
</button>

