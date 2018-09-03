<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Downloader.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.Downloader" %>

<div class="mailing-ready__block">
    <cms:LocalizedHeading Level="2" CssClass="mailing-ready__header" runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DownloadMaterials" />
    <div class="mailing-ready__links">
        <asp:LinkButton runat="server" ID="btnDownload" CssClass="link link--attachment" OnClick="btnDownload_Click">
            <svg class="icon icon-download">
                <use xlink:href="/gfx/svg/sprites/icons.svg#download--blue" />
            </svg>
            <cms:LocalizedLabel runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DownloadFile" />
        </asp:LinkButton>
    </div>
</div>
