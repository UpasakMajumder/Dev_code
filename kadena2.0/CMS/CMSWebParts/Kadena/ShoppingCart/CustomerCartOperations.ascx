<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerCartOperations.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.ShoppingCart.CustomerCartOperations" %>

<div class="modal_popup">
    <div class="modal-content">
        <div class="modal_header">
            <asp:Button Text="Add Items to  Cart" ID="btnDisplay" runat="server" CssClass="btn-action" OnClick="btmAddItemsToCart_Click" />
            <a href="#" class="btn_close"><i class="fa fa-close"></i></a>
        </div>
        <div class="modal_body">
            <asp:Label Text="" ID="lblProductName" runat="server" />
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
                    <asp:BoundField DataField="AddressID" HeaderText="Store ID" />
                    <asp:BoundField DataField="AddressPersonalName" HeaderText="Customer Name" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Quantity
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" CssClass="input__text" ID="txtQuanityOrdering" Text='<%# ValidationHelper.GetString(Eval("SKUUnits"),"0") %>' TextMode="Number" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ShoppingCartID" Visible="true" HeaderText="" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible" />
                    <asp:BoundField DataField="SKUID" Visible="true" HeaderText="" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible" />
                </Columns>
            </asp:GridView>
            <asp:Label runat="server" ID="lblSuccessMsg" Text=""></asp:Label>
        </div>
    </div>
</div>
