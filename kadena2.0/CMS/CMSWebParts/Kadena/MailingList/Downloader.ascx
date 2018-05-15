<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Downloader.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.Downloader" %>

<div class="mailing-ready__block">
    <cms:LocalizedHeading Level="2" CssClass="mailing-ready__header" runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DownloadMaterials" />
    <div class="mailing-ready__links">
        <a class="link link--attachment" href="/klist/export/<% =Request.QueryString["containerId"] %>" download="<% =Request.QueryString["containerId"] %>">
            <svg class="icon icon-download">
                <use xlink:href="/gfx/svg/sprites/icons.svg#download--blue" />
            </svg>
            <cms:LocalizedLabel runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DownloadFile" />
        </a>
    </div>
</div>
