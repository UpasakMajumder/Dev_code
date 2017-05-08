<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ColumnMapper.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.ColumnMapper" %>

<div class="map-columns__form map-columns-form">
    <div class="map-columns-form__group-wrapper">
        <asp:PlaceHolder runat="server" ID="phTitle" />
        <%--<div class=" input__wrapper ">
            <span class="input__label">TITLE</span>
            <span class="input__right-label">optional</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>Select a title</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>--%>
    </div>
</div>
<div class="btn-group btn-group--left">
    <button type="submit" class="btn-action btn-action--secondary" runat="server" id="btnReupload" onserverclick="btnReupload_ServerClick">Reupload list</button>
    <button type="submit" class="btn-action" runat="server" id="btnProcess" onserverclick="btnProcess_Click">Process my list</button>
</div>
