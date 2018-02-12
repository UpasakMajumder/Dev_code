<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Product_SwapSKUNumberToCRN"  CodeBehind="~/CMSWebParts/Kadena/Product/SwapSKUNumberToCRN.ascx.cs" %>
Current Site name: <b><asp:Label ID="lblSiteName" runat="server"></asp:Label></b><br />
<asp:Button ID="btnGetRecords" runat="server" Text="Get Records" OnClick="btnGetRecords_Click" /> 
<asp:Label ID="lblTotalCount" runat="server"></asp:Label><br />
 
<p>
<asp:Button ID="btnUpdateCRN" runat="server" Text="Move SKU --> CRN" OnClick="btnUpdateCRN_Click" />
<asp:Label ID="lblUpdatedCRN" runat="server"></asp:Label><br />
</p>
<p>
<asp:Button ID="btnUpdateSKU" runat="server" Text="Update SKU->00000" OnClick="btnUpdateSKU_Click" />
<asp:Label ID="lblSKUUpdate" runat="server"></asp:Label><br />
</p>
<p>
<asp:Button ID="btnRevert" runat="server" Text="Revert CRN-->SKUNumber" OnClick="btnRevert_Click"/>
<asp:Label ID="lblRevert" runat="server"></asp:Label><br />
</p>