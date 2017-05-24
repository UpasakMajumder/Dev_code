<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Forms_CustomerShipToAddress" CodeBehind="CustomerShipToAddress.ascx.cs" %>
<asp:RadioButtonList runat="server" OnSelectedIndexChanged="UpdateCartAddress" ID="hiddenAddressesList" RepeatLayout="Flow" name="hiddenAddressesList" AutoPostBack="true" Visible="true"  ClientIDMode="Static"/>
<asp:Literal runat="server" ID="htmlContent" EnableViewState="false" />
