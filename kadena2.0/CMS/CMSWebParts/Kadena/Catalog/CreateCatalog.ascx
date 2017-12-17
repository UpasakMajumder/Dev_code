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
        <cms:LocalizedLinkButton runat="server" ID="llbPrintFull" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.PrintFull" Enabled="false" OnClientClick="Printfull();return false;"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveFull" CssClass="btn-action login__login-button btn--no-shadow saveAllCatalog" ResourceString="KDA.CustomCatalog.Filters.SaveFull" OnClick="llbSaveFull_Click" ></cms:LocalizedLinkButton>
    </div>
</div>
<div class="custom_content">
    <cms:CMSRepeater runat="server" ID="rptCatalogProducts">
         <HeaderTemplate>
             <div class="Crimes_section Notdisplay  printIt">
            <h1>19 Crimes - Share it with the Gang</h1>
               </div>
        </HeaderTemplate>
        <ItemTemplate>
           <div class="cus_content_block">
               <div class="noprint ">
                <div class="img_block">
                    <input type="checkbox" id="zoomCheck_<%# Eval("SKUID")%>" />
                    <label for="zoomCheck_<%# Eval("SKUID")%>">
                        <img src='<%#GetProductImage(Eval("SKUImagePath"))%>' />
                    </label>
                </div>
                <div class="input__wrapper">
                    <label for="dom" class="input__label "><%#TypeOfProduct == (int)ProductsType.GeneralInventory? GetBrandName(ValidationHelper.GetInteger(Eval("BrandID"), default(int))):""%></label>
                    <input type="checkbox" id="dom_<%# Eval("SKUID")%>" name="ProductCheckBox" value='<%#Eval("SKUNumber")%>' class=" input__checkbox  js_Product" onchange="SelectforPrint(this);return false;" />
                    <label for="dom_<%# Eval("SKUID")%>" class="input__label input__label--checkbox"><%#Eval("ProductName")%></label>
                </div>
                <p><%#Eval("SKUDescription")%></p>
               </div>
                <div class="crime_block enablePrint Notdisplay">
                    <div class="crime_leftsec">
                        <div class="img_block"><p><%# Eval("SKUImagePath")%></p>
                            <img src="<%#GetProductImage(Eval("SKUImagePath"))%>" />
                        </div>
                    </div>
                    <div class="crime_rightsec">
                        <div class="crime_topsec">
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.PartNumberText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><b><%#Eval("SKUNumber")%>&nbsp;</b></label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.BrandText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#GetBrandName(ValidationHelper.GetInteger(Eval("BrandID"), default(int)))%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.ShortDescText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("ProductName")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.DescriptionText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("SKUDescription")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="details_sec">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.ValidStatesText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#CMS.Globalization.StateInfoProvider.GetStateInfo("State")%>&nbsp;</label>
                                </div>
                            </div>
                        </div>
                        <div class="crime_btmSec">
                            <div class="crime_btmSec_detail">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("Kadena.CatalogPrint.CostText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("EstimatedPrice")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="crime_btmSec_detail">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("Kadena.CatalogPrint.BundleQtyText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("QtyPerPack")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="crime_btmSec_detail">
                                <div class="input_label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("Kadena.CatalogPrint.ExpireDateText") %>:</b></label>
                                </div>
                                <div class="input_con">
                                    <label><%#Eval("SKUValidUntil")%>&nbsp;</label>
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
