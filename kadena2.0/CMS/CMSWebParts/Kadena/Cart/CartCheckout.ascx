<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartCheckout.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Cart.CartCheckout" %>

<div class="add__btn">
    <asp:LinkButton runat="server" ID="lnkCheckout" OnClick="lnkCheckout_Click" class="btn-action"><%=CheckoutButtonText %></asp:LinkButton>
</div>
<div class="dialog" id="divErrorDailogue" runat="server">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__content">
            <p>
                <asp:Label runat="server" ID="lblCartUpdateSuccess"></asp:Label>
            </p>
            <cms:CMSRepeater runat="server" ID="rptErrors">
                <HeaderTemplate>
                    <table class="table show__table-bottom">
                        <tbody>
                            <tr>
                                <th><%= ResHelper.GetString("KDA.Checkout.DistributorName") %> </th>
                                <th><%= ResHelper.GetString("KDA.Checkout.ErrorReason") %></th>
                            </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval<string>("AddressPersonalName")%> </td>
                        <td><%# Eval<string>("Reason")%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </cms:CMSRepeater>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <button type="button" onclick="" class="btn-action btn-action--secondary js-CloseMesaage"><%=PopupCloseButtonText %></button>
            </div>
        </div>
    </div>
</div>
