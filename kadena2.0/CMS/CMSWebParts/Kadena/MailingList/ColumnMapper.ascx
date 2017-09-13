<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ColumnMapper.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.ColumnMapper" %>

<div class="map-columns__form map-columns-form j-klist-column-mapper">
    <div class="map-columns-form__group-wrapper">
        <asp:PlaceHolder runat="server" ID="phTitle" />

        <!--Title-->
        <div class=" input__wrapper ">
            <span class="input__label">TITLE</span>
            <span class="input__right-label"><%=GetString("Kadena.MailingList.Optional")%></span>
            <div class="input__select ">
                <select runat="server" id="selTitle" name="Title">
                </select>
            </div>
        </div>

        <!--FirstName-->
        <div runat="server" id="wrapFirstName" class=" input__wrapper ">
            <span class="input__label">name</span>
            <div runat="server" id="divFirstName" class="input__select ">
                <select runat="server" id="selFirstName" name="FirstName">
                </select>
            </div>
            <span runat="server" id="spanFirstName" class="input__error input__error--noborder" visible="false">Error</span>
        </div>

        <!--LastName-->
        <div class=" input__wrapper ">
            <span class="input__label">last name</span>
            <span class="input__right-label"><%=GetString("Kadena.MailingList.Optional")%></span>
            <div class="input__select ">
                <select runat="server" id="selLastName" name="LastName">
                </select>
            </div>
        </div>

        <!--Address1-->
        <div runat="server" id="wrapAddress1" class=" input__wrapper ">
            <span class="input__label">first address line</span>
            <div runat="server" id="divAddress1" class="input__select ">
                <select runat="server" id="selAddress1" name="Address1">
                </select>
            </div>
            <span runat="server" id="spanAddress1" class="input__error input__error--noborder" visible="false">Error</span>
        </div>

        <!--Address2-->
        <div runat="server" id="wrapAddress2" class=" input__wrapper ">
            <span class="input__label">second address line</span>
            <span class="input__right-label"><%=GetString("Kadena.MailingList.Optional")%></span>
            <div class="input__select ">
                <select runat="server" id="selAddress2" name="Address2">
                </select>
            </div>
        </div>

        <!--City-->
        <div runat="server" id="wrapCity" class=" input__wrapper ">
            <span class="input__label">city</span>
            <div runat="server" id="divCity" class="input__select ">
                <select runat="server" id="selCity" name="City">
                </select>
            </div>
            <span runat="server" id="spanCity" class="input__error input__error--noborder" visible="false">Error</span>
        </div>

        <!--State-->
        <div runat="server" id="wrapState" class=" input__wrapper ">
            <span class="input__label">State</span>
            <div runat="server" id="divState" class="input__select ">
                <select runat="server" id="selState" name="State">
                </select>
            </div>
            <span runat="server" id="spanState" class="input__error input__error--noborder" visible="false">Error</span>
        </div>

        <!--Zip-->
        <div runat="server" id="wrapZip" class=" input__wrapper ">
            <span class="input__label">Zip</span>
            <div runat="server" id="divZip" class="input__select ">
                <select runat="server" id="selZip" name="Zip">
                </select>
            </div>
            <span runat="server" id="spanZip" class="input__error input__error--noborder" visible="false">Error</span>
        </div>
    </div>
    <input id="inpErrorTitle" runat="server" type="hidden" class="j-error-title" enableviewstate="false" />
    <input id="inpErrorText" runat="server" type="hidden" class="j-error-text" enableviewstate="false" />
</div>
<div class="btn-group btn-group--left">
    <button type="button" class="btn-action btn-action--secondary" runat="server" id="btnReupload" onserverclick="btnReupload_ServerClick"></button>
    <button type="button" class="btn-action" runat="server" id="btnProcess" onserverclick="btnProcess_Click"></button>
</div>

