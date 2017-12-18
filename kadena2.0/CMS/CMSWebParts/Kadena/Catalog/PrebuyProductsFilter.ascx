<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Catalog_PrebuyProductsFilter"  CodeBehind="~/CMSWebParts/Kadena/Catalog/PrebuyProductsFilter.ascx.cs" %>
<div class="custom__block">
    <div class="custom__select">
        <asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlProductTypes" runat="server" OnSelectedIndexChanged="ddlProductTypes_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </div>
</div>