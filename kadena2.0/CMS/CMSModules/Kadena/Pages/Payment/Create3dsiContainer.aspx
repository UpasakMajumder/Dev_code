<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create3dsiContainer.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Payment.Create3dsiContainer"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Theme="default" %>




<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">

    <div id="m_pM_pMP">
		<div id="m_pM_lI" class="alert-dismissable alert-info alert" style="opacity: 1; position: absolute; max-width: 988px;">
            <span class="alert-icon">
                <i class="icon-i-circle"></i>
                <span class="sr-only">Info</span>
            </span>
            <div id="m_pM_lI_lbl" class="alert-label">
                This will call Credit Card Manager microservice to create or update customer container on 3DSi. Parameters are set in Kadena Settings, in sections 'Credit card payment' and 'Delivery price sender address'
            </div>
        </div>
        <div class="lblPlc m_pM_lI" style="height: 82px;"></div>
    </div>
	
    
    <div>
        <div>

                <div>
                    <div style="font-weight: bold">Customer code (container name):</div>
                    <div><asp:Literal runat="server" ID="ltlCode"/> </div>
                </div>

                <br />

                <div>
                    <div style="font-weight: bold">Create payload:</div>
                    <div><asp:Literal runat="server" ID="ltlCreatePayload"/> </div>
                    <div><asp:Button runat="server" CssClass="btn btn-default" Text="Create" ID="btnSubmit" OnClick="btnCreate_Click" /></div>
                </div>
                
                <br />

                <div>
                    <div style="font-weight: bold">Update payload:</div>
                    <div><asp:Literal runat="server" ID="ltlUpdatePayload"/> </div>
                    <asp:Button runat="server" CssClass="btn btn-default" Text="Update" ID="Button1" OnClick="btnUpdate_Click" />
                </div>

        </div>            
    </div>
    
    <br />

    <div>
        <cms:LocalizedLabel ID="LocalizedLabelResult" runat="server" CssClass="control-label" ForeColor="Black" EnableViewState="false" Text="" DisplayColon="false" />
    </div>
</asp:Content>



