<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/CampaignWebFormActions.ascx.cs" %>


<asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" Visible="false" Enabled="false"  CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkInitiate" runat="server" OnClick="lnkInitiate_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkViewProducts" runat="server" OnClick="lnkViewProducts_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkUpdateProducts" runat="server" OnClick="lnkUpdateProducts_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkOpenCampaign" runat="server" OnClick="lnkOpenCampaign_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>' class="js-tooltip" data-tooltip-placement="bottom" data-tooltipped="" data-original-title=" Click here to Open Campaign"></asp:LinkButton>
<asp:LinkButton ID="lnkCloseCampaign" runat="server" OnClick="lnkCloseCampaign_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>