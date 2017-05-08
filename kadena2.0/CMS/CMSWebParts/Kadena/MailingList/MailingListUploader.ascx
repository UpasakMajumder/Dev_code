<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingListUploader.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.MailingListUploader" %>

<div class="upload-mail__drop-zone">
    <div class="drop-zone js-drop-zone">
        <button runat="server" id="btnHelp" type="button" class="drop-zone__btn question js-tooltip" data-tooltip-placement="left" title="">
            <svg class="icon icon-question">
                <use xlink:href="/gfx/svg/sprites/icons.svg#question-mark" />
            </svg>
        </button>
        <input runat="server" id="inpFile" accept="text/csv" type="file" name="file" class="js-drop-zone-file">
        <div class="drop-zone__dropping">
            <svg class="icon icon-drop">
                <use xlink:href="/gfx/svg/sprites/icons.svg#draganddrop" />
            </svg>
            <p runat="server" id="textFileToUpload" class="font-text"></p>
        </div>
        <div class="drop-zone__dropped">
            <div>
                <button type="button" class="drop-zone__btn close js-drop-zone-btn">
                    <svg class="icon icon-cross">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#cross" />
                    </svg>
                </button>
                <svg class="icon icon-csv">
                    <use xlink:href="/gfx/svg/sprites/icons.svg#csv" />
                </svg>
            </div>
            <p class="js-drop-zone-name">File name</p>
        </div>
    </div>
</div>
<div class="upload-mail__row upload-mail__offer">
    <span runat="server" id="textOr"></span>
    <p runat="server" id="textSkipField"></p>
</div>
<%--<div class="upload-mail__row">
    <h2>Mail type</h2>
    <p>First class guarantees next working day delivery, standart class usually takes 3-5 days</p>
    <div class="row">
        <div class="col-lg-4 col-xl-3">
            <div class="input__wrapper">
                <input name="mail-type" value="first-class" checked id="mail-type-first-class" type="radio" class="input__radio">
                <label for="mail-type-first-class" class="input__label input__label--radio">First Class</label>
            </div>
        </div>
        <div class="col-lg-4 col-xl-3">
            <div class="input__wrapper">
                <input name="mail-type" value="unsorted" id="mail-type-unsorted" type="radio" class="input__radio">
                <label for="mail-type-unsorted" class="input__label input__label--radio">Standart - Unsorted</label>
            </div>
        </div>
        <div class="col-lg-4 col-xl-3">
            <div class="input__wrapper">
                <input name="mail-type" value="sorted" id="mail-type-sorted" type="radio" class="input__radio">
                <label for="mail-type-sorted" class="input__label input__label--radio">Standart - Sorted</label>
            </div>
        </div>
    </div>
</div>--%>
<asp:PlaceHolder runat="server" ID="phMailType" />
<asp:PlaceHolder runat="server" ID="phProduct" />
<asp:PlaceHolder runat="server" ID="phValidity" />
<div class="upload-mail__row">
    <h2 runat="server" id="textFileName1"></h2>
    <p runat="server" id="textFileNameDescr"></p>
    <div class="row">
        <div class="col-lg-5 col-xl-3">
            <div class="input__wrapper">
                <span class="input__label" runat="server" id="textFileName2"></span>
                <input runat="server" id="inpFileName" type="text" name="name" class="input__text js-drop-zone-name-input" placeholder="">
            </div>
        </div>
    </div>
</div>
<button type="submit" class="btn-action" runat="server" id="btnSubmit" onserverclick="btnSubmit_Click"></button>
