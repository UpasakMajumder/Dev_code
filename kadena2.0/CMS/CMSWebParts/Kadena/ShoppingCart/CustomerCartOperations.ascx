<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerCartOperations.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.ShoppingCart.CustomerCartOperations" %>

<asp:Button Text="Add Items to  Cart" ID="btnDisplay" runat="server" OnClick="btmAddItemsToCart_Click"  />
<asp:Label Text="" ID="lblProductName" runat="server" />

<asp:GridView runat="server" ID="gvCustomersCart" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox Text="" ID="chkSelected" runat="server" Checked='<%# ValidationHelper.GetBoolean(Eval("IsSelected"),default(bool)) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CustomerID" HeaderText="Store ID" />
        <asp:BoundField DataField="CustomerEmail" HeaderText="Customer Name" />
        <asp:TemplateField>
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtQuanityOrdering" Text='<%# ValidationHelper.GetString(Eval("SKUUnits"),"0") %>' TextMode="Number" />
            </ItemTemplate>
        </asp:TemplateField>
        <%--        <asp:BoundField DataField="ShoppingCartInventryType" HeaderText="Cart Type" />--%>
        <asp:BoundField DataField="ShoppingCartID" Visible="true" HeaderText="" ItemStyle-CssClass="hide" />
        <asp:BoundField DataField="SKUID" Visible="true" HeaderText="" ItemStyle-CssClass="hide" />
    </Columns>
</asp:GridView>
