<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Category" CodeBehind="~/CMSWebParts/Kadena/Category.ascx.cs" %>
<div class="css-login changepwd_block">
    <div class="form">
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblName" runat="server" CssClass="input__label" ResourceString="Kadena.Categoryform.lblName" />
                <cms:CMSTextBox ID="txtName" runat="server" MaxLength="100" CssClass="input__text" />
                <cms:CMSRequiredFieldValidator ID="rfvUserNameRequired" runat="server" ControlToValidate="txtName" CssClass="EditingFormErrorLabel"  ValidationGroup="feildvalidation" />
                <asp:RegularExpressionValidator  ControlToValidate="txtName" ID="revName" CssClass="EditingFormErrorLabel" ValidationExpression="^[\s\S]{0,50}$" runat="server" ValidationGroup="feildvalidation"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblDecription" runat="server" CssClass="input__label" ResourceString="Kadena.Categoryform.DesText" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtDescription" runat="server" MaxLength="100" TextMode="MultiLine" CssClass="input__textarea" />
                    <asp:RegularExpressionValidator CssClass="EditingFormErrorLabel"   ControlToValidate="txtDescription" ID="revDescription" ValidationExpression="^[\s\S]{0,50}$" ValidationGroup="feildvalidation" runat="server"></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
          <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblStatus" runat="server" CssClass="input__label" ResourceString="Kadena.Categoryform.StatusText" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlStatus" runat="server" EnableViewState="True" CssClass="input__select"></cms:CMSDropDownList>
                </div>
            </div>
        </div>
    </div>

    <div class="mb-3 form__btns">
        <div class="">

            <cms:LocalizedButton ID="btnSave" ValidationGroup="feildvalidation" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary" EnableViewState="false"
                ResourceString="Kadena.CategroyForm.SaveText" />
            <cms:LocalizedButton ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" OnClick="btnCancel_Cancel" runat="server" ButtonStyle="Primary" CommandName="Login" EnableViewState="false"
                ResourceString="Kadena.CategroyForm.CancelButton" />

        </div>
        <cms:LocalizedLabel ID="lblSuccessMsg" Visible="false" runat="server" CssClass="input__label" EnableViewState="False" ResourceString="Kadena.CategroyForm.SaveMsg" />
        <cms:LocalizedLabel ID="lblFailureText" runat="server"  EnableViewState="False" CssClass="error-label EditingFormErrorLabel" Visible="false" ResourceString="Kadena.CategoryForm.FailureMsg" ForeColor="Red" />
    </div>

</div>


