<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Programs_AddNewProgram" CodeBehind="~/CMSWebParts/Kadena/Programs/AddNewProgram.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="uc1" TagName="UniSelector" %>
<div class="login__form-content js-login">
    <div class="css-login changepwd_block">
        <div class="form">
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProgramName"></span>
                    <asp:TextBox ID="txtProgramName" runat="server" CssClass="input__text"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="programNameRequired" runat="server" CssClass="EditingFormErrorLabel"  ControlToValidate="txtProgramName"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator  ControlToValidate="txtProgramName" ID="revProgramName" CssClass="EditingFormErrorLabel" ValidationExpression="^[\s\S]{0,50}$" runat="server" ></asp:RegularExpressionValidator>
                 </div>
            </div>
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProgramDescription"></span>
                    <div class="input__inner long__desc">
                        <asp:TextBox ID="txtProgramDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="input__textarea" MaxLength="150" ClientIDMode="Static"></asp:TextBox>
                        <asp:RegularExpressionValidator CssClass="EditingFormErrorLabel"   ControlToValidate="txtProgramDescription" ID="revDescription"  ValidationExpression="^[\s\S]{0,150}$"  runat="server"></asp:RegularExpressionValidator>  
                    </div>
                </div>
            </div>
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" id="lblBrandName" runat="server"></span>
                    <cms:CMSDropDownList ID="ddlBrand" CssClass="input__select" runat="server" EnableViewState="True"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="ddlBrandRequired" runat="server" CssClass="EditingFormErrorLabel" ErrorMessage="Please select Brand" ControlToValidate="ddlBrand" InitialValue="0"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" id="lblCampaignName" runat="server"></span>
                    <cms:CMSDropDownList ID="ddlCampaign" CssClass="input__select" runat="server"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="ddlCampaignRequired" runat="server" CssClass="EditingFormErrorLabel" ErrorMessage="Please select Campaign" ControlToValidate="ddlCampaign" InitialValue="0"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="mb-2">
                <div class="input__wrapper" id="deliveryDateDiv" runat="server">
                    <span class="input__label" id="lblProgramDeliveryDate" runat="server"></span>
                    <asp:TextBox ID="txtProgramDeliveryDate" type="text" ClientIDMode="Static" runat="server" CssClass="input__text js-datepicker"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtDeliveryDateRequired" runat="server" CssClass="EditingFormErrorLabel" ErrorMessage="Please select delivery date" ControlToValidate="txtProgramDeliveryDate"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="compareDate" runat="server" Operator="GreaterThanEqual" CssClass="EditingFormErrorLabel"  ControlToValidate="txtProgramDeliveryDate" Type="date" />
                </div>
            </div>
             <div class="mb-2">
                <div class="input__wrapper">
                    <span class="input__label" id="lblStatus" runat="server"></span>
                    <cms:CMSDropDownList id="ddlStatus" CssClass="input__select" runat="server" ></cms:CMSDropDownList>
               </div>
            </div>
        </div>
        <div class="mb-3 form__btns">
            <div class="">
                <asp:Button ID="btnAddProgram" runat="server" CssClass="btn-action login__login-button btn--no-shadow" OnClick="btnAddProgram_Click" />
                <asp:Button ID="btnUpdateProgram" runat="server" CssClass="btn-action login__login-button btn--no-shadow" OnClick="btnUpdateProgram_Click" />
                <asp:Button ID="btnCancelProgram" CssClass="btn-action login__login-button btn--no-shadow" runat="server" OnClick="btnCancelProgram_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</div>
