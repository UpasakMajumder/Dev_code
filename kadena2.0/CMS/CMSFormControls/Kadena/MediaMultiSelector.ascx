<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaMultiSelector.ascx.cs" Inherits="Kadena.CMSFormControls.Kadena.MediaMultiSelector" %>
<cms:CMSUpdatePanel runat="server">
    <ContentTemplate>
        <asp:Repeater ID="ItemsRepeater" runat="server" ItemType="System.string">
            <ItemTemplate>
                <cms:MediaSelector runat="server" Value='<%# Item %>' ShowPreview="false" style="margin-bottom: 6px;"/>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Button CssClass="btn btn-primary" ID="AddButton" OnClick="AddButton_Click" Text="Add" runat="server" />
    </ContentTemplate>
</cms:CMSUpdatePanel>