<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_BusinessUnit_BusinessUnit" CodeBehind="~/CMSWebParts/Kadena/BusinessUnit/BusinessUnit.ascx.cs" %>

<div class="content-block">
    <div class="login__form-content js-login">
        <div class="css-login changepwd_block">
            <div class="form">
                <div class="mb-2">
                    <div class="input__wrapper">
                        <span id="lblBUNumber" runat="server" class="input__label"></span>
                        <asp:TextBox ID="txtBUNumber" runat="server" MaxLength="10" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfBUNumber" ControlToValidate="txtBUNumber" runat="server" CssClass="EditingFormErrorLabel"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revBUNumber" runat="server" ControlToValidate="txtBUNumber" ValidationExpression="^[0-9]{8,10}$" CssClass="EditingFormErrorLabel" />
                        <asp:CustomValidator ID="cvBUNumber" runat="server" OnServerValidate="cvBUNumber_ServerValidate" ControlToValidate="txtBUNumber" Enabled="true" CssClass="EditingFormErrorLabel"></asp:CustomValidator>
                    </div>
                </div>
                <div class="mb-2">
                    <div class="input__wrapper">
                        <span id="lblBUName" runat="server" class="input__label"></span>
                        <asp:TextBox ID="txtBUName" runat="server" MaxLength="50" CssClass="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfBUName" ControlToValidate="txtBUName" CssClass="EditingFormErrorLabel"></asp:RequiredFieldValidator><br />
                    </div>
                </div>
                <div class="mb-2">
                    <div class="input__wrapper">
                        <span id="lblBUStatus" runat="server" class="input__label"></span>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input__select"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="mb-3 form__btns">
                <div class="">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn-action login__login-button btn--no-shadow" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn-action login__login-button btn--no-shadow" CausesValidation="false" />
                </div>
            </div>
        </div>

    </div>
</div>
