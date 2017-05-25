<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.MailingList" %>

<asp:Repeater ID="repMailingLists" runat="server" EnableViewState="false">
    <HeaderTemplate>
        <table class="table">
            <tbody>
                <tr>
                    <th>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.MailingName" />
                    </th>
                    <th>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.DateAdded" />
                    </th>
                    <th>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.NumberOfAddresses" />
                    </th>
                    <th>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.NumberOfErrors" />
                    </th>
                </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <%# Eval("name") %>
            </td>
            <td>
                <%# Eval<DateTime>("createDate").ToString("MMM dd yyyy") %>
            </td>
            <td>
                <%# Eval("addressCount") %>
            </td>
            <td>
                <%# Eval("errorCount") %>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
