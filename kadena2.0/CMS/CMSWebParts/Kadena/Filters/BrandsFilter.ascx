<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BrandsFilter.ascx.cs" Inherits="CMSApp.CMSWebParts.Kadena.Filters.BrandsFilter" %>

    <%--<input type="text" class="input__text" placeholder="Search" value="">--%>
    <asp:TextBox runat="server" ID="txtSearchBrand" CssClass="input__text" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txtSearchBrand_TextChanged"></asp:TextBox>


