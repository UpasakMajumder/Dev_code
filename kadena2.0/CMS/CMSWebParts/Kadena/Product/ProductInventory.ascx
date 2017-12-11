<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Product_ProductInventory" CodeBehind="~/CMSWebParts/Kadena/Product/ProductInventory.ascx.cs" %>
<%@ Register TagName="CustomerCart" Src="~/CMSWebParts/Kadena/ShoppingCart/CustomerCartOperations.ascx" TagPrefix="Cart" %>

<div class="custom_section">
    <div class="custom_block clearfix">
        <div class="custom_select">
            <asp:DropDownList ID="ddlProgram" runat="server" Visible="false" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlCategory" runat="server" Visible="false" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div class="search_block">
            <asp:TextBox ID="txtPos" runat="server" OnTextChanged="txtPos_TextChanged" AutoPostBack="true" class="input__text"></asp:TextBox>
        </div>
    </div>
    <div class="custom_content">
        <cms:CMSRepeater ID="rptProductList" runat="server">
            <ItemTemplate>
                <div class="cus_content_block">
                    <div class="img_block">
                        <input type="checkbox" id='zoomCheck_<%#Eval("SKUID") %>'>
                        <label for='zoomCheck_<%#Eval("SKUID") %>'>
                            <img src='<%#GetProductImage(Eval("SKUImagePath"))%>' />
                    </div>
                    <div class="custom_blockin">
                        <h4><%# Eval("SKUNumber")%></h4>
                        <h3><%#Eval("SKUName") %></h3>
                        <span><%# $"${Eval("SKUPrice")} pack of {Eval("QtyPerPack")}"%></span>
                        <asp:LinkButton ID="lnkAddToCart" runat="server" CommandArgument='<%#Eval("SKUID") %>' OnCommand="lnkAddToCart_Command" Text='<%#AddToCartLinkText%>'></asp:LinkButton>
                    </div>
                    <p><%#Eval("SKUDescription") %></p>
                </div>
            </ItemTemplate>
        </cms:CMSRepeater>
    </div>
</div>
<Cart:CustomerCart runat="server" ID="crtCustomerCart" InventoryType='<%# ProductType %>'  />
