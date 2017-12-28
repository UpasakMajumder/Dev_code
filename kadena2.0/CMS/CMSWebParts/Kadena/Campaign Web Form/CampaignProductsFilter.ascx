<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/CampaignProductsFilter.ascx.cs" %>
<div class="row">
    <div class="col-sm-12">
        <div class="search__block searchproduct-width">
            <asp:TextBox ID="txtSearchProducts" runat="server" OnTextChanged="txtSearchProducts_TextChanged" class="input__text" AutoPostBack="true"></asp:TextBox>
        </div>
        <div class="custom__select webcustom__select">
            <asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div class="custom__select webcustom__select">
            <asp:DropDownList ID="ddlProductcategory" runat="server" OnSelectedIndexChanged="ddlProductcategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div class="add__btn ">
            <asp:Button ID="btnNotifyAdmin" runat="server" class="btn-action" OnClick="btnNotifyAdmin_Click" Visible="false" />
            <asp:Button ID="btnAllowUpates" runat="server" class="btn-action" OnClick="btnAllowUpates_Click" Visible="false" />
            <asp:Button ID="btnNewProduct" runat="server" class="btn-action" OnClick="btnNewProduct_Click" />
        </div>
    </div>
</div>
