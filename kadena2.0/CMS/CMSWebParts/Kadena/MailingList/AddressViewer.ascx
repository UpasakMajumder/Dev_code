<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressViewer.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.AddressViewer" %>

<asp:Panel runat="server" ID="pnlBadAddresses" CssClass="processed-list__table-block" Visible="false">
    <div class="processed-list__table-heading processed-list__table-heading--error">
        <h3>We have found 8 errors in your file</h3>
        <div class="btn-group btn-group--right">
            <button type="button" class="btn-action btn-action--secondary" style="display:none">Reload .csv</button>
            <button type="button" data-dialog="#mail-list-errors" class="js-dialog btn-action btn-action--secondary" style="display: none">Correct errors</button>
        </div>
    </div>
    <div class="processed-list__table-inner">
        <asp:Table runat="server" ID="tblBadAddresses" CssClass="table processed-list__table--shadow" />
        <span class="processed-list__table-helper">To correct your .csv file, view the line number, go to your original file and check the respective record.
            <svg class="icon help-arrow">
                <use xlink:href="/gfx/svg/sprites/icons.svg#info-arrow" />
            </svg>
        </span>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlGoodAddresses" CssClass="processed-list__table-block" Visible="false">
    <div class="processed-list__table-heading processed-list__table-heading--success">
        <h3>332 addresses have been processed successfuly</h3>
        <div class="btn-group btn-group--right">
            <button type="button" class="btn-action btn-action--secondary" style="display:none">Use only correct</button>
        </div>
    </div>
    <div class="processed-list__table-inner">
        <asp:Table runat="server" ID="tblGoodAddresses" CssClass="table processed-list__table--shadow" />
    </div>
</asp:Panel>
<div class="btn-group btn-group--left">
    <button type="button" class="btn-action">Save list</button>
</div>
