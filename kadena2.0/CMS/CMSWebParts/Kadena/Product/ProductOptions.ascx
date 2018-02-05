<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductOptions.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.ProductOptions" %>


<div class="js-product-options product-options product-options--column" data-price-element='#<% =PriceElementName %>' data-url='<% =PriceUrl %>'>
    <asp:PlaceHolder runat="server" ID="phSelectors" />
</div>
