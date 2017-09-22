<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create3dsiContainer.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Payment.Create3dsiContainer"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Theme="default" %>




<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">

    <div id="m_pM_pMP">
			<div id="m_pM_lI" class="alert-dismissable alert-info alert" style="opacity: 1; position: absolute; max-width: 988px;">
				<span class="alert-icon"><i class="icon-i-circle"></i><span class="sr-only">Info</span></span><div id="m_pM_lI_lbl" class="alert-label">This will call Credit Card Manager microservice to create customer container on 3DSi. The 'Customer code' parameter is set in Kadena Settings, in the section 'Credit card payment'</div>
			<span class="alert-close"><i class="close icon-modal-close"></i><span" class="sr-only">Close</span"></span></div><div class="lblPlc m_pM_lI" style="height: 82px;"></div>
		</div>

    <div class="form-horizontal">
        <div class="form-group">
                
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel ID="lblSite" runat="server" CssClass="control-label" EnableViewState="false" Text="Code" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <asp:TextBox CssClass="form-control" runat="server" ID="tbCode" ReadOnly="true" />
                <asp:Button runat="server" CssClass="btn btn-default" Text="Submit" ID="btnSubmit" OnClick="btnSubmit_Click" />
                <div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                    ControlToValidate="tbCode"
                    ErrorMessage="Code is a required field."
                    ForeColor="Red">
                </asp:RequiredFieldValidator>
                </div>
            </div>            
        </div>        
    </div>
    
    <div>
        <cms:LocalizedLabel ID="LocalizedLabelResult" runat="server" CssClass="control-label" ForeColor="Black" EnableViewState="false" Text="" DisplayColon="false" />
    </div>
</asp:Content>



