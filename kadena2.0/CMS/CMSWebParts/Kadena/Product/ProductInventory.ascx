<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Product_ProductInventory" CodeBehind="~/CMSWebParts/Kadena/Product/ProductInventory.ascx.cs" %>


<div class="custom_section">
    <div class="custom_block">
        <div class="custom_select">
            <select>
                <option>Select a program</option>
                <option>1</option>
                <option>1</option>
            </select>
            <select>
                <option>Select Product Category</option>
                <option>1</option>
                <option>1</option>
            </select>
        </div>
    </div>
    <div class="custom_content">
       <cms:CMSRepeater ID="rptProductList" runat="server" TransformationName="" WhereCondition=""></cms:CMSRepeater>
    </div>
</div>
