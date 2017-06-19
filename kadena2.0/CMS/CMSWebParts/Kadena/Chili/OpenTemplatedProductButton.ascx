<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenTemplatedProductButton.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Chili.OpenTemplatedProductButton" %>

<div class="input__wrapper">
    <asp:Button ID="btnOpenTemplatedProduct" runat="server" CssClass="btn-action btn-action" OnClick="btnOpenTemplatedProduct_Click" />

    <span id="spanErrorMessage" runat="server" enableviewstate="false" class="input__error input__error--noborder" style="display:block;" visible="false">
        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OpenTemplatedProductButton.TemplateServiceError" />
    </span>
</div>
