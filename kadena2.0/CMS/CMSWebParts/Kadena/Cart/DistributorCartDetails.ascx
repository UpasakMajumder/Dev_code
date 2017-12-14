<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DistributorCartDetails.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.DistributorCartDetails" %>
<div class="add_btn">
    <asp:LinkButton runat="server" ID="lnkSaveCartItems" OnClick="btnSaveCartItems_Click" class="btn-action">
        <i class="fa fa-edit" aria-hidden="true"></i><%=Save %>
    </asp:LinkButton>
</div>
<div class="add_btn">
    <a href="#" class="btn-action"><i class="fa fa-print" aria-hidden="true"></i><%= Print %></a>
</div>
<div class="add_btn">
    <asp:LinkButton runat="server" ID="lnkSaveasPDF"  class="btn-action">
        <i class="fa fa-file-pdf-o" aria-hidden="true"></i><%=SaveasPDF %>
    </asp:LinkButton>
</div>
</div>
 <div runat="server" id="tblCartItems" class="js-cartItems">
     <cms:QueryRepeater ID="rptCartItems" runat="server">
         <HeaderTemplate>
             <table class="show-table show-table-right selectdisable">
                 <tbody>
                     <tr>
                         <th><%= POSNumber %> </th>
                         <th><%= ProductName%> </th>
                         <th><%= Quantity %></th>
                         <th><%= Price %></th>
                         <th></th>
                     </tr>
         </HeaderTemplate>
     </cms:QueryRepeater>
     <tr>
         <td colspan="2"><%= Shipping %></td>
         <td>
             <asp:DropDownList runat="server" ID="ddlShippingOption" EnableViewState="true" AutoPostBack="true"></asp:DropDownList>
         </td>
         <td colspan="2">
             <asp:Label ID="lblShippingCharge" runat="server" />
         </td>
     </tr>
     <tr>
         <td colspan="4"><%= BusinessUnit %></td>
         <td>
             <asp:DropDownList runat="server" AutoPostBack="false" ID="ddlBusinessUnits"></asp:DropDownList>
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
     <asp:Label runat="server" ID="lblCartUpdateSuccess"></asp:Label>
     <asp:Label runat="server" ID="lblCartError"></asp:Label>
 </div>