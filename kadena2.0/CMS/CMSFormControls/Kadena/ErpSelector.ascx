<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ErpSelector.ascx.cs" Inherits="Kadena.CMSFormControls.Kadena.ErpSelector" %>
<style>
    .erp-mapping-row {
        height: 25px !important;
        word-spacing: 0 !important;
    }
    .erp-mapping-col {
        padding-right: 5px !important;
        padding-bottom: 5px !important;
        word-spacing: 0 !important;
    }
</style>
<table>
    <tr class="erp-mapping-row">
        <td class="erp-mapping-col">
            <asp:Literal runat="server" ID="labelDrp"></asp:Literal>:<br />
            <asp:DropDownList ID="drpERP" runat="server" OnSelectedIndexChanged="drpERP_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
        </td>    
    </tr>
    <tr class="erp-mapping-row">
        <td class="erp-mapping-col">
            <asp:Literal runat="server" ID="labelInput"></asp:Literal>:<br />
            <asp:TextBox ID="inputCustomerERPID" runat="server" CssClass="form-control" OnTextChanged="inputCustomerERPID_TextChanged"></asp:TextBox>
        </td>
    </tr>
</table>