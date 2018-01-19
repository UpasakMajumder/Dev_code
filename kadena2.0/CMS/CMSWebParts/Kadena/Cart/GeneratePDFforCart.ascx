<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneratePDFforCart.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.GeneratePDFforCart" %>
<asp:LinkButton ID="lnkGeneratePDF" runat="server" OnClick="lnkGeneratePDF_Click" class="btn-action">
        <i class="fa fa-file-pdf-o" aria-hidden="true"></i><%=TotalCartPDFButtonText %>
</asp:LinkButton>
