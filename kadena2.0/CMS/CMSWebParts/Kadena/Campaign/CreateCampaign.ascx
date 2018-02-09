<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Campaign_CreateCampaign" CodeBehind="~/CMSWebParts/Campaign/CreateCampaign.ascx.cs" %>
<div class="content-block">
    <div class="form">
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblTokenIDlabel" runat="server" CssClass="input__label" ResourceString="Kadena.CampaignForm.CampaignName" />
                <cms:CMSTextBox ID="Name" runat="server" CssClass="input__text" MaxLength="100" />
                <cms:CMSRequiredFieldValidator ID="rfvUserNameRequired" runat="server" ControlToValidate="Name"  CssClass="EditingFormErrorLabel" />
                <asp:RegularExpressionValidator CssClass="EditingFormErrorLabel" ControlToValidate="Name" ID="rvName" ValidationExpression="^[\s\S]{0,40}$" runat="server" ></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="LocalizedLabel1" runat="server" CssClass="input__label" ResourceString="Kadena.CampaignForm.CampaignDes" />
                <div class="input__inner long__desc">
                    <cms:CMSTextBox ID="Description" runat="server" MaxLength="140" TextMode="MultiLine" />
                    <asp:RegularExpressionValidator CssClass="EditingFormErrorLabel" ControlToValidate="Description" ID="rvDescription" ValidationExpression="^[\s\S]{0,140}$" runat="server"></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblStartDate" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.CampaignForm.lblStartDate" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtStartDate" runat="server" EnableViewState="false" CssClass="input__text js-datepicker"></cms:CMSTextBox>
                     <cms:CMSRequiredFieldValidator ID="rfvStartDate" CssClass="EditingFormErrorLabel" ControlToValidate="txtStartDate" runat="server"></cms:CMSRequiredFieldValidator>
                    <asp:CompareValidator ID="compareDate" runat="server" Operator="GreaterThanEqual" CssClass="EditingFormErrorLabel" ControlToValidate="txtStartDate" Type="date"/>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lbl" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.CampaignForm.lblEndDate" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtEndDate" runat="server" EnableViewState="false" CssClass="input__text js-datepicker"></cms:CMSTextBox>
                   <cms:CMSRequiredFieldValidator ID="rfvEndDate"  CssClass="EditingFormErrorLabel" ControlToValidate="txtEndDate" runat="server"></cms:CMSRequiredFieldValidator>
                     <asp:CompareValidator ID="compareWithStartdate" runat="server" Operator="GreaterThan" CssClass="EditingFormErrorLabel" ControlToValidate="txtEndDate" ControlToCompare="txtStartDate" Type="date" />
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="LocalizedFiscalYear" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.CampaignForm.FiscalYear" />
                <div class="input__inner">
                   <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="input__select"></asp:DropDownList>
                   <asp:RequiredFieldValidator ID="rqFiscalYear" runat="server" CssClass="EditingFormErrorLabel" ControlToValidate="ddlFiscalYear" InitialValue="0"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel runat="server" ID="lblCampaignStatus" CssClass="input__label" ResourceString="Kadena.CampaignForm.lblStatus"></cms:LocalizedLabel>
                <div class="input__inner">
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input__select"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <div class="mb-3 form__btns">
        <cms:LocalizedButton ID="btnSave" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary" CommandName="Login" EnableViewState="false"
            ResourceString="Kadena.CampaignForm.SaveButton" />
        <cms:LocalizedButton ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" CausesValidation="false" OnClick="btnCancel_Cancel" runat="server" ButtonStyle="Primary" CommandName="Login" EnableViewState="false"
            ResourceString="Kadena.CampaignForm.CancelButton" />
    </div>
    <cms:LocalizedLabel ID="lblSuccessMsg" Visible="false" runat="server" CssClass="input__label" EnableViewState="False" ResourceString="Kadena.CampaignForm.SaveMsg" />
    <cms:LocalizedLabel ID="lblFailureText" runat="server" EnableViewState="False" CssClass="error-label EditingFormErrorLabel" Visible="false" ResourceString="Kadena.CampaignForm.FailureMsg" />
</div>
