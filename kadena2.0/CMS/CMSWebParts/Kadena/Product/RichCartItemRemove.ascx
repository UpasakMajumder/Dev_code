<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_Checkout_RichCartItemRemove" CodeBehind="RichCartItemRemove.ascx.cs" %>
<%@ Register TagPrefix="cms" TagName="UniButton" Src="~/CMSAdminControls/UI/UniControls/UniButton.ascx" %>

<button type="button" class="cart-product__btn" runat="server" onserverclick="Remove">
    <svg class="icon ">
        <use xlink:href="/gfx/svg/sprites/icons.svg#cross--dark" />
    </svg>
    Remove
</button>
