<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemoveItemFromCart.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.RemoveItemFromCart" %>
<%@ Register TagPrefix="cms" TagName="UniButton" Src="~/CMSAdminControls/UI/UniControls/UniButton.ascx" %>
<div class="webform_view">
  <%--  <cms:unibutton id="btnRemove"  cssclass="fa fa-trash delete_btn" runat="server" onclick="Remove" />--%>
    <asp:LinkButton id="btnRemoveItem"   cssclass="fa fa-trash delete_btn" runat="server" onclick="Remove"></asp:LinkButton>
</div>
