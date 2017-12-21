<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Catalog_CreateCatalog" CodeBehind="~/CMSWebParts/Kadena/Catalog/CreateCatalog.ascx.cs" %>
<div class="custom__block">
    <div class="custom__select clearfix">
        <asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlBrands" runat="server" OnSelectedIndexChanged="ddlBrands_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlProductTypes" runat="server" OnSelectedIndexChanged="ddlProductTypes_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <label id="errorLabel"></label>
    </div>
    <div class="search__block">
        <asp:TextBox ID="posNumber" CssClass="input__text" runat="server" AutoPostBack="true" OnTextChanged="posNumber_TextChanged"></asp:TextBox>
    </div>
    <div class="custom__check">
        <div class="input__wrapper">
            <input type="checkbox" class=" input__checkbox selectAllChk " id="allCheck-<%# Eval("NodeSKUID") %>" value="true">
            <label for="allCheck-<%# Eval("NodeSKUID") %>" class="input__label input__label--checkbox selectAll" id="selectAllLabel"></label>
        </div>
    </div>
    <div class="custom__btns">
        <cms:LocalizedLinkButton runat="server" ID="llbPrintSelection" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.PrintSelection" OnClientClick="printselected();return false;"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveSelection" CssClass="saveSelection btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.SaveSelection" OnClick="llbSaveSelection_Click"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbPrintFull" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.PrintFull" Enabled="false" OnClientClick="Printfull();return false;"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveFull" CssClass="btn-action login__login-button btn--no-shadow saveAllCatalog" ResourceString="KDA.CustomCatalog.Filters.SaveFull" OnClick="llbSaveFull_Click" ></cms:LocalizedLinkButton>
    </div>
</div>
<div class="custom__content row">
    <cms:CMSRepeater runat="server" ID="rptCatalogProducts">
         <HeaderTemplate>
             <div class="crimes__section notdisplay  printIt">
            <h1>19 Crimes - Share it with the Gang</h1>
               </div>
        </HeaderTemplate>
        <ItemTemplate>
           <div class="cus__content--block col-sm-3">
               <div class="noprint ">
                <div class="img__block">
                    <input type="checkbox" id="zoomCheck_<%# Eval("NodeSKUID")%>" />
                    <label for="zoomCheck_<%# Eval("NodeSKUID")%>">
                        <img src='<%#GetProductImage(Eval("SKUImagePath"))%>' />
                    </label>
                </div>
                <div class="input__wrapper">
                    <label for="dom" class="input__label "><%#TypeOfProduct == (int)ProductsType.GeneralInventory? GetBrandName(ValidationHelper.GetInteger(Eval("BrandID"), default(int))):""%></label>
                    <input type="checkbox" id="dom_<%# Eval("NodeSKUID")%>" name="ProductCheckBox" value='<%#Eval("SKUNumber")%>' class=" input__checkbox  js_Product" onchange="SelectforPrint(this);return false;" />
                    <label for="dom_<%# Eval("NodeSKUID")%>" class="input__label input__label--checkbox"><%#Eval("ProductName")%></label>
                </div>
                <p><%#Eval("SKUDescription")%></p>
               </div>
                <div class="crime__block enablePrint notdisplay">
                    <div class="crime__leftsec">
                        <div class="img__block"><p><%# Eval("SKUImagePath")%></p>
                            <img src="<%#GetProductImage(Eval("SKUImagePath"))%>" />
                        </div>
                    </div>
                    <div class="crime__rightsec">
                        <div class="crime__topsec">
                            <div class="details__sec">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.PartNumberText") %>:</b></label>
                                </div>
                                <div class="input__con">
                                    <label><b><%#Eval("SKUNumber")%>&nbsp;</b></label>
                                </div>
                            </div>
                            <div class="details__sec">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.BrandText") %>:</b></label>
                                </div>
                                <div class="input__con">
                                    <label><%#GetBrandName(ValidationHelper.GetInteger(Eval("BrandID"), default(int)))%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="details__sec">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.ShortDescText") %>:</b></label>
                                </div>
                                <div class="input__con">
                                    <label><%#Eval("ProductName")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="details__sec">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.DescriptionText") %>:</b></label>
                                </div>
                                <div class="input__con">
                                    <label><%#Eval("SKUDescription")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="details__sec">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("kadena.CatalogPrint.ValidStatesText") %>:</b></label>
                                </div>
                                <div class="input__con">
                                    <label><%#CMS.Globalization.StateInfoProvider.GetStateInfo("State")%>&nbsp;</label>
                                </div>
                            </div>
                        </div>
                        <div class="crime_btmSec">
                            <div class="crime_btmSec_detail">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("Kadena.CatalogPrint.CostText") %>:</b></label>
                                </div>
                                <div class="input__con">
                                    <label><%#Eval("EstimatedPrice")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="crime_btmSec_detail">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("Kadena.CatalogPrint.BundleQtyText") %>:</b></label>
                                </div>
                                <div class="input__con">
                                    <label><%#Eval("QtyPerPack")%>&nbsp;</label>
                                </div>
                            </div>
                            <div class="crime__btmSec_detail">
                                <div class="input__label">
                                    <label><b><%# CMS.Helpers.ResHelper.GetString("Kadena.CatalogPrint.ExpireDateText") %>:</b></label>
                                </div>
                                <div class="input__con">
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
<style>
   
    @media print{
        printIt{
            display:block;
        }
         .css-header{
        display:none;
    }
    .sidebar-logo{
        display:none;
    }
    .css-sidebar{
        display:none;
    }
    .cus__content--block{
        width: 100%;
    max-width: 100%;
    flex: 0 0 100%;
    }
    .crime__leftsec{width:20%;float:left}
    .crime__leftsec .img__block{width:100%;margin:0 auto}
    .crime__leftsec .img__block img{padding:30px 0;max-width:100%;height:auto}
    .crime__rightsec{width:77%;float:right;margin:20px 0 0}
    .crime__rightsec .crime__btmsec{margin-bottom:10px}
    .crime__rightsec .crime__btmsec .crime__btmsec-detail{width:30%;display:inline-block}
    .crime__rightsec .crime__btmsec .crime__btmsec-detail .input__label{display:inline-block;width:auto;float:none}
    .footer{
        display:none;
    }
    }
</style>