<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Product_ProductInventory" CodeBehind="~/CMSWebParts/Kadena/Product/ProductInventory.ascx.cs" %>

<div class="custom_section">
    <div class="custom_block">
        <div class="custom_select">
           <asp:DropDownList ID="ddlProgram" runat="server" Visible="false" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
           <asp:DropDownList ID="ddlCategory" runat="server" Visible="false" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
    </div>
    <div class="custom_content">
        <cms:CMSRepeater ID="rptProductList" runat="server">
            <ItemTemplate>
                <div class="cus_content_block">
                    <div class="img_block">
                       <img src='<%#GetProductImage(Eval("SKUImagePath"))%>'/>
                    </div>
                    <i class="fa fa-search" aria-hidden="true"></i>
                    <div class="custom_blockin">
                        <h4><%# Eval("SKUNumber")%></h4>
                        <h3><%#Eval("SKUName") %></h3>
                        <span>$<%#Eval("SKUPrice") %>pack of <%#Eval("SKUAvailableItems") %></span>
                        <h6>Current Demand:</h6>
                        <a href="#">Add to Cart</a>
                    </div>
                    <p><%#Eval("SKUDescription") %></p>
                </div>
            </ItemTemplate>
        </cms:CMSRepeater>
    </div>
</div>
