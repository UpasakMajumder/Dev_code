<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingListSelector.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.MailingListSelector" %>

<asp:Table runat="server" ID="tblMalilingList" CssClass="table product-editor__table-select" Visible="false">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell>Date Added</asp:TableHeaderCell>
        <asp:TableHeaderCell>Addresses</asp:TableHeaderCell>
        <asp:TableHeaderCell>Errors</asp:TableHeaderCell>
        <asp:TableHeaderCell>Valid through</asp:TableHeaderCell>
        <asp:TableHeaderCell>Status</asp:TableHeaderCell>
        <asp:TableHeaderCell></asp:TableHeaderCell>
    </asp:TableHeaderRow>
</asp:Table>
<asp:Panel runat="server" ID="pnlNewList" CssClass="product-editor__bottom">
    <div class="product-editor__left-text">
        <span>Missing the right mailing list? Go ahead and create it. This will save your current progress in Drafts section where you can access it through top navigation bar.</span> 
    </div>
    <div class="product-editor__right-btn-group btn-group btn-group--right">
        <a href="#" class="btn-action btn-action--secondary">New mailing list</a>
    </div>
</asp:Panel>
