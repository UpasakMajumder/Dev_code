<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Address_CreateAddress" CodeBehind="~/CMSWebParts/Kadena/Address/CreateAddress.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="uc1" TagName="UniSelector" %>
<%@ Register Src="~/CMSFormControls/CountrySelector.ascx" TagPrefix="uc1" TagName="CountrySelector" %>


<div class="content-block">
    <div class="login__form-content js-login">
        <div class="css-login form_section">
            <div class="form">
                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Name</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="Enter Name" value="">--%>
                            <asp:TextBox ID="txtName" runat="server" CssClass="input__password" placeholder="Enter Name" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfName" runat="server" ControlToValidate="txtName" ErrorMessage="Please enter Name"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>
                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Address Type</span>
                        <div class="input__inner">
                            <uc1:UniSelector runat="server" ID="ddlAddressType" ObjectType="customtableitem.KDA.AddressType" ReturnColumnName="ItemID" SelectionMode="SingleDropDownList" CssClass="input__select" DisplayNameFormat="{%AddressTypeName%}" AllowEmpty="false" />
                            <%--<select>
                                <option>Select Address type</option>
                                <option>1</option>
                                <option>1</option>
                            </select>--%>
                        </div>
                    </div>
                </div>

                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Company</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="Enter Company" value="">--%>
                            <asp:TextBox ID="txtComapnyName" runat="server" CssClass="input__password" placeholder="Enter Company"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Address Line1</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="Enter Address Line1" value="">--%>
                            <asp:TextBox ID="txtAddressLine1" runat="server" CssClass="input__password" placeholder="Enter Address Line1" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfAddressLine1" runat="server" ControlToValidate="txtAddressLine1" ErrorMessage="Please enter Address Line1"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Address Line2</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="Enter Address Line2" value="">--%>
                            <asp:TextBox ID="txtAddressLine2" runat="server" CssClass="input__password" placeholder="Enter Address Line2" MaxLength="50"></asp:TextBox>

                        </div>
                    </div>
                </div>
                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">City</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="Enter City" value="">--%>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="input__password" placeholder="Enter City" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfCity" ControlToValidate="txtCity" ErrorMessage="Please enter City"></asp:RequiredFieldValidator>
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
                        <span class="input__label">Country</span>
                        <div class="input__inner">
                            <uc1:CountrySelector runat="server" ID="ddlCountry" AddNoneRecord="false" AddSelectCountryRecord="false" />
                        </div>
                    </div>
                </div>
                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Zip / Postal Code</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="Enter Zip/Postal code" value="">--%>
                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="input__password" placeholder="Enter Zip/Postal code" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtZipcodeRequired" runat="server" ErrorMessage="Please enter Zip/Postal code" ControlToValidate="txtZipcode"></asp:RequiredFieldValidator>


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
                        <span class="input__label">Telephone</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="Telephone" value="">--%>
                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="input__password" placeholder="Enter Telephone" MaxLength="25"></asp:TextBox>

                        </div>
                    </div>
                </div>
                <div class="mb-2 form_block">
                    <div class="input__wrapper">
                        <span class="input__label">Email</span>
                        <div class="input__inner">
                            <%--<input type="type" class="input__password " placeholder="your@email.com" value="">--%>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input__password" placeholder="your@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfEmail" ControlToValidate="txtEmail" runat="server" ErrorMessage="Please enter Email"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>
                <div class="mb-2 form_block" style="height: 65px;">
                    <div class="input__wrapper">
                        <span class="input__label"></span>

                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="mb-3 form_btns">
                <div class="">

                    <%--<button type="button" class="btn-action login__login-button btn--no-shadow">save</button>--%>

                    <%--<button type="button" class="btn-action login__login-button btn--no-shadow">Cancel</button>--%>
                    <asp:Button runat="server" ID="btnSave" CssClass="btn-action login__login-button btn--no-shadow"  Text="Save" OnClick="btnSave_Click"/>
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" Text="Cancel" OnClick="btnCancel_Click" />
                </div>
            </div>

            <div class="clearfix"></div>
        </div>

    </div>
</div>
