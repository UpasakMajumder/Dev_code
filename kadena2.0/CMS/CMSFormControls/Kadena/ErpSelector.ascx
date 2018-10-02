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
            <asp:DropDownList ID="drpERP" runat="server" CssClass="form-control erp-selector" OnSelectedIndexChanged="drpERP_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
    <tr class="erp-mapping-row">
        <td class="erp-mapping-col">
            <asp:Literal runat="server" ID="labelInput"></asp:Literal>:             
            <asp:CustomValidator runat="server" ID="ErpCustomerIdValidator" ClientValidationFunction="ValidateErpCustomerId" ValidateEmptyText="true" ControlToValidate="inputCustomerERPID" Text="* Required" Font-Bold="true"></asp:CustomValidator>
            <br />
            <asp:TextBox ID="inputCustomerERPID" runat="server" CssClass="form-control erp-customer-id" OnTextChanged="inputCustomerERPID_TextChanged"></asp:TextBox>
        </td>
    </tr>
</table>

<script>

    function ValidateErpCustomerId(sender, args) {

        var erpSystem = $cmsj(".erp-selector").val();
        var erpCustomerId = $cmsj(".erp-customer-id").val();

        if (erpSystem != null && erpSystem.length > 0 && (erpCustomerId == null || erpCustomerId.length == 0)) {
            args.IsValid = false;  // field is empty
        }
        else {
            // do your other validation tests here...
        }
    }

    $cmsj(document).ready(function () {
        $cmsj(".erp-selector").on("change", function () {
            if ($cmsj(this).val() == "") {
                $cmsj(".erp-customer-id").val("");
            }
        });
    });

</script>