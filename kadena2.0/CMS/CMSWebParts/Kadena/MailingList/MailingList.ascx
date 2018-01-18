<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.MailingList.MailingList" %>
<%@ Import Namespace="CMS.MacroEngine" %>
<%@ Import Namespace="Kadena.BusinessLogic.Services" %>

<div class="j-klist">
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
                        <th>
                            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.Action" />
                        </th>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("name") %>
                </td>
                <td>
                    <%# new DateTimeFormatter(Kadena2.Container.Default.ContainerBuilder.Resolve<Kadena.WebAPI.KenticoProviders.Contracts.IKenticoLocalizationProvider>()).Format(EvalDateTime("createDate"))  %>
                </td>
                <td>
                    <%# Eval("addressCount") %>
                </td>
                <td>
                    <%# Eval("errorCount") %>
                </td>
                <td class="text--center">
                    <a href="<%# ViewListUrl %>?containerId=<%# Eval("id") %>" class="mailing-lists__btn btn-action btn-action--secondary">
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.MailingList.View" />
                    </a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
        </table>
        </FooterTemplate>
    </asp:Repeater>
    <input id="inpError" runat="server" type="hidden" enableviewstate="false" class="j-error-message" />
</div>
