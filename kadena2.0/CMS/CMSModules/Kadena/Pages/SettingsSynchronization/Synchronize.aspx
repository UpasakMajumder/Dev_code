﻿<%@ 
    Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="Synchronize.aspx.cs" 
    Inherits="Kadena.CMSModules.Kadena.Pages.SettingsSynchronization.Synchronize"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" 
    Title="Settings Synchronization - Synchronize" 
    Theme="Default"
    %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        
        <div class="form-group" style="margin-bottom: 3rem">
            <asp:Button Text="Synchronize" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnSynchronizeSettings" OnClick="btnSynchronizeSettings_Click" runat="server" />
        </div>

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
        <asp:PlaceHolder runat="server" ID="successMessageContainer" Visible="false">
            <div class="alert-dismissable alert-info alert">
                <span class="alert-icon">
                    <i class="icon-check-circle"></i>
                    <span class="sr-only">OK</span>
                </span>
                <div class="alert-label">
                    <asp:Literal runat="server" ID="successMessage"></asp:Literal>
                </div>
            </div>
        </asp:PlaceHolder>

    </div>
</asp:Content>
