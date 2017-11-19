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
<!--pop up html-->
<div class="modal_popup modal_businessunit" id="myModal_businessunits" style="display: none">
    <div class="modal-content">
        <div class="modal_header clearfix">
            <button type="button" class="btn-action js-btnSaveBU">Add Business Units</button>
            <a href="#" class="btn_close"><i class="fa fa-close"></i></a>
        </div>
        <div class="modal_body">
            <table class="show-table" id="buinessunits">
                <tbody id="buinessunitsbody">
                    <tr>
                        <td>
                            <input type="checkbox" class="js-chkAll" id="selectAll">
                        </td>
                        <td>Bussiness Unit Name</td>
                        <td>Bussiness Unit Number</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>



<%--<div class="container">
  <h2>Modal Example</h2>
  <!-- Trigger the modal with a button -->
  <button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal">Open Modal</button>

  <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>

          <h4 class="modal-title">Modal Header</h4>
        </div>
        <div class="modal-body">
          <p>Some text in the modal.</p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
  
</div>--%>

