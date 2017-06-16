<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddToCartEdit.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Chili.AddToCartExtended" %>

<asp:UpdatePanel ID="updatePanelId" runat="server" class="input__wrapper">
    <ContentTemplate>     
    <asp:Label ID="lblQuantity" runat="server" CssClass="input__label" />    
    <input id="inpNumberOfItems" runat="server" type="number" class="input__text"  value="0" enableviewstate="false">  
    <asp:Label ID="lblNumberOfItemsError" runat="server" CssClass="input__error input__error--noborder" Visible="false" EnableViewState="false" />

    <asp:Button ID="btnAddToCart" runat="server" OnClick="btnAddToCart_Click" CssClass="btn-action btn-action" />
    </ContentTemplate>
</asp:UpdatePanel>

