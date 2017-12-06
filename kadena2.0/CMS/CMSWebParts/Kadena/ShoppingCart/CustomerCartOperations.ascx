<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerCartOperations.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.ShoppingCart.CustomerCartOperations" %>

<asp:Button Text="Add Items to  Cart" ID="btnDisplay" runat="server" OnClick="btmAddItemsToCart_Click" /><br />
<asp:Label Text="" ID="lblProductName" runat="server" />
<asp:Label Text="" ID="lblError" runat="server" />
<cms:LocalizedLabel runat="server" ID="lblErrorMsg" Visible="false" ResourceString="" ></cms:LocalizedLabel><br />
<asp:Label Text="" runat="server" ID="lblAvailbleItems" /><br /><br />

<asp:GridView runat="server" ID="gvCustomersCart" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox Text="" ID="chkSelected" runat="server" Checked='<%# ValidationHelper.GetBoolean(Eval("IsSelected"),default(bool)) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="AddressID" HeaderText="Store ID" />
        <asp:BoundField DataField="AddressPersonalName" HeaderText="Customer Name" />
        <asp:TemplateField>
            <HeaderTemplate>
                Quantity
            </HeaderTemplate>
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtQuanityOrdering" Text='<%# ValidationHelper.GetString(Eval("SKUUnits"),"0") %>' TextMode="Number" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ShoppingCartID" Visible="true" HeaderText="" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
        <asp:BoundField DataField="SKUID" Visible="true" HeaderText="" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
    </Columns>
</asp:GridView>
<asp:Label runat="server" ID="lblSuccessMsg" Text=""></asp:Label>
<style>
    .hide {
        display: none;
    }
</style>
