<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Address_CreateAddress" CodeBehind="~/CMSWebParts/Kadena/Address/CreateAddress.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="cms" TagName="UniSelector" %>
<%@ Register Src="~/CMSFormControls/CountrySelector.ascx" TagPrefix="uc1" TagName="CountrySelector" %>

<div class="content-block">
    <div class="login__form-content js-login">
        <div class="css-login form_section">
            <div class="form signup_form form_width100">

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblName"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtName" runat="server" CssClass="input__text" placeholder="Enter Name" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfName" runat="server" ControlToValidate="txtName"  CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressType"></span>
                        <div class="input__inner">
                            <cms:UniSelector runat="server" ID="ddlAddressType" ObjectType="customtableitem.KDA.AddressType" ReturnColumnName="ItemID" SelectionMode="SingleDropDownList" CssClass="input__select" DisplayNameFormat="{%AddressTypeName%}" AllowEmpty="false" />
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCompany"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtComapnyName" runat="server" CssClass="input__text" placeholder="Enter Company"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressLine1"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtAddressLine1" runat="server" CssClass="input__text js-Address" placeholder="Enter Address Line1" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfAddressLine1" runat="server" ControlToValidate="txtAddressLine1" CssClass="input__error js-errAddress"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressLine2"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtAddressLine2" runat="server" CssClass="input__text" placeholder="Enter Address Line2" MaxLength="50"></asp:TextBox>

                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCity"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtCity" runat="server" CssClass="input__text js-City" placeholder="Enter City" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfCity" ControlToValidate="txtCity"  CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <cms:CMSUpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="mb-2 form_block">
                            <div class="input__wrapper">
                                <span class="input__label" runat="server" id="lblCountry"></span>
                                <div class="input__inner">
                                    <cms:UniSelector ID="uniSelectorCountry" runat="server" DisplayNameFormat="{%CountryDisplayName%}" ObjectType="cms.country" ResourcePrefix="countryselector" AllowAll="false" AllowEmpty="false" CssClass="input__select js-Country" MaxDisplayedItems="400" MaxDisplayedTotalItems="450" OnOnSelectionChanged="uniSelectorCountry_OnSelectionChanged" HasDependingFields="true" />
                                </div>
                            </div>
                        </div>

                        <div class="mb-2 form_block">
                            <div class="input__wrapper">
                                <cms:LocalizedLabel runat="server" ResourceString="Kadena.Address.State"></cms:LocalizedLabel>
                                <div class="input__inner">
                                    <cms:UniSelector ID="uniSelectorState" runat="server" DisplayNameFormat="{%StateDisplayName%}"
                                        ObjectType="cms.state" ResourcePrefix="stateselector" DependsOnAnotherField="true" MaxDisplayedItems="400" MaxDisplayedTotalItems="450" Enabled="false" CssClass="input__select js-UniState" AllowAll="false" AllowEmpty="false" />
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </cms:CMSUpdatePanel>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblZipcode"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="input__text js-Zipcode" placeholder="Enter Zip/Postal code" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfZipcode" runat="server"  ControlToValidate="txtZipcode" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblTelephone"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="input__text" placeholder="Enter Telephone" MaxLength="25"></asp:TextBox>
                            <asp:CustomValidator ID="cvTelephone" runat="server" CssClass="input__error"  ControlToValidate="txtTelephone" Enabled="false" OnServerValidate="cvTelephone_ServerValidate"></asp:CustomValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblEmail"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input__text" placeholder="your@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfEmail" ControlToValidate="txtEmail" runat="server" CssClass="input__error"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>

                <div class="clearfix"></div>

            </div>
            <div class="mb-3 form_btns">
                <div class="">
                    <asp:Button runat="server" ID="btnSave" CssClass="btn-action login__login-button btn--no-shadow js-btnSmarty" Text="Save" OnClick="btnSave_Click" />
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
</div>
