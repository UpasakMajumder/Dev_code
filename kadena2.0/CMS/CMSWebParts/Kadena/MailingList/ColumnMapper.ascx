<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ColumnMapper.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.ColumnMapper" %>

<div class="map-columns__form map-columns-form">
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
        <div class=" input__wrapper ">
            <span class="input__label">name</span>
            <div runat="server" id="divFirstName" class="input__select ">
                <select runat="server" id="selFirstName" name="FirstName">
                </select>
                <span runat="server" id="spanFirstName" class="input__error input__error--noborder" visible="false">Error</span>
            </div>
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
        <div class=" input__wrapper ">
            <span class="input__label">first address line</span>
            <div runat="server" id="divAddress1" class="input__select ">
                <select runat="server" id="selAddress1" name="Address1">
                </select>
                <span runat="server" id="spanAddress1" class="input__error input__error--noborder" visible="false">Error</span>
            </div>
        </div>

        <!--Address2-->
        <div class=" input__wrapper ">
            <span class="input__label">second address line</span>
            <div runat="server" id="divAddress2" class="input__select ">
                <select runat="server" id="selAddress2" name="Address2">
                </select>
                <span runat="server" id="spanAddress2" class="input__error input__error--noborder" visible="false">Error</span>
            </div>
        </div>

        <!--City-->
        <div class=" input__wrapper ">
            <span class="input__label">city</span>
            <div runat="server" id="divCity" class="input__select ">
                <select runat="server" id="selCity" name="City">
                </select>
                <span runat="server" id="spanCity" class="input__error input__error--noborder" visible="false">Error</span>
            </div>
        </div>

        <!--State-->
        <div class=" input__wrapper ">
            <span class="input__label">State</span>
            <div runat="server" id="divState" class="input__select ">
                <select runat="server" id="selState" name="State">
                </select>
                <span runat="server" id="spanState" class="input__error input__error--noborder" visible="false">Error</span>
            </div>
        </div>

        <!--Zip-->
        <div class=" input__wrapper ">
            <span class="input__label">Zip</span>
            <div runat="server" id="divZip" class="input__select ">
                <select runat="server" id="selZip" name="Zip">
                </select>
                <span runat="server" id="spanZip" class="input__error input__error--noborder" visible="false">Error</span>
            </div>
        </div>
    </div>
</div>
<div class="btn-group btn-group--left">
    <button type="submit" class="btn-action btn-action--secondary" runat="server" id="btnReupload" onserverclick="btnReupload_ServerClick"></button>
    <button type="submit" class="btn-action" runat="server" id="btnProcess" onserverclick="btnProcess_Click"></button>
</div>
