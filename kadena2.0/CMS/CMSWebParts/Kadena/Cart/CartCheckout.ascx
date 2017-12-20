<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartCheckout.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.CartCheckout" %>
<div class="add_btn">
    <asp:LinkButton runat="server" ID="lnkCheckout" OnClick="lnkCheckout_Click" class="btn-action"><%=CheckoutButtonText %></asp:LinkButton>
</div>
<div class="dialog" id="divErrorDailogue" runat="server">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__content">
            <p><asp:Label runat="server" ID="lblCartUpdateSuccess"></asp:Label></p>
            <p> <asp:Label runat="server" ID="lblCartError"></asp:Label></p>
            <asp:BulletedList runat="server" ID="lstErrors"></asp:BulletedList>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <button type="button" onclick="" class="btn-action btn-action--secondary js-CloseMesaage"><%=PopupCloseButtonText %></button>
            </div>
        </div>
    </div>
</div>