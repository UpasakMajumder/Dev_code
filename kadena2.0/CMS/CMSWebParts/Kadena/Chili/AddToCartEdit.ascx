<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddToCartEdit.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Chili.AddToCartExtended" %>

<asp:UpdatePanel ID="updatePanelId" runat="server" class="input__wrapper">
    <ContentTemplate>
      
    <asp:Label ID="lblQuantity" runat="server" CssClass="input__label" />
    <asp:Label ID="lblNumberOfItemsError" runat="server" CssClass="input__error input__error--noborder" Visible="false" />
    <input id="inpNumberOfItems" runat="server" type="number" class="input__text"  disabled="disabled" value="0">  

<asp:Button ID="btnAddToCart" runat="server" OnClick="btnAddToCart_Click" CssClass="btn-action btn-action" />
    </ContentTemplate>
</asp:UpdatePanel>

