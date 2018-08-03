<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/CampaignProductsFilter.ascx.cs" %>

<div class="custom__block twe-search">
    <div class="custom__select">
        <asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlProductcategory" runat="server" OnSelectedIndexChanged="ddlProductcategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </div>
    <div class="search__block search__recent search__recent--icon">
        <asp:TextBox ID="txtSearchProducts" runat="server" OnTextChanged="txtSearchProducts_TextChanged" class="input__text" AutoPostBack="true"></asp:TextBox>
        <button class="search__submit btn--off" type="submit">
            <svg class="icon icon-dollar">
                <use xlink:href="/gfx/svg/sprites/icons.svg#search"></use>
            </svg>
        </button>
    </div>
    <div class="add__btn ">
        <asp:Button ID="btnNotifyAdmin" runat="server" class="btn-action" OnClick="btnNotifyAdmin_Click" Visible="false" />
        <asp:Button ID="btnAllowUpates" runat="server" class="btn-action" OnClick="btnAllowUpates_Click" Visible="false" />
        <asp:Button ID="btnNewProduct" runat="server" class="btn-action" OnClick="btnNewProduct_Click" />
    </div>
</div>
