<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="S3Uploader.aspx.cs" Inherits="Kadena.CMSFormControls.S3Uploader"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="S3 Uploader"
    Theme="Default" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <asp:HyperLink runat="server" ID="lnkFile" ClientIDMode="Static" Target="_blank" />
    <cms:FormErrorLabel runat="server" ID="lblMessage" />
    <div>
        <input id="inpFile" clientidmode="Static" type="file" runat="server" style="display: none" />
        <cms:LocalizedButton runat="server" ID="btnUpload" ClientIDMode="Static" ButtonStyle="Default" EnableViewState="false" ResourceString="attach.uploadfile" />
    </div>
</asp:Content>
