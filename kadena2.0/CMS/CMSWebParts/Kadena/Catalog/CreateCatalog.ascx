<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Catalog_CreateCatalog" CodeBehind="~/CMSWebParts/Kadena/Catalog/CreateCatalog.ascx.cs" %>
<div class="custom__block" runat="server" id="catalogControls">
    <div class="custom__select clearfix">
        <asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true" style="max-width:200px;"></asp:DropDownList>
        <asp:DropDownList ID="ddlBrands" runat="server" OnSelectedIndexChanged="ddlBrands_SelectedIndexChanged" AutoPostBack="true" style="max-width:200px;"></asp:DropDownList>
        <asp:DropDownList ID="ddlProductTypes" runat="server" OnSelectedIndexChanged="ddlProductTypes_SelectedIndexChanged" AutoPostBack="true" style="max-width:200px;"></asp:DropDownList>
        <label id="errorLabel"></label>
    </div>
    <div class="search__block" id="searchDiv" runat="server">
        <asp:TextBox ID="posNumber" CssClass="input__text" runat="server" AutoPostBack="true" OnTextChanged="posNumber_TextChanged"></asp:TextBox>
    </div>
    <div class="custom__check">
        <div class="input__wrapper">
            <input type="checkbox" class=" input__checkbox selectAllChk " id="allCheck-<%# Eval("NodeSKUID") %>" value="true">
            <label for="allCheck-<%# Eval("NodeSKUID") %>" class="input__label input__label--checkbox selectAll" id="selectAllLabe">
                <%= SelectAllText %>
            </label>
        </div>
    </div>
    <div class="custom__btns float-right">
        <cms:LocalizedLinkButton runat="server" ID="llbSaveSelection" CssClass="saveSelection btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.SaveSelection" OnClick="llbSaveSelection_Click"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveFull" CssClass="btn-action login__login-button btn--no-shadow saveAllCatalog" ResourceString="KDA.CustomCatalog.Filters.SaveFull" OnClick="llbSaveFull_Click"></cms:LocalizedLinkButton>
        <cms:LocalizedLabel runat="server" ID="lblNoProducts" CssClass="input__label" ResourceString="KDA.CustomCatalog.SelectProducts" Visible="false"></cms:LocalizedLabel>
        <cms:LocalizedLinkButton runat="server" ID="llbExportFull" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.ExportFull" OnClick="llbExportFull_Click"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbExportSelection" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.ExportSelection" OnClick="llbExportSelection_Click"></cms:LocalizedLinkButton>
    </div>
</div>
<div id="noData" runat="server" visible="false">
    <div class="clearfix"></div>
    <div class=" mt-2">
        <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen"><span><%= NoDataFoundText  %></span></div>
    </div>
</div>
<div id="campaignIsNotOpen" runat="server" visible="false">
    <div class="clearfix"></div>
    <div class=" mt-2">
        <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen"><span><%= NoCampaignOpen  %></span></div>
    </div>
</div>
<div id="noProductSelected" runat="server" class="noProSelected" visible="false">
    <div class="clearfix"></div>
    <div class=" mt-2">
        <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen"><span><%= NoProductSelected  %></span></div>
    </div>
</div>
<div class="custom__content row">
    <cms:CMSRepeater runat="server" ID="rptCatalogProducts" DataBindByDefault="false">
        <ItemTemplate>
            <div class="cus__content--block col-sm-3" id="imagediv">
                <div class="img__block">
                    <input type="checkbox" id="zoomCheck_<%# Eval("NodeSKUID")%>" />
                    <label for="zoomCheck_<%# Eval("NodeSKUID")%>">
                        <img src='<%#Kadena.Old_App_Code.Kadena.PDFHelpers.CartPDFHelper.GetThumbnailImageAbsolutePath(Eval<string>("ProductImage"))%>' />
                    </label>
                </div>
                 <div class="zoom__in"><a href="javascript:void(0);" onclick='ShowZoomEffect("<%# Eval<string>("ProductImage")%>")'><svg class="icon"> <use xlink:href="/gfx/svg/sprites/icons.svg#search" xmlns:xlink="http://www.w3.org/1999/xlink"></use> </svg></a></div>
                <div class="input__wrapper custom__blockin">
                    <h4><%= POSNumberText %> : <%# Eval("SKUProductCustomerReferenceNumber")%></h4>
                    <label for="dom" class="input__label "><%# TypeOfProduct == (int)ProductsType.GeneralInventory? GetBrandName(ValidationHelper.GetInteger(Eval("BrandID"), default(int))):""%></label>
                    <input type="checkbox" id="dom_<%# Eval("NodeSKUID")%>" name="ProductCheckBox" value='<%#Eval("NodeSKUID")%>' class=" input__checkbox  js_Product" />
                    <label for="dom_<%# Eval("NodeSKUID")%>" class="input__label input__label--checkbox"><%#Eval("ProductName")%></label>
                </div>
                <p><%#Eval("SKUDescription")%></p>
            </div>
        </ItemTemplate>
    </cms:CMSRepeater>
</div>
<asp:HiddenField ID="hdncheckedValues" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnSaveFullCatalog" runat="server" ClientIDMode="Static" />
<cms:CMSRepeater Visible="false" runat="server" ID="hdnrptExport"></cms:CMSRepeater>
<%--Zoom EffectPopup--%>
<div class="dialog" id="ImageZoomPopup">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__header">
            <span><%# CMS.Helpers.ResHelper.GetString("Kadena.ProductStateInfo.StateGroupPopupHeading") %></span>
            <a onclick="$('#ImageZoomPopup').toggleClass('active');" class="btn__close js-btnClose"><i class="fa fa-close"></i></a>
        </div>
        <div class="dialog__content">
            <div class="modal__body business__assigned-user">
                 <div class="zoom__block"><img id="ZoomImage" src='<%#CMS.DataEngine.SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_ProductsPlaceHolderImage")%>' /></div>
            </div>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
            </div>
        </div>
    </div>
</div>