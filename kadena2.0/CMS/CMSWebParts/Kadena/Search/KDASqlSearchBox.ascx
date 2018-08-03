<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Kadena_Search_KDASqlSearchBox" CodeBehind="~/CMSWebParts/Kadena/Search/KDASqlSearchBox.ascx.cs" %>
<asp:Panel ID="pnlSearch" runat="server"  
    CssClass="search__recent search__recent--icon searchBox">
    <asp:Label ID="lblSearch" runat="server" AssociatedControlID="txtWord" EnableViewState="false" Style="display: none;" />
    <cms:CMSTextBox ID="txtWord" runat="server" ClientIDMode="Static" CssClass="input__text" />

    <button id="btnSearch" runat="server" onserverclick="btnSearch_ServerClick" class="search__submit btn--off">
        <svg class="icon icon-dollar">
            <use xlink:href="/gfx/svg/sprites/icons.svg#search"></use>
        </svg>
    </button>
</asp:Panel>
