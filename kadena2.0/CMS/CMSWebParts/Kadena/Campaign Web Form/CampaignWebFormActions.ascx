<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/CampaignWebFormActions.ascx.cs" %>


<asp:LinkButton ID="lnkEdit" runat="server" Text="Edit Camapign" OnClick="lnkEdit_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkInitiate" runat="server" Text="Initiate Camapign" OnClick="lnkInitiate_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkViewProducts" runat="server" Text="View Products" OnClick="lnkViewProducts_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkUpdateProducts" runat="server" Text="Update Products" OnClick="lnkUpdateProducts_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkOpenCampaign" runat="server" Text="Open Camapign" OnClick="lnkOpenCampaign_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>
<asp:LinkButton ID="lnkCloseCampaign" runat="server" Text="Close Camapign" OnClick="lnkCloseCampaign_Click" Visible="false" Enabled="false" CommandArgument='<%# CampaignID %>'></asp:LinkButton>