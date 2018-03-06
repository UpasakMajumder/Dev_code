<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Cart_CartDistributorList" CodeBehind="~/CMSWebParts/Kadena/Cart/CartDistributorList.ascx.cs" %>

<div class="dialog" id="CartDistributorListModal">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__header">
            <h5>
                <cms:LocalizedLiteral runat="server" ResourceString="KDA.AddToCart.Popup.HeaderText"></cms:LocalizedLiteral>
            </h5>
        </div>
        <div class="dialog__content">
            <span id="ProductName"></span><br />
            <span id="AvailableStock" data-text='<%=ResHelper.GetString("Kadena.AddToCart.StockAvilable")%>'></span>
            <cms:CMSRepeater runat="server" ID="rptCartDistributorList" TransformationName="KDA.Transformations.CartDistributorList">
                <HeaderTemplate>
                    <table class="table">
                        <thead>
                            <tr>
                                <th><cms:LocalizedLiteral runat="server" ResourceString="KDA.ShoppingCart.StoreID"></cms:LocalizedLiteral></th>
                                <th><cms:LocalizedLiteral runat="server" ResourceString="KDA.ShoppingCart.CustomerName"></cms:LocalizedLiteral></th>
                                <th><cms:LocalizedLiteral runat="server" ResourceString="KDA.ShoppingCart.Quantity"></cms:LocalizedLiteral></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </cms:CMSRepeater>
            <asp:HiddenField runat="server" ID="hdnInventoryType" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnNoDistributorsError" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnInsufficientStockError" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnMoreThanAllocatedError" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnCartUpdatedText" ClientIDMode="Static" />
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <span class="btn-action btn-action--secondary" onclick="$('#CartDistributorListModal').toggleClass('active');">
                    <cms:LocalizedLiteral runat="server" ResourceString="KDA.ShoppingCart.Close"></cms:LocalizedLiteral>
                </span>
                <span class="btn-action js-update-distributor-cart">
                    <cms:LocalizedLiteral runat="server" ResourceString="KDA.ShoppingCart.AddItemsToCart"></cms:LocalizedLiteral>
                </span>
            </div>
        </div>
    </div>
</div>
