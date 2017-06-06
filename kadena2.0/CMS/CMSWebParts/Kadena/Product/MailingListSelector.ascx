<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingListSelector.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.MailingListSelector" %>

<asp:Table runat="server" ID="tblMalilingList" CssClass="table product-editor__table-select" Visible="false">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.MailingName" />
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DateAdded" />
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.NumberOfAddresses" />
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.NumberOfErrors" />
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.ValidThrough" />
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.Status" />
        </asp:TableHeaderCell>
        <asp:TableHeaderCell></asp:TableHeaderCell>
    </asp:TableHeaderRow>
</asp:Table>
<asp:Panel runat="server" ID="pnlNewList" CssClass="product-editor__bottom">
    <div class="product-editor__left-text">
        <span>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.MissingMailingList" />
        </span>
    </div>
    <div class="product-editor__right-btn-group btn-group btn-group--right">
        <a href="<% =NewMailingListUrl %>" class="btn-action btn-action--secondary">
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.Create" />
        </a>
    </div>
</asp:Panel>
