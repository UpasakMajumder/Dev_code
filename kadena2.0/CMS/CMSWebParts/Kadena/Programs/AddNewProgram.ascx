<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Programs_AddNewProgram" CodeBehind="~/CMSWebParts/Kadena/Programs/AddNewProgram.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="uc1" TagName="UniSelector" %>

<div class="login__form-content js-login">
    <div class="css-login changepwd_block">
        <div class="form signup_form">
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProgramName"></span>
                    <asp:TextBox ID="txtProgramName" runat="server" CssClass="input__text"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="programNameRequired" runat="server" CssClass="input__error" ErrorMessage="" ControlToValidate="txtProgramName"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProgramDescription"></span>
                    <div class="input__inner">
                        <asp:TextBox ID="txtProgramDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="input__textarea" MaxLength="140" ClientIDMode="Static"></asp:TextBox>
                        <asp:CustomValidator runat="server" ID="cvDesc" ControlToValidate="txtProgramDescription" CssClass="input__error" OnServerValidate="cvDesc_ServerValidate"></asp:CustomValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" id="lblBrandName" runat="server"></span>
                    <uc1:UniSelector runat="server" ID="ddlBrand" ObjectType="customtableitem.KDA.Brand" ReturnColumnName="ItemID" SelectionMode="SingleDropDownList" CssClass="input__select" DisplayNameFormat="{%BrandName%}" AllowEmpty="false" />
                    <asp:RequiredFieldValidator ID="ddlBrandRequired" runat="server" CssClass="" ErrorMessage="Please select Brand" InitialValue="0" ControlToValidate="ddlBrand"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" id="lblCampaignName" runat="server"></span>
                    <uc1:UniSelector runat="server" ID="ddlCampaign" ObjectType="cms.document.KDA.Campaign" ReturnColumnName="CampaignID" SelectionMode="SingleDropDownList" CssClass="input__select" AllowEmpty="false" />
                    <asp:RequiredFieldValidator ID="ddlCampaignRequired" runat="server" CssClass="" ErrorMessage="Please select Campaign" InitialValue="0" ControlToValidate="ddlCampaign"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-3 form_btns">
            <div class="">
                <asp:Button ID="btnAddProgram" runat="server" CssClass="btn-action login__login-button btn--no-shadow" OnClick="btnAddProgram_Click" />
                <asp:Button ID="btnUpdateProgram" runat="server" CssClass="btn-action login__login-button btn--no-shadow" OnClick="btnUpdateProgram_Click" />
                <asp:Button ID="btnCancelProgram" CssClass="btn-action login__login-button btn--no-shadow" runat="server" OnClick="btnCancelProgram_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</div>
