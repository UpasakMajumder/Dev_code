<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerCartOperations.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.ShoppingCart.CustomerCartOperations" %>


<div class="dialog active" id="dialog_Add_To_Cart">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__header">
            <asp:Label Text="" ID="lblProductName" runat="server" />
        </div>
        <div class="dialog__content">
            <asp:Label Text="" ID="lblError" Visible="false" runat="server" />
            <cms:LocalizedLabel runat="server" ID="lblErrorMsg" Visible="false"></cms:LocalizedLabel><br />
            <asp:Label Text="" runat="server" ID="lblAvailbleItems" /><br />
            <asp:GridView runat="server" ID="gvCustomersCart" AutoGenerateColumns="false" CssClass="show-table">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox Text="" ID="chkSelected" runat="server" Checked='<%# ValidationHelper.GetBoolean(Eval("IsSelected"),default(bool)) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AddressID" />
                    <asp:BoundField DataField="AddressPersonalName" />
                    <asp:TemplateField>
                        <HeaderTemplate><%# ResHelper.GetString("KDA.ShoppingCart.Quantity") %></HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" CssClass="input__text" ID="txtQuanityOrdering" Text='<%# ValidationHelper.GetString(Eval("SKUUnits"),"0") %>' TextMode="Number" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ShoppingCartID" Visible="true" HeaderText="" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible" />
                    <asp:BoundField DataField="SKUID" Visible="true" HeaderText="" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible" />
                </Columns>
            </asp:GridView>
            <asp:Label runat="server" ID="lblSuccessMsg"></asp:Label>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <button type="button" class="btn-action btn-action--secondary" id="btnClose"><%= CartCloseText %></button>
                <cms:LocalizedLinkButton runat="server" ID="llbtnAddToCart" ResourceString="KDA.ShoppingCart.AddItemsToCart" CssClass="btn__action" OnClick="btmAddItemsToCart_Click"></cms:LocalizedLinkButton>
            </div>
        </div>
    </div>
</div>
