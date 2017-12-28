<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DistributorCartDetails.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.DistributorCartDetails" %>
<div class="add__btn">
    <asp:LinkButton runat="server" ID="lnkSaveasPDF" OnClick="lnkSaveasPDF_Click" class="btn-action">
        <i class="fa fa-file-pdf-o" aria-hidden="true"></i><%=SaveasPDF %>
    </asp:LinkButton>
</div>
</div>
</div>
<div runat="server" id="tblCartItems" class="js-cartItems">
    <cms:QueryRepeater ID="rptCartItems" runat="server">
        <HeaderTemplate>
            <table class="table show-table-bottom">
                <tbody>
                    <tr>
                        <th><%= POSNumber %> </th>
                        <th><%= ProductName%> </th>
                        <th><%= Quantity %></th>
                        <th><%= Price %></th>
                        <th><%= Action %></th>
                    </tr>
        </HeaderTemplate>
    </cms:QueryRepeater>
    <tr>
        <td colspan="2"><%= Shipping %></td>
        <td>
            <asp:DropDownList runat="server" ID="ddlShippingOption" CssClass="select-list js-Shipping" EnableViewState="true" OnSelectedIndexChanged="ddlShippingOption_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
        </td>
        <td colspan="2">
            <asp:Label ID="lblShippingCharge" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="4"><%= BusinessUnit %></td>
        <td>
            <asp:DropDownList runat="server"  ID="ddlBusinessUnits" CssClass="js-BusinessUnit" EnableViewState="true" OnSelectedIndexChanged="ddlBusinessUnits_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
            <asp:Label ID="lblTotalUnits" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="3"><%= SubTotal %></td>
        <td>
            <asp:Label ID="lblTotalPrice" runat="server" />
        </td>
        <td></td>
    </tr>
    </tbody>
</table>
</div>
<div class="dialog" id="divDailogue" runat="server">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__content">
            <p>
                <asp:Label runat="server" ID="lblCartUpdateSuccess"></asp:Label></p>
            <p>
                <asp:Label runat="server" ID="lblCartError"></asp:Label></p>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <button type="button" class="btn-action btn-action--secondary js-CloseMesaage">Close</button>
            </div>
        </div>
    </div>
</div>