<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Membership_Users_UsersFilterControl" CodeBehind="~/CMSWebParts/Kadena/Membership/Users/UsersFilterControl.ascx.cs" %>
<asp:Panel CssClass="Filter" DefaultButton="btnSelect" runat="server" ID="pnlUsersFilter">
    <span class="FilterSort" style="display: none;">
        <asp:Label runat="server" ID="lblSortBy" EnableViewState="false" />
        <asp:LinkButton runat="server" ID="lnkSortByUserName" OnClick="lnkSortByUserName_Click"
            EnableViewState="false" />
        <asp:LinkButton runat="server" ID="lnkSortByActivity" OnClick="lnkSortByActivity_Click"
            EnableViewState="false" />
    </span><span class="search__block">
        <cms:LocalizedLabel ID="lblValue" runat="server" EnableViewState="false" AssociatedControlID="txtValue"
            Display="false" ResourceString="general.searchexpression" />
        <cms:CMSTextBox runat="server" ID="txtValue" EnableViewState="false" CssClass="input__text" />
        <cms:CMSButton runat="server" ID="btnSelect" OnClick="btnSelect_Click" EnableViewState="false" ButtonStyle="Default" Style="display: none;" />
    </span>
    <div class="add__btn">
        <a href="?userid=-1" class="btn-action">
            <cms:LocalizedLiteral runat="server" ID="ltrNewUser"></cms:LocalizedLiteral>
        </a>
    </div>
</asp:Panel>
