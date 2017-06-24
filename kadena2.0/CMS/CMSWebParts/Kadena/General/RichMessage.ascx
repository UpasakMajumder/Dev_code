<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RichMessage.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.General.RichMessage" %>

<div class="notification js-close-this" data-animation-length="400">
    <div class="notification__header">
        <p>
            <asp:Literal ID="ltlHeaderText" runat="server" EnableViewState="false" />
        </p>
    </div>
    <div class="notification__body">
        <p>
            <cms:LocalizedLiteral ID="ltlContent" runat="server" EnableViewState="false" />
        </p>
        <div class="notification__body-footer btn-group btn-group--right">
            <asp:HyperLink ID="lnkSecondary" runat="server" EnableViewState="false" CssClass="btn-success btn-success--secondary" Visible="false" />

            <button type="button" class="btn-success js-close-this-trigger">
                <asp:Literal ID="ltlPrimaryButtonText" runat="server" EnableViewState="false" />
            </button>
        </div>
    </div>
</div>
