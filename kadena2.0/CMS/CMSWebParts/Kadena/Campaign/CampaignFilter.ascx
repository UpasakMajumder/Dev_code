<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignFilter.ascx.cs" Inherits="CMSApp.CMSWebParts.Kadena.Campaign.CampaignFilter" %>
<div class="search_block">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="input__text" onkeypress="FireOnClickButton()"></asp:TextBox>
    
      <cms:LocalizedButton ID="btnFilter" runat="server" Text="Apply Filter" style="visibility: hidden; display: none;"/>
      </div>

<script type="text/javascript">
function FireOnClickButton() { 
document.getElementByID("Button1").click();
 
}
</script>
 
 