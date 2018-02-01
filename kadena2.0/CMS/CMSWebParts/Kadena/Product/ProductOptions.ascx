<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductOptions.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.ProductOptions" %>


<div class="js-product-options" data-price-element='#<% =PriceElementName %>' data-url='<% =PriceUrl %>''>
    <div class="product-options product-options--select">
        
        <div class=" input__wrapper ">
            <div class="input__select ">
                <select name="size" class="js-product-option js-add-to-cart-property">
                    <option disabled selected>(Please select)</option>
                    <option>red</option>
                    <option>blue</option>
                    <option>green</option>
                </select>
            </div>
        </div>
        
        <div class=" input__wrapper ">
            <div class="input__select ">
                <select name="color" class="js-product-option js-add-to-cart-property">
                    <option disabled selected>(Please select)</option>
                    <option>s</option>
                    <option>m</option>
                    <option>l</option>
                </select>
            </div>
        </div>

    </div>
</div>
