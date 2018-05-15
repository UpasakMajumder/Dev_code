<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Downloader.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.Downloader" %>

<div class="mailing-ready__block">
    <cms:LocalizedHeading Level="2" CssClass="mailing-ready__header" runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DownloadMaterials" />
    <div class="mailing-ready__links">
        <asp:HyperLink runat="server" ID="hlnkDownload" CssClass="link link--attachment">
            <svg class="icon icon-download">
                <use xlink:href="/gfx/svg/sprites/icons.svg#download--blue" />
            </svg>
            <cms:LocalizedLabel runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DownloadFile" />
        </asp:HyperLink>
    </div>
</div>
