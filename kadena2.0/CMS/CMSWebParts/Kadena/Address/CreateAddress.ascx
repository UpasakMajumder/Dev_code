<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Address_CreateAddress" CodeBehind="~/CMSWebParts/Kadena/Address/CreateAddress.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="uc1" TagName="UniSelector" %>
<%@ Register Src="~/CMSFormControls/CountrySelector.ascx" TagPrefix="uc1" TagName="CountrySelector" %>


<div class="content-block">
    <div class="login__form-content js-login">
        <div class="css-login form_section">
            <div class="form signup_form form_width100">

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblName"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtName" runat="server" CssClass="input__password" placeholder="Enter Name" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfName" runat="server" ControlToValidate="txtName" ErrorMessage="Please enter Name" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressType"></span>
                        <div class="input__inner">
                            <uc1:UniSelector runat="server" ID="ddlAddressType" ObjectType="customtableitem.KDA.AddressType" ReturnColumnName="ItemID" SelectionMode="SingleDropDownList" CssClass="input__select" DisplayNameFormat="{%AddressTypeName%}" AllowEmpty="false" />
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCompany"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtComapnyName" runat="server" CssClass="input__password" placeholder="Enter Company"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressLine1"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtAddressLine1" runat="server" CssClass="input__password" placeholder="Enter Address Line1" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfAddressLine1" runat="server" ControlToValidate="txtAddressLine1" ErrorMessage="Please enter Address Line1" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblAddressLine2"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtAddressLine2" runat="server" CssClass="input__password" placeholder="Enter Address Line2" MaxLength="50"></asp:TextBox>

                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCity"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtCity" runat="server" CssClass="input__password" placeholder="Enter City" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfCity" ControlToValidate="txtCity" ErrorMessage="Please enter City" CssClass="input__error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <%--<div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">State</span>
                        <div class="input__inner">
                            <input type="type" class="input__password " placeholder="Enter State" value="">
                            <asp:TextBox ID="txtState" runat="server" CssClass="input__password" placeholder="Enter State"></asp:TextBox>

                        </div>
                    </div>
                </div>--%>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblCountry"></span>
                        <div class="input__inner">
                            <uc1:CountrySelector runat="server" ID="ddlCountry" AddNoneRecord="false" AddSelectCountryRecord="false"/>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblZipcode"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="input__password" placeholder="Enter Zip/Postal code" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfZipcode" runat="server" ErrorMessage="Please enter Zip/Postal code" ControlToValidate="txtZipcode" CssClass="input__error"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>

                <%--<div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Brand</span>
                        <div class="input__inner">
                            <uc1:UniSelector runat="server" ID="ddlBrand" ObjectType="customtableitem.KDA.Brand" ReturnColumnName="BrandCode" SelectionMode="SingleDropDownList" CssClass="input__select" DisplayNameFormat="{%BrandName%}" AllowEmpty="false" />
                        </div>
                    </div>
                </div>--%>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblTelephone"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="input__password" placeholder="Enter Telephone" MaxLength="25"></asp:TextBox>

                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label" runat="server" id="lblEmail"></span>
                        <div class="input__inner">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input__password" placeholder="your@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfEmail" ControlToValidate="txtEmail" runat="server" ErrorMessage="Please enter Email" CssClass="input__error"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>
               
                 <%--<div class="mb-2 form_block" style="height: 65px;">
                    <div class="input__wrapper">
                        <span class="input__label"></span>

                    </div>
                </div>--%>
                
                <div class="clearfix"></div>

               
            </div>
            <div class="mb-3 form_btns">
                <div class="">
                    <asp:Button runat="server" ID="btnSave" CssClass="btn-action login__login-button btn--no-shadow" Text="Save" OnClick="btnSave_Click" />
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
</div>
