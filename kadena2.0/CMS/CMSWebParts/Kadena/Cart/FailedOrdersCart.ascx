<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FailedOrdersCart.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.FailedOrdersCart" %>
<div runat="server" id="tblCartItems" class="js-cartItems">
    <cms:QueryRepeater ID="rptCartItems" runat="server">
        <HeaderTemplate>
            <table class="table show__table-bottom">
                <tbody>
                    <tr>
                        <th><%= POSNumber %> </th>
                        <th><%= ProductName%> </th>
                        <th><%= Quantity %></th>
                        <th><%= Price %></th>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <td><%# Eval("SKUNumber") %></td>
            <td><%# Eval("SKUName") %> </td>
            <td>
                <asp:Label runat="server" ID="lblUnits" Text='<%# Eval("SKUUnits") %>'></asp:Label>
                <asp:HiddenField runat="server" ID="hdnCartItemID" Value='<%# Eval("CartItemID") %>' />
            </td>
            <td>
                <asp:Label runat="server" ID="lblPrice" Text='<%#(EvalInteger("ShoppingCartInventoryType")==1) ? (CMS.Ecommerce.CurrencyInfoProvider.GetFormattedPrice(0, CurrentSite.SiteID)):(CMS.Ecommerce.CurrencyInfoProvider.GetFormattedPrice(EvalInteger("SKUUnits")*EvalDouble("SKUPrice"), CurrentSite.SiteID))%>'></asp:Label>
                <asp:HiddenField runat="server" ID="hdnSKUPrice" Value='<%# (EvalInteger("ShoppingCartInventoryType")==1) ? 0 : EvalInteger("SKUUnits")*EvalDouble("SKUPrice") %>' />
            </td>
            </tr>
        </ItemTemplate>
    </cms:QueryRepeater>
    <tr>
        <td colspan="2" ><%= Shipping %></td>
        <td>
            <asp:Label runat="server" ID="lblShippingOption" Visible="false"></asp:Label>
        </td>
        <td >
            <asp:Label ID="lblShippingCharge" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="3"><%= BusinessUnit %></td>
        <td>
            <asp:DropDownList runat="server" ID="ddlBusinessUnits" CssClass="js-BusinessUnit" EnableViewState="true" ></asp:DropDownList>
            <asp:Label ID="lblTotalUnits" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="3"><%= SubTotal %></td>
        <td>
            <asp:Label ID="lblTotalPrice" runat="server" />
        </td>
    </tr>
    </tbody>
</table>
    <asp:HiddenField runat="server" ID="hdnDeleteSuccess" />
</div>
<div class="dialog" id="divDailogue" runat="server">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__content">
            <p>
                <asp:Label runat="server" ID="lblCartUpdateSuccess"></asp:Label>
            </p>
            <p>
                <asp:Label runat="server" ID="lblCartError"></asp:Label>
            </p>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <button type="button" class="btn-action btn-action--secondary js-CloseMesaage">Close</button>
            </div>
        </div>
    </div>
</div>