<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create3dsiContainer.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Payment.Create3dsiContainer"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Theme="default" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
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
            <div>
                <cms:LocalizedLabel ID="LocalizedLabelResult" runat="server" CssClass="control-label" EnableViewState="false" Text="Code" DisplayColon="true" />
            </div>
        </div>
    </div>
</asp:Content>



