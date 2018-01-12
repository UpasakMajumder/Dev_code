<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="~/CMSModules/Membership/FormControls/Passwords/KadenaPasswordStrength.ascx.cs"
    Inherits="CMSModules_Membership_FormControls_Passwords_KadenaPasswordStrength" %>
<div class="password-strength">
    <cms:CMSTextBox runat="server" ID="txtPassword" TextMode="Password" />
         <cms:CMSRequiredFieldValidator ID="rfvPassword" CssClass="EditingFormErrorLabel" runat="server" ControlToValidate="txtPassword"
        Display="Dynamic" EnableViewState="false" />
    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" CssClass="EditingFormErrorLabel" ValidationExpression="(?=^.{8,50}$)((?=.*\d)|(?=.*[!@#$%^&*]+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"></asp:RegularExpressionValidator>
    <asp:Label ID="lblRequiredFieldMark" runat="server" Text="" Visible="false" />
    <div class="password-strength-text">
        <cms:LocalizedLabel runat="server" ID="lblPasswStregth" CssClass="password-strength-hint"
            ResourceString="Membership.PasswordStrength" />
        <strong runat="server" ID="lblEvaluation" EnableViewState="false" ></strong>
    </div>
    <asp:Panel runat="server" ID="pnlPasswStrengthIndicator" CssClass="passw-strength-indicator">
        <asp:Panel runat="server" ID="pnlPasswIndicator">
            &nbsp;
        </asp:Panel>
    </asp:Panel>
</div>
