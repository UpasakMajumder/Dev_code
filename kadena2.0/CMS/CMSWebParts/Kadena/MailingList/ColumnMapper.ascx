<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ColumnMapper.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.ColumnMapper" %>

<div class="map-columns__form map-columns-form">
    <div class="map-columns-form__group-wrapper">
        <div class=" input__wrapper ">
            <span class="input__label">TITLE</span> <span class="input__right-label">optional</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>Select a title</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
        <div class=" input__wrapper ">
            <span class="input__label">NAME</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>CUSTOMER_NAME</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
        <div class=" input__wrapper ">
            <span class="input__label">LAST NAME</span> <span class="input__right-label">optional</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>Empty</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
        <div class=" input__wrapper ">
            <span class="input__label">FIRST ADRESS LINE</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>PREM_ADRESS1</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
        <div class=" input__wrapper ">
            <span class="input__label">SECOND ADRESS LINE</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>PREM_ADRESS2</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
        <div class=" input__wrapper ">
            <span class="input__label">CITY</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>PREM_CITY</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
        <div class=" input__wrapper ">
            <span class="input__label">STATE</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>PREM_STATE</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
        <div class=" input__wrapper ">
            <span class="input__label">ZIP CODE</span>
            <div class="input__select ">
                <select name="">
                    <option disabled selected>PREM_ZIP</option>
                    <option>option1</option>
                    <option>option2</option>
                    <option>option3</option>
                </select>
            </div>
        </div>
    </div>
</div>
<div class="btn-group btn-group--left">
    <button type="submit" class="btn-action btn-action--secondary">Reupload list</button>
    <button type="submit" class="btn-action">Process my list</button>
</div>
