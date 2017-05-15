<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_Checkout_RichCartItemRemove" CodeBehind="RichCartItemRemove.ascx.cs" %>
<%@ Register TagPrefix="cms" TagName="UniButton" Src="~/CMSAdminControls/UI/UniControls/UniButton.ascx" %>

<cms:UniButton ID="btnRemove" CssClass="cart-product__btn" runat="server" OnClick="Remove" />