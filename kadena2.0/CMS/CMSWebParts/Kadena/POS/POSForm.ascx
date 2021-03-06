<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_POSForm" CodeBehind="~/CMSWebParts/Kadena/POS/POSForm.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="uc1" TagName="UniSelector" %>

<div class="css-login changepwd_block">
    <div class="form">
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblCategory" runat="server" EnableViewState="False" CssClass="logon-token-info" ResourceString="Kadena.POSFrom.POSCategoryLabel" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlCategory" runat="server" EnableViewState="false" CssClass="input__select"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator  ID="rfvCatgory" InitialValue="0" runat="server" CssClass="EditingFormErrorLabel"  ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblBrand" runat="server" EnableViewState="False" CssClass="logon-token-info" ResourceString="Kadena.POSFrom.BrandLabel" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlBrand" runat="server" EnableViewState="false" onchange="brandChange(this);return false;" CssClass="input__select"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvBrand" runat="server" CssClass="EditingFormErrorLabel" InitialValue="0"  ControlToValidate="ddlBrand"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblYear" runat="server" EnableViewState="False" CssClass="logon-token-info" ResourceString="Kadena.POSFrom.FiscalYearLabel" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlYear" runat="server" EnableViewState="false" onchange="yearChange(this);return false;" CssClass="input__select"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator  ID="rfvYear" runat="server" InitialValue="0"  ControlToValidate="ddlYear" CssClass="EditingFormErrorLabel"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblPOSCode" runat="server" EnableViewState="False" CssClass="logon-token-info" ResourceString="Kadena.POSFrom.POSLabel" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtPOSCode"  runat="server" EnableViewState="false" onChange="poscodeChange(this);return false;" CssClass="input__text"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvPOSCode"  CssClass="EditingFormErrorLabel" runat="server"   ControlToValidate="txtPOSCode">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="LocalizedLabel1" runat="server" EnableViewState="False" CssClass="logon-token-info" ResourceString="Kadena.POSFrom.POSNumber" />
                <div class="input__inner">
                    <asp:TextBox ID="txtPOSNumber" runat="server" ReadOnly="true" EnableViewState="false" CssClass="input__text"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="mb-3 form__btns">
        <div class="">
            <cms:LocalizedButton ID="btnSave" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary" EnableViewState="false"
                ResourceString="Kadena.POSFrom.SaveButtonText" />
            <cms:LocalizedButton ID="btnCancel" CausesValidation="false" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary" EnableViewState="false" ResourceString="Kadena.POSForm.CancelButtonText" />
        </div>
    </div>
    <cms:LocalizedLabel ID="lblSuccess" runat="server" EnableViewState="False" Visible="false" CssClass="logon-token-info" ForeColor="Green" ResourceString="Kadena.POSFrom.SuccessMsg" />
    <cms:LocalizedLabel ID="lblError" runat="server" EnableViewState="False" Visible="false" CssClass="logon-token-info"  ResourceString="Kadena.POSFrom.ErrorMsg" />
    <cms:LocalizedLabel ID="lblDuplicate" runat="server" EnableViewState="False" Visible="false" CssClass="logon-token-info"  ResourceString="Kadena.POSFrom.DuplicatePOSMsg" />
</div>
