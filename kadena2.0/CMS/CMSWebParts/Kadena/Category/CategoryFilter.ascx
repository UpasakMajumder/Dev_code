<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryFilter.ascx.cs" Inherits="CMSApp.CMSWebParts.Kadena.Category.CategoryFilter" %>
<div class="search_block">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="input__text" AutoPostBack="true" ></asp:TextBox>
    
      <cms:LocalizedButton ID="btnFilter" runat="server" Text="Apply Filter" style="visibility: hidden; display: none;"/>
      </div>


 