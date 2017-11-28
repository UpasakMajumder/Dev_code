<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="POSFilter.ascx.cs" Inherits="CMSApp.CMSWebParts.Kadena.POS.POSFilter" %>
<div class="search_block">
    <asp:TextBox ID="txtPOSSearch" runat="server" CssClass="input__text" AutoPostBack="true" ></asp:TextBox>
    
      <cms:LocalizedButton ID="btnFilter" runat="server" Text="Apply Filter" style="visibility: hidden; display: none;"/>
      </div>