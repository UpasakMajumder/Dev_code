<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Kadena_Membership_KadenaChangePassword" CodeBehind="~/CMSWebParts/Kadena/Membership/KadenaChangePassword.ascx.cs" %>

<%@ Register Src="~/CMSModules/Membership/FormControls/Passwords/PasswordStrength.ascx" TagName="PasswordStrength"
    TagPrefix="cms" %>

<asp:Panel ID="pnlWebPart" runat="server" DefaultButton="btnChangePassword" CssClass="change-password">
    <asp:Label runat="server" ID="lblInfo" CssClass="InfoLabel" EnableViewState="false"
        Visible="false" />
    <asp:Label runat="server" ID="lblError" CssClass="EditingFormErrorLabel" EnableViewState="false"
        Visible="false" />
    <div class="form">
        <div class="mb-2">
            <div class="input__wrapper">
                    <asp:Label CssClass="input__label" ID="lblOldPassword" AssociatedControlID="txtOldPassword" runat="server" />
                    <cms:CMSTextBox ID="txtOldPassword" runat="server" TextMode="Password" CssClass="input__text" />
                    <asp:RequiredFieldValidator ID="rvOldPassword" runat="server" ControlToValidate="txtOldPassword" CssClass="EditingFormErrorLabel"></asp:RequiredFieldValidator>
                </div>
        </div>

        <div class="mb-2">
            <div class="input__wrapper">
                    <cms:LocalizedLabel CssClass="input__label" ID="lblNewPassword" runat="server" />
                    <cms:PasswordStrength runat="server" ID="passStrength" TextBoxClass="input__text" AllowEmpty="true" />
                    <asp:RequiredFieldValidator ID="rvPasswordStrength" runat="server" ControlToValidate="passStrength" CssClass="EditingFormErrorLabel"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="passStrength" CssClass="EditingFormErrorLabel" ValidationExpression="(?=^.{8,50}$)((?=.*\d)|(?=.*[!@#$%^&*]+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="mb-2">
            <div class="input__wrapper">
                    <asp:Label CssClass="input__label" ID="lblConfirmPassword" AssociatedControlID="txtConfirmPassword" runat="server" />
                    <cms:CMSTextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="input__text" />
                    <asp:RequiredFieldValidator runat="server" ID="rvConfirmPassword" ControlToValidate="txtConfirmPassword" CssClass="EditingFormErrorLabel"></asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ID="cvConfirmPassword" CssClass="EditingFormErrorLabel" ControlToValidate="txtConfirmPassword" ControlToCompare="passStrength"></asp:CompareValidator>
            </div>
        </div>
    </div>
    <div class="mb-3 form__btns">
        <div class="">
            <cms:LocalizedLinkButton ID="btnChangePassword" runat="server" OnClick="btnOk_Click" CssClass="btn-action login__login-button btn--no-shadow" Text="Kadena.MyAccount.ChangePassword" ValidationGroup="PasswordChange"></cms:LocalizedLinkButton>
        </div>
    </div>


</asp:Panel>
