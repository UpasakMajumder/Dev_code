<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="Code.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.SettingsSynchronization.Code"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Settings Synchronization - Code"
    Theme="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">

        <asp:PlaceHolder runat="server" ID="errorMessageContainer" Visible="false">
            <div class="alert-dismissable alert-error alert">
                <span class="alert-icon">
                    <i class="icon-times-circle"></i>
                    <span class="sr-only">Error</span>
                </span>
                <div class="alert-label">
                    <asp:Literal runat="server" ID="errorMessage"></asp:Literal>
                </div>
            </div>
        </asp:PlaceHolder>

        <h4>Find</h4>
        <div class="form-group">
            <div class="editing-form-value-cell">
                <div class="editing-form-control-nested-control">
                    <div class="control-group-inline">
                        <label class="control-label" style="text-align: left">Settings key:</label>
                        <input type="text" id="settingsKeyName" class="form-control" runat="server" />
                        <asp:Button Text="Get Code" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnGetCode" OnClick="btnGetCode_Click" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="editing-form-value-cell">
                <div class="editing-form-control-nested-control">
                    <div class="control-group-inline">
                        <label class="control-label" style="text-align: left">Code:</label>
                        <code>
                            <textarea rows="10" cols="150" class="" id="code" runat="server"></textarea>
                        </code>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
