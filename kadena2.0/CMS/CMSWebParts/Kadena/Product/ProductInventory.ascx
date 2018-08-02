<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Product_ProductInventory" CodeBehind="~/CMSWebParts/Kadena/Product/ProductInventory.ascx.cs" %>

<div class="custom__section">
    <div class="custom__block clearfix" runat="server" visible="true" id="orderControls">
        <div class="custom__select">
            <asp:DropDownList runat="server" ID="ddlBrand" Visible="false" OnSelectedIndexChanged="OnFilterChanged" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlProgram" runat="server" Visible="false" OnSelectedIndexChanged="OnFilterChanged" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlCategory" runat="server" Visible="false" OnSelectedIndexChanged="OnFilterChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div class="search__block search__recent search__recent--icon">
            <asp:TextBox ID="txtSearch" runat="server" OnTextChanged="OnFilterChanged" AutoPostBack="true" class="input__text"></asp:TextBox>
            <button class="search__submit btn--off" type="submit">
                <svg class="icon icon-dollar">
                    <use xlink:href="/gfx/svg/sprites/icons.svg#search"></use>
                </svg>
            </button>
        </div>
        <div class="custom__check">
            <div class="input__wrapper">
                <asp:CheckBox runat="server" ID="chkOnlyAllocatedToMe" Checked="false" OnCheckedChanged="OnFilterChanged" AutoPostBack="true" />
                <cms:LocalizedLabel runat="server" AssociatedControlID="chkOnlyAllocatedToMe" ID="LocalizedLabel1" CssClass="input__label input__label--checkbox" ResourceString="KDA.InventoryOrder.OnlyAllocatedToMe"></cms:LocalizedLabel>
            </div>
        </div>
    </div>
    <div class="custom__content row">
        <cms:BasicRepeater runat="server" ID="rptProductLists">
            <ItemTemplate>
                <div class="cus__content--block col-sm-3">
                    <div class="img__block">
                        <input type="checkbox" id='zoomCheck_<%#Eval("SKUID") %>'>
                        <label for='zoomCheck_<%#Eval("SKUID") %>'>
                            <img src='<%#Eval<string>("ProductImage")==string.Empty?CMS.DataEngine.SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_ProductsPlaceHolderImage"):Eval<string>("ProductImage")%>' />
                    </div>
                    <div class="custom__blockin">
                        <h4><%= POSNumberText %> : <%# Eval("SKUProductCustomerReferenceNumber")%></h4>
                        <h3><%#Eval("SKUName") %></h3>
                        <h3><cms:LocalizedLiteral runat ="server" ResourceString="Kadena.Product.QuantityAvailable"/> <%#Eval("SKUAvailableItems") %></h3>
                        <span><%# ProductType == (int)Kadena.Models.Product.CampaignProductType.GeneralInventory? $"{CMS.Ecommerce.CurrencyInfoProvider.GetFormattedPrice(EvalDouble("SKUPrice"), CurrentSite.SiteID,true)} pack of {Eval("QtyPerPack")}" : $"{CMS.Ecommerce.CurrencyInfoProvider.GetFormattedPrice(EvalDouble("EstimatedPrice"), CurrentSite.SiteID,true)} pack of {Eval("QtyPerPack")}" %></span>
                        <b>
                            <asp:Label runat="server" Visible='<%# ProductType == (int)Kadena.Models.Product.CampaignProductType.PreBuy %>'>
                            <cms:LocalizedLiteral runat="server" ResourceString="Kadena.PreBuyOrder.CurrentDemand"></cms:LocalizedLiteral>&nbsp;<%# GetDemandCount(Eval<int>("SKUID")) %>
                        </asp:Label>
                        </b>
                        <a class="js-addToCart-Modal" href="javascript:void(0);" style='<%=((ProductType == (int)Kadena.Models.Product.CampaignProductType.PreBuy && !EnableAddToCart)?"display:none;":"")%>' data-skuid='<%#Eval<int>("SKUID")%>' data-productname='<%#Eval("SKUName")%>'><%#AddToCartLinkText%></a>
                        <asp:Label runat="server" Visible='<%#(ProductType == (int)Kadena.Models.Product.CampaignProductType.PreBuy ? !EnableAddToCart : false)%>'><%#AddToCartLinkText%></asp:Label>
                    </div>
                    <p><%#Eval("SKUDescription") %></p>
                </div>
            </ItemTemplate>
        </cms:BasicRepeater>
    </div>

    <div class="data_footer">
        <div class="dataTables_paginate paging_simple_numbers">
            <ul class="pagination mb-0 text--right list--unstyled">
                <cms:UniPager runat="server" ID="unipager" PageSize="24" GroupSize="10" PageControl="rptProductLists" PagerMode="PostBack" HidePagerForSinglePage="true" OnOnPageChanged="unipager_OnPageChanged"></cms:UniPager>
            </ul>
        </div>
    </div>

    <div id="divNoRecords" runat="server" visible="false">
        <div class=" mt-2">
            <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen"><span><%=NoDataText %></span></div>
        </div>
    </div>
    <div id="divNoCampaign" runat="server" visible="false">
        <div class=" mt-2">
            <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen"><span><%=NoCampaignOpen %></span></div>
        </div>
    </div>
</div>
