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
<style type="text/css">
   
 .content_container {
        width: 100%;
        margin: 60px 0px;
    }

    .clearfix {
        clear: both;
    }

    .treasure_content {
        margin-bottom: 220px;
    }

    .treasure_heading {
        text-align: center;
    }

        .treasure_heading h2 {
            color: #b5161a;
            text-transform: uppercase;
            margin-bottom: 40px;
        }

        .treasure_heading p {
            font-size: 24px;
            margin-bottom: 40px;
        }

    .treasure_con {
        text-align: center;
    }

        .treasure_con h2 {
            font-size: 22px;
            text-decoration: underline;
            font-family: arial;
            font-weight: 600;
            color: #333;
        }

        .treasure_con p {
            font-size: 22px;
            color: #333;
        }

            .treasure_con p.merch_con {
                color: #b5161a;
                padding: 0 180px;
            }

    .Crimes_section {
        width: 100%;
        padding: 0 0px;
        margin: 0 auto;
    }

        .Crimes_section h1 {
            text-align: center;
        }

    .crime_block {
        border-bottom: 1px solid #000;
    }

    .crime_leftsec {
        width: 20%;
        float: left;
    }

    .img_block {
        width: 100%;       
        margin: 0 auto;
    }

        .img_block img {
            padding: 30px 0;
            max-width: 100%;
        }

    .crime_rightsec {
        width: 77%;
        float: right;
        margin: 20px 0 0 0px;
    }

    .details_sec {
        margin-bottom: 10px;
    }

    .input_label {
        width: 20%;
        float: left;
        display: inline-block;
    }

        .input_label label, .input_con label {
            font-size: 20px;
        }

    .input_con {
        width: 60%;
        display: inline-block;
    }

    .crime_btmSec {
        margin-bottom: 10px;
    }

    .crime_btmSec_detail {
        width: 30%;
        display: inline-block;
    }

        .crime_btmSec_detail .input_label {
            display: inline-block;
            width: auto;
            float: none;
        }

        .crime_btmSec_detail .input_con {
            display: inline-block;
            width: auto;
        }

    @media print {
        .content_container {
            width: 1000px;
        }
    }

    .img_block input[type=checkbox] {
        display: none;
    }

    .img_block img {
       
        transition: transform 0.25s ease;
        cursor: zoom-in;
    }

    .img_block input[type=checkbox]:checked ~ label > img {
        transform: scale(2);
        cursor: zoom-out;
    }

    .Notdisplay {
        display: none;
    }
    
    .pop_closeicon{
        position: absolute;
    right: 10px;
    top: 0px;
    text-align: right;
    font-size: 25px;
    }
    @media print {
        .noprint {
            display: none;
        }
        .printIt {
            display: block;
        }
        .custom_block {
            display: none;
        }
        .content {
            padding-left: 0 !important;
        }
        .crime_leftsec {
            width: 20%;
            float: left;
        }
        .cus_content_block {
            width:100%;
        }
        .content-header__inner { display: none;
        }
    }
</style>