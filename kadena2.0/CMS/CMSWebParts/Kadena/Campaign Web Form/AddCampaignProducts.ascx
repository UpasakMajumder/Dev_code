<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/AddCampaignProducts.ascx.cs" %>

<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link rel="stylesheet" href="/resources/demos/style.css">

<asp:HiddenField ID="hdnDatepickerUrl" runat="server" />
<div class="login__form-content js-login">
    <div class="css-login">
        <div class="form form_width100 formerrormsgs">
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProgramName"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlProgram" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true" CssClass="input__select">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqProgram" runat="server" CssClass="input__error" ControlToValidate="ddlProgram" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblPosNumber"></span>
                    <div class="input__inner">
                        <asp:DropDownList runat="server" ID="ddlPos" CssClass="input__select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqPOS" runat="server" CssClass="input__error" ControlToValidate="ddlPos" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProductName"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtProductName" CssClass="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqProductName" CssClass="input__error" ControlToValidate="txtProductName"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblState"></span>
                    <div class="input__inner">
                        <asp:DropDownList runat="server" ID="ddlState" CssClass="input__select"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblLongDescription"></span>
                    <div class="input__inner">
                        <asp:TextBox ID="txtLongDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqLongDescription" CssClass="input__error" runat="server" ControlToValidate="txtLongDescription"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblExpirationDate"></span>
                    <div class="input__inner date_picker">
                        <asp:TextBox runat="server" ID="txtExpireDate" CssClass="input__text"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblBrand"></span>
                    <div class="input__inner">
                        <cms:CMSDropDownList ID="ddlBrand" runat="server" class="input__select" Enabled="false"></cms:CMSDropDownList>
                        <asp:HiddenField runat="server" ID="hfBrandItemID" />
                        <asp:RequiredFieldValidator ID="rqBrand" CssClass="input__error" runat="server" ControlToValidate="ddlBrand" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblEstimatedPrice"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtEstimatedprice" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqEstimatePrice" CssClass="input__error" ControlToValidate="txtEstimatedprice"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEstPrice" runat="server" CssClass="input__error"
                            ControlToValidate="txtEstimatedprice" ValidationExpression="((\d+)((\.\d{1,100})?))$" ForeColor="Red">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblActualPrice"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtActualPrice" class="input__text" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProductCategory"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlProductcategory" runat="server" CssClass="input__select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqProductCategory" CssClass="input__error" runat="server" ControlToValidate="ddlProductcategory" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblQtyPerPack"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtQty" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqQty" CssClass="input__error" runat="server" ControlToValidate="txtQty"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revQty" runat="server" CssClass="input__error"
                            ControlToValidate="txtQty" ValidationExpression="^[0-9]*$" ForeColor="Red">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblStatus"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input__select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqStatus" CssClass="input__error" runat="server" ControlToValidate="ddlStatus" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblItemSpecs"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtItemSpecs" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqItemSpecs" CssClass="input__error" ControlToValidate="txtItemSpecs" runat="server"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblImage"></span>
                    <div class="input__inner">
                        <asp:FileUpload ID="productImage" runat="server" class="input__file" EnableViewState="true" />
                        <div class="product-img">
                            <asp:Image ID="imgProduct" runat="server" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="mb-3 form_btns">
            <div class="">
                <asp:Button ID="btnSave" runat="server" class="btn-action login__login-button btn--no-shadow" OnClick="btnSave_Click" />
                <asp:Button ID="btnUpdate" runat="server" class="btn-action login__login-button btn--no-shadow" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancel" runat="server" class="btn-action login__login-button btn--no-shadow" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</div>
