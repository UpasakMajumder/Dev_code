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