<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Brand_Brand" CodeBehind="~/CMSWebParts/Kadena/Brand/Brand.ascx.cs" %>
<cms:DataForm ID="form" runat="server" IsLiveSite="true" ValidateRequestMode="Enabled" DefaultFormLayout="SingleTable" Visible="true"/>
<div class="mb-3 form_btns">
    <div>
        <cms:LocalizedLinkButton runat="server" ID="btnSave" CssClass="btn-action login__login-button btn--no-shadow" Text="Save" OnClick="btnSave_Click"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" Text="Cancel" OnClick="btnCancel_Click"></cms:LocalizedLinkButton>
    </div>
</div>
