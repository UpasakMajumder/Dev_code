<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Product_ProductInventory" CodeBehind="~/CMSWebParts/Kadena/Product/ProductInventory.ascx.cs" %>

<div class="custom__section">
    <div class="custom__block clearfix" runat="server" visible="true" id="orderControls">
        <div class="custom__select">
            <asp:DropDownList ID="ddlProgram" runat="server" Visible="false" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlCategory" runat="server" Visible="false" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div class="search__block">
            <asp:TextBox ID="txtPos" runat="server" OnTextChanged="txtPos_TextChanged" AutoPostBack="true" class="input__text"></asp:TextBox>
        </div>
    </div>
    <div class="custom__content row">
        <asp:Repeater runat="server" ID="rptProductLists">
            <ItemTemplate>
                <div class="cus__content--block col-sm-3">
                    <div class="img__block">
                        <input type="checkbox" id='zoomCheck_<%#Eval("SKUID") %>'>
                        <label for='zoomCheck_<%#Eval("SKUID") %>'>
                            <img src='<%#Eval<string>("SKUImagePath")==string.Empty?CMS.DataEngine.SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_ProductsPlaceHolderImage"):Eval<string>("SKUImagePath")%>?MaxSideSize=150' />
                    </div>
                    <div class="custom__blockin">
                        <h4>POS#: <%# Eval("SKUNumber")%></h4>
                        <h3><%#Eval("SKUName") %></h3>
                        <span><%# $"${Eval("SKUPrice")} pack of {Eval("QtyPerPack")}"%></span>
                        <asp:LinkButton ID="lnkAddToCart" runat="server" CommandArgument='<%# Eval("SKUID") %>' CommandName="Add" OnCommand="lnkAddToCart_Command" Text='<%#AddToCartLinkText%>' EnableViewState="true"></asp:LinkButton>
                    </div>
                    <p><%#Eval("SKUDescription") %></p>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<div class="dialog" id="dialog_Add_To_Cart" runat="server" clientidmode="Static">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__header">
            <asp:Label runat="server" ID="lblPopUpHeader"></asp:Label>
            <asp:Label Text="" ID="lblProductName" runat="server" />
        </div>
        <div class="dialog__content">
            <asp:Label Text="" ID="lblError" Visible="false" runat="server" />
            <cms:LocalizedLabel runat="server" ID="lblErrorMsg" Visible="false"></cms:LocalizedLabel><br />
            <asp:Label Text="" runat="server" ID="lblAvailbleItems" />
            <asp:GridView runat="server" ID="gvCustomersCart" AutoGenerateColumns="false" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="AddressID" />
                    <asp:BoundField DataField="AddressPersonalName" />
                    <asp:TemplateField>
                        <HeaderTemplate><%# ResHelper.GetString("KDA.ShoppingCart.Quantity") %></HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" CssClass="input__text" ID="txtQuanityOrdering" Text='<%# ValidationHelper.GetString(Eval("SKUUnits"),"0") %>' TextMode="Number" min="0" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ShoppingCartID" Visible="true" HeaderText="" HeaderStyle-CssClass="u-hidden" ItemStyle-CssClass="u-hidden" />
                    <asp:BoundField DataField="SKUID" Visible="true" HeaderText="" HeaderStyle-CssClass="u-hidden" ItemStyle-CssClass="u-hidden" />
                </Columns>
            </asp:GridView>
            <asp:Label runat="server" ID="lblSuccessMsg" Visible="false"></asp:Label>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <button type="button" class="btn-action btn-action--secondary" id="btnClose" runat="server" onserverclick="btnClose_ServerClick" clientidmode="Static"></button>
                <cms:LocalizedLinkButton runat="server" ClientIDMode="Static" ID="llbtnAddToCart" ResourceString="KDA.ShoppingCart.AddItemsToCart" CssClass="btn-action" OnClick="btmAddItemsToCart_Click"></cms:LocalizedLinkButton>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField runat="server" ID="hdnClickSKU" />
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
