<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusinessUnitControl.ascx.cs" Inherits="Kadena.CMSFormControls.Kadena.BusinessUnitControl" %>
<div class="input__wrapper allocated_block">
    <span class="input__label"></span>
    <a href="#" class="js-btnAssignBU" data-toggle="modal" data-target="#myModal_businessunits"><i class="fa fa-plus" aria-hidden="true"></i>Business Unit</a>
    <asp:HiddenField ID="hdnbuid" runat="server" ClientIDMode="Static" />
</div>

<div class="Business_Assigned_user">
    <table class="show-table js-buTable" id="UserBusinessUnit" style="display:none">
        <tbody id="UserBusinessUnitbody">
            <tr>
                <td>Bussiness Unit Name</td>
                <td>Bussiness Unit Number</td>
            </tr>
        </tbody>
    </table>
</div>







