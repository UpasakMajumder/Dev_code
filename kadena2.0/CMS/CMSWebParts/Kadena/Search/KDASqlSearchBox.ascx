<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Kadena_Search_KDASqlSearchBox" CodeBehind="~/CMSWebParts/Kadena/Search/KDASqlSearchBox.ascx.cs" %>
<asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnImageButton" CssClass="searchBox">
    <asp:Label ID="lblSearch" runat="server" AssociatedControlID="txtWord" EnableViewState="false" Style="display: none;" />
    <cms:CMSTextBox ID="txtWord" runat="server" ClientIDMode="Static"/>
    <cms:CMSButton ID="btnGo" runat="server" OnClick="btnGo_Click" EnableViewState="false" ButtonStyle="Default" Style="display: none;" />
    <asp:ImageButton ID="btnImageButton" runat="server" Visible="false" OnClick="btnImageButton_Click" EnableViewState="false" />
</asp:Panel>
