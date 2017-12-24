<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Campaign_CreateCampaign" CodeBehind="~/CMSWebParts/Campaign/CreateCampaign.ascx.cs" %>
<div class="content-block">
    <div class="form">
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblTokenIDlabel" runat="server" CssClass="input__label" ResourceString="Kadena.CampaignForm.CampaignName" />
                <cms:CMSTextBox ID="Name" runat="server" CssClass="input__text" MaxLength="100" />
                <cms:CMSRequiredFieldValidator ValidationGroup="requiredValidation" ID="rfvUserNameRequired" runat="server" ControlToValidate="Name" Display="Dynamic" ForeColor="Red" />
                <asp:RegularExpressionValidator ValidationGroup="requiredValidation" Display="Dynamic" ControlToValidate="Name" ID="rvName" ValidationExpression="^[\s\S]{0,40}$" runat="server" ForeColor="Red"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="LocalizedLabel1" runat="server" CssClass="input__label" ResourceString="Kadena.CampaignForm.CampaignDes" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="Description" runat="server" MaxLength="140" TextMode="MultiLine" />
                    <asp:RegularExpressionValidator ForeColor="Red" ValidationGroup="requiredValidation" Display="Dynamic" CssClass="input__error" ControlToValidate="Description" ID="rvDescription" ValidationExpression="^[\s\S]{0,140}$" runat="server"></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblStartDate" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.CampaignForm.lblStartDate" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtStartDate" runat="server" EnableViewState="false" CssClass="input__text js-datepicker" ReadOnly="true"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" CssClass="input__error" ControlToValidate="txtStartDate">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lbl" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.CampaignForm.lblEndDate" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtEndDate" runat="server" EnableViewState="false" CssClass="input__text js-datepicker" ReadOnly="true"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" CssClass="input__error" ControlToValidate="txtEndDate">
                    </asp:RequiredFieldValidator>
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
        <cms:LocalizedButton ID="btnSave" ValidationGroup="requiredValidation" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary" CommandName="Login" EnableViewState="false"
            ResourceString="Kadena.CampaignForm.SaveButton" />
        <cms:LocalizedButton ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" OnClick="btnCancel_Cancel" runat="server" ButtonStyle="Primary" CommandName="Login" EnableViewState="false"
            ResourceString="Kadena.CampaignForm.CancelButton" />
    </div>
    <cms:LocalizedLabel ID="lblSuccessMsg" Visible="false" runat="server" CssClass="input__label" EnableViewState="False" ResourceString="Kadena.CampaignForm.SaveMsg" />
    <cms:LocalizedLabel ID="lblFailureText" runat="server" EnableViewState="False" CssClass="error-label input__error" Visible="false" ResourceString="Kadena.CampaignForm.FailureMsg" />
</div>
