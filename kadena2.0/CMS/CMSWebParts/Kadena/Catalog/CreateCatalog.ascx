<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Catalog_CreateCatalog" CodeBehind="~/CMSWebParts/Kadena/Catalog/CreateCatalog.ascx.cs" %>
<div class="custom_block">
    <div class="custom_select clearfix">
        <asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlBrands" runat="server" OnSelectedIndexChanged="ddlBrands_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlProductTypes" runat="server" OnSelectedIndexChanged="ddlProductTypes_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <label id="errorLabel"></label>
    </div>
    <div class="search_block">
        <asp:TextBox ID="posNumber" CssClass="input__text" runat="server" AutoPostBack="true" OnTextChanged="posNumber_TextChanged"></asp:TextBox>
    </div>
    <div class="custom_check">
        <div class="input__wrapper">
            <input type="checkbox" class=" input__checkbox selectAllChk " id="allCheck-<%# Eval("SKUID") %>" value="true">
            <label for="allCheck-<%# Eval("SKUID") %>" class="input__label input__label--checkbox selectAll" id="selectAllLabel"></label>
        </div>
    </div>
    <div class="custom_btns">
        <cms:LocalizedLinkButton runat="server" ID="llbPrintSelection" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.PrintSelection" OnClientClick="printselected();return false;"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveSelection" CssClass="saveSelection btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.SaveSelection" OnClick="llbSaveSelection_Click"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbPrintFull" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.PrintFull" Enabled="false"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveFull" CssClass="btn-action login__login-button btn--no-shadow saveAllCatalog" ResourceString="KDA.CustomCatalog.Filters.SaveFull" OnClick="llbSaveFull_Click" OnClientClick="PrintAll();return false;"></cms:LocalizedLinkButton>
    </div>
</div>
<div class="custom_content">
    <cms:CMSRepeater runat="server" ID="rptCatalogProducts">
        <ItemTemplate>
            <div class="cus_content_block">

                <div class="img_block">
                    <input type="checkbox" id="zoomCheck_<%# Eval("SKUID")%>" />
                    <label for="zoomCheck_<%# Eval("SKUID")%>">
                        <img src='<%#GetProductImage(Eval("SKUImagePath"))%>' />
                    </label>
                </div>
                <div class="input__wrapper">
                    <label for="dom" class="input__label "><%#ProductType == (int)ProductOfType.InventoryProduct? GetBrandName(ValidationHelper.GetInteger(Eval("BrandID"), default(int))):""%></label>
                    <input type="checkbox" id="dom_<%# Eval("SKUID")%>" name="ProductCheckBox" value='<%#Eval("SKUNumber")%>' class=" input__checkbox  js_Product" />
                    <label for="dom_<%# Eval("SKUID")%>" class="input__label input__label--checkbox"><%#Eval("ProductName")%></label>
                </div>
                <p><%#Eval("SKUDescription")%></p>

                <div class="crime_block Notdisplay">
                    <div class="crime_leftsec">
                        <div class="img_block">
                            <img src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png" style="width: 230px; padding: 30px 0;">
                        </div>
                    </div>
                    <div class="crime_rightsec">
                        <div class="crime_topsec">
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b>Part Number:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><b><%#Eval("SKUNumber")%></b></label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b>Brand:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#GetBrandName(ValidationHelper.GetInteger(Eval("BrandID"), default(int)))%></label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b>Short Desc:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("ProductName")%></label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b>Description:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("SKUDescription")%></label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b>Valid States:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#CMS.Globalization.StateInfoProvider.GetStateInfo("State")%></label>
                                </div>
                            </div>
                        </div>
                        <div class="crime_btmSec">
                            <div class="crime_btmSec_detail">
                                <div class="input_label">
                                    <label><b>Cost/Bundle:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("EstimatedPrice")%></label>
                                </div>
                            </div>
                            <div class="crime_btmSec_detail">
                                <div class="input_label">
                                    <label><b>Bundle Qty:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("QtyPerPack")%></label>
                                </div>
                            </div>
                            <div class="crime_btmSec_detail">
                                <div class="input_label">
                                    <label><b>Expire Date:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("SKUValidUntil")%></label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </ItemTemplate>
    </cms:CMSRepeater>
</div>
<asp:HiddenField ID="hdncheckedValues" runat="server" ClientIDMode="Static" />
