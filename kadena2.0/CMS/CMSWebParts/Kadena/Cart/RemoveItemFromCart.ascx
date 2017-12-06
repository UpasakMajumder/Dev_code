<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemoveItemFromCart.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.RemoveItemFromCart" %>
<%@ Register TagPrefix="cms" TagName="UniButton" Src="~/CMSAdminControls/UI/UniControls/UniButton.ascx" %>
<div class="webform_view">
    <asp:LinkButton ID="btnRemoveItem" CssClass="fa fa-trash delete_btn" runat="server" OnClick="Remove"></asp:LinkButton>
</div>