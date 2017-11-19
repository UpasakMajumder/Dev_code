<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/AddCampaignProducts.ascx.cs" %>
<%--<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="uc1" TagName="UniSelector" %>--%>

<div class="css-login">
    <div class="form">
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Program Name</span>
                <div class="input__inner">
                    <%--<uc1:UniSelector runat="server" ID="ddlProgram" ObjectType="cms.document.KDA.Program" ReturnColumnName="ProgramID" SelectionMode="SingleDropDownList" CssClass="input__select" AllowEmpty="false"/>--%>
                    <asp:DropDownList ID="ddlProgram" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">--Select Program--</asp:ListItem>
                    </asp:DropDownList>
                   
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">POS number</span>
                <div class="input__inner">
                    <select>
                        <option>Select POS Number</option>
                        <option>select1</option>
                        <option>select2</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Product Name</span>
                <div class="input__inner">
                    <asp:TextBox runat="server" ID="txtProductName" CssClass="input__text"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Allowed States</span>
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
                <span class="input__label">Long Description</span>
                <div class="input__inner" style="line-height: 0;">
                    <asp:TextBox ID="txtLongDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="input__text"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Expiration Date</span>
                <div class="input__inner date_picker">
                    <input type="type" class="input__text">
                    <div class="datepicker_icon">
                        <i class="fa fa-calendar" aria-hidden="true"></i>
                    </div>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Brand Name</span>
                <div class="input__inner">
                    <asp:TextBox runat="server" ID="txtBrand" class="input__text" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Product Category</span>
                <div class="input__inner">
                    <select>
                        <option>Select Product Category</option>
                        <option>select1</option>
                        <option>select2</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Qty Per Pack</span>
                <div class="input__inner">
                    <asp:TextBox runat="server" ID="txtQty" class="input__text"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Item Specs</span>
                <div class="input__inner">
                    <asp:TextBox runat="server" ID="txtItemSpecs" class="input__text"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">Image</span>
                <div class="input__inner">
                    <asp:FileUpload ID="productImage" runat="server" class="input__file" />
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
    </div>
    <div class="mb-3 form_btns">
        <div class="">
            <asp:Button ID="btnSave" runat="server" class="btn-action login__login-button btn--no-shadow" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnUpdate" runat="server" class="btn-action login__login-button btn--no-shadow" Text="Update" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnCancel" runat="server" class="btn-action login__login-button btn--no-shadow" Text="Cancel" OnClick="btnCancel_Click" />
        </div>
    </div>
</div>
