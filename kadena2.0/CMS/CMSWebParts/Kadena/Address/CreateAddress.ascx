<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Address_CreateAddress" CodeBehind="~/CMSWebParts/Kadena/Address/CreateAddress.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="cms" TagName="UniSelector" %>
<%@ Register Src="~/CMSFormControls/CountrySelector.ascx" TagPrefix="uc1" TagName="CountrySelector" %>

<div class="content-block">
    <div class="login__form-content js-login">
        <div class="css-login form__section">
            <div class="form signup_form form_width100">

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblName"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtName" runat="server" CssClass="input__text" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterName")%>' MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfName" runat="server" ControlToValidate="txtName" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressType"></span>
                        <div class="input__inner">
                            <cms:UniSelector runat="server" ID="ddlAddressType" ObjectType="customtableitem.KDA.AddressType" ReturnColumnName="ItemID" SelectionMode="SingleDropDownList" CssClass="input__select" DisplayNameFormat="{%AddressTypeName%}" AllowEmpty="false" />
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCompany"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtComapnyName" runat="server" CssClass="input__text" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterCompany")%>'></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblTelephone"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="input__text" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterPhone")%>' MaxLength="25"></asp:TextBox>
                            <asp:CustomValidator ID="cvTelephone" runat="server" CssClass="input__error" ControlToValidate="txtTelephone" Enabled="false" OnServerValidate="cvTelephone_ServerValidate"></asp:CustomValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCountry"></span>
                        <div class="input__inner">
                            <cms:UniSelector ID="uniSelectorCountry" runat="server" DisplayNameFormat="{%CountryDisplayName%}" ObjectType="cms.country" ResourcePrefix="countryselector" AllowAll="false" AllowEmpty="false" CssClass="input__select js-Country" MaxDisplayedItems="400" MaxDisplayedTotalItems="450" OnOnSelectionChanged="uniSelectorCountry_OnSelectionChanged" HasDependingFields="true" />
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressLine1"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtAddressLine1" runat="server" CssClass="input__text js-Address" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterAddressline1")%>' MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfAddressLine1" runat="server" ControlToValidate="txtAddressLine1" CssClass="input__error js-errAddress"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressLine2"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtAddressLine2" runat="server" CssClass="input__text" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterAddressLine2")%>' MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCity"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtCity" runat="server" CssClass="input__text js-City" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterCity")%>' MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfCity" ControlToValidate="txtCity" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <cms:LocalizedLabel runat="server" ResourceString="Kadena.Address.State" CssClass="input__label"></cms:LocalizedLabel>
                        <div class="input__inner">
                            <cms:UniSelector ID="uniSelectorState" runat="server" DisplayNameFormat="{%StateDisplayName%}"
                                ObjectType="cms.state" ResourcePrefix="stateselector" DependsOnAnotherField="true" MaxDisplayedItems="400" MaxDisplayedTotalItems="450" Enabled="false" CssClass="input__select js-UniState" AllowAll="false" AllowEmpty="false" />
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblZipcode"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="input__text js-Zipcode" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterZip")%>' MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfZipcode" runat="server" ControlToValidate="txtZipcode" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form__block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblEmail"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input__text" placeholder='<%#ResHelper.GetString("KDA.Address.Watermark.EnterEmail")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfEmail" ControlToValidate="txtEmail" runat="server" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblBrand"><%#ResHelper.GetString("KDA.Address.Brands")%></span>
                        <div class="input__inner">
                            <a href="#" class="js-btnBrand" data-toggle="modal" data-target="#myModal_brand"><i class="fa fa-plus" aria-hidden="true"></i><%#ResHelper.GetString("KDA.Address.Brand")%></a>
                        </div>
                    </div>
                </div>

                <div class="clearfix"></div>
            </div>
            <div class="mb-3 form_btns">
                <div class="">
                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn-action login__login-button btn--no-shadow js-btnSmarty" OnClick="btnSave_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lnkCancel" runat="server" CssClass="btn-action login__login-button btn--no-shadow" CausesValidation="false" OnClick="btnCancel_Click"></asp:LinkButton>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdnBrand" runat="server" ClientIDMode="Static" />

<!--pop up html-->
<div class="modal_popup modal_brand" id="myModal_brand" style="display: none">
    <div class="modal-content">
        <div class="modal_header clearfix">
            <a href="#" class="btn-action js-btn js-btnSaveBrand"><%#ResHelper.GetString("KDA.Address.AddBrand")%></a>
            <a href="#" class="btn_close js-btnClose"><i class="fa fa-close"></i></a>
        </div>
        <div class="modal_body">
            <table class="show-table" id="brands">
                <tbody id="brandsbody">
                    <tr>
                        <td>
                            <input type="checkbox" class="js-chkAll" id="selectAll">
                        </td>
                        <td><%#ResHelper.GetString("KDA.Address.BrandName")%></td>
                        <td><%#ResHelper.GetString("KDA.Address.BrandCcode")%></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>

<!--Bind Brands-->
<div class="Business_Assigned_user">
    <table class="show-table js-brandsTable" id="AddressBrandsTable" style="display: none">
        <tbody id="AddressBrandsTablebody">
            <tr>
                <td><%#ResHelper.GetString("KDA.Address.BrandName")%></td>
                <td><%#ResHelper.GetString("KDA.Address.BrandCcode")%></td>
            </tr>
        </tbody>
    </table>
</div>

