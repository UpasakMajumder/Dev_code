<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Address_CreateAddress" CodeBehind="~/CMSWebParts/Kadena/Address/CreateAddress.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="cms" TagName="UniSelector" %>
<%@ Register Src="~/CMSFormControls/CountrySelector.ascx" TagPrefix="uc1" TagName="CountrySelector" %>

<div class="content-block">
    <div class="login__form-content js-login">
        <div class="css-login form__section">
            <div class="form form__lg">

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

                <div class="mb-2 form__block">
                    <div class="input__wrapper allocated__block allocated__business">
                        <span class="input__label" runat="server" id="lblBrand"><%#ResHelper.GetString("Kadena.Address.Brands")%></span>
                        <a href="#" class="js-btnBrand" data-toggle="modal" data-target="#myModal_brand" id="btnAssignBrand">
                            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" width="10px" height="10px" viewBox="0 0 511.398 511.398" style="enable-background: new 0 0 511.398 511.398;" xml:space="preserve" class=""><g><g><path d="M477.549,182.379H329.018V33.847c0-18.69-15.154-33.844-33.844-33.844H216.22c-18.69,0-33.844,15.153-33.844,33.844    v148.526H33.844C15.153,182.373,0,197.526,0,216.216v78.966c0,18.691,15.153,33.844,33.844,33.844h148.532v148.527    c0,18.689,15.153,33.842,33.844,33.842h78.96c18.691,0,33.844-15.152,33.844-33.842V329.026h148.533    c18.689,0,33.842-15.152,33.842-33.844v-78.966C511.393,197.526,496.246,182.379,477.549,182.379z" data-original="#D63A3A" class="active-path" data-old_color="#0275d8" fill="#0275d8"/></g></g></g></svg>
                            </a>
                    </div>
                    <div class="Business_Assigned_user">
                        <table class="show-table js-brandsTable" id="AddressBrandsTable" style="display: none">
                            <tbody id="AddressBrandsTablebody">
                                <tr>
                                    <td><%#ResHelper.GetString("Kadena.Address.BrandName")%></td>
                                    <td><%#ResHelper.GetString("Kadena.Address.BrandCcode")%></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="clearfix"></div>
            </div>
            <div class="mb-3 form__btns">
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
<div class="modal__popup modal_brand" id="myModal_brand" style="display: none">
    <div class="modal__content">
        <div class="modal__header clearfix">
            <a href="#" class="btn-action js-btn js-btnSaveBrand"><%#ResHelper.GetString("Kadena.Address.AddBrand")%></a>
            <a href="#" class="btn__close js-btnClose"><i class="fa fa-close"></i></a>
        </div>
        <div class="modal__body">
            <table class="table" id="brands">
                <tbody id="brandsbody">
                    <tr>
                        <td>
                            <input type="checkbox" class="js-chkAll" id="selectAll">
                        </td>
                        <td><%#ResHelper.GetString("Kadena.Address.BrandName")%></td>
                        <td><%#ResHelper.GetString("Kadena.Address.BrandCode")%></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>



