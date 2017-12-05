<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Catalog_CreateCatalog" CodeBehind="~/CMSWebParts/Kadena/Catalog/CreateCatalog.ascx.cs" %>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<div class="custom_block">
    <div class="custom_select">
        <asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlBrands" runat="server" OnSelectedIndexChanged="ddlBrands_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlProductTypes" runat="server" OnSelectedIndexChanged="ddlProductTypes_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </div>
    <div class="custom_check">
        <div class="input__wrapper">
            <input type="checkbox" class=" selectAllChk " id="dom-1" value="true">
            <cms:LocalizedLabel runat="server" CssClass="selectAll" ID="selectAllLabel" ResourceString="KDA.CustomCatalog.Filter.SelectAllLabel"></cms:LocalizedLabel>
        </div>
    </div>
    <div class="custom_btns">
        <cms:LocalizedLinkButton runat="server" ID="llbPrintSelection" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.PrintSelection"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveSelection" ViewStateMode="Enabled" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.SaveSelection" OnClick="llbSaveSelection_Click"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbPrintFull" CssClass="btn-action login__login-button btn--no-shadow" ResourceString="KDA.CustomCatalog.Filters.PrintFull" Enabled="false"></cms:LocalizedLinkButton>
        <cms:LocalizedLinkButton runat="server" ID="llbSaveFull" CssClass="btn-action login__login-button btn--no-shadow saveAllCatalog" ResourceString="KDA.CustomCatalog.Filters.SaveFull" OnClick="llbSaveFull_Click"></cms:LocalizedLinkButton>
    </div>
</div>
<div class="custom_content">
    <cms:CMSRepeater runat="server" ClassNames="KDA.CampaignProduct" Path="./%" ID="rptCatalogProducts">
        <ItemTemplate>
            <div class="cus_content_block">
                <div class="img_block">
                    <img src='~/getmedia/<%#Eval("Image")%>/test' />
                </div>
                <i class="fa fa-search" aria-hidden="true"></i>
                <div>
                    <input type="checkbox" id="test" name="ProductCheckBox" value='<%#Eval("CampaignProductID")%>' class="js_Product" runat="server" />
                    <label for="dom" class="input__label input__label--checkbox"><%#Eval("ProductName")%></label>
                </div>
                <p><%#Eval("ShortDescription")%></p>
            </div>
        </ItemTemplate>
    </cms:CMSRepeater>
</div>
<asp:HiddenField ID="hdncheckedValues" runat="server" ClientIDMode="Static" />
