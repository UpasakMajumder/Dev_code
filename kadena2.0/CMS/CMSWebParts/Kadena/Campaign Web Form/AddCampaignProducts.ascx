<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/AddCampaignProducts.ascx.cs" %>

<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link rel="stylesheet" href="/resources/demos/style.css">
<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>


<div class="login__form-content js-login">
    <div class="css-login">
        <div class="form form_width100 formerrormsgs">
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProgramName"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlProgram" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqProgram" runat="server" CssClass="input-error" ControlToValidate="ddlProgram" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblPosNumber"></span>
                    <div class="input__inner">
                        <asp:DropDownList runat="server" ID="ddlPos">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqPOS" runat="server" CssClass="input-error" ControlToValidate="ddlPos" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProductName"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtProductName" CssClass="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqProductName" CssClass="input-error" ControlToValidate="txtProductName"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblState"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtState" CssClass="input__text"></asp:TextBox>
                        <%--  <select>
                        <option>Select States</option>
                        <option>select1</option>
                        <option>select2</option>
                    </select>--%>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblLongDescription"></span>
                    <div class="input__inner">
                        <asp:TextBox ID="txtLongDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqLongDescription" CssClass="input-error" runat="server" ControlToValidate="txtLongDescription"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblExpirationDate"></span>
                    <div class="input__inner date_picker">
                        <asp:TextBox runat="server" ID="txtExpireDate" CssClass="input__text"></asp:TextBox>
                        <%--  <div class="datepicker_icon" id="datepicker">
                            <i class="fa fa-calendar" aria-hidden="true"></i>
                        </div>--%>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblBrand"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtBrand" class="input__text" ReadOnly="true"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hfBrandItemID" />
                        <asp:RequiredFieldValidator ID="rqBrand" CssClass="input-error" runat="server" ControlToValidate="txtBrand"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblEstimatedPrice"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtEstimatedprice" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqEstimatePrice" CssClass="input-error" ControlToValidate="txtEstimatedprice"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblActualPrice"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtActualPrice" class="input__text"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProductCategory"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlProductcategory" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqProductCategory" CssClass="input-error" runat="server" ControlToValidate="ddlProductcategory" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblQtyPerPack"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtQty" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqQty" CssClass="input-error" runat="server" ControlToValidate="txtQty"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblStatus"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="0">De-Active</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqStatus" CssClass="input-error" runat="server" ControlToValidate="ddlStatus" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form_block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblItemSpecs"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtItemSpecs" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqItemSpecs" CssClass="input-error" ControlToValidate="txtItemSpecs" runat="server"></asp:RequiredFieldValidator>
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

<script>
    $(function () {
        $('[id$=txtExpireDate]').datepicker({
            showOn: "both",
            buttonImage: "https://png.icons8.com/?id=3344&size=280",
            buttonImageOnly: false
        });
    });

    $('[id$=productImage]').on('change', function () {
        $('.product-img').hide();
    });
</script>

