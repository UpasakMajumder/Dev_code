<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplatesList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Chili.TemplatesList" %>

<div class="product-template__item">
    <h3>
        <cms:localizedliteral runat="server" enableviewstate="false" resourcestring="Kadena.Product.ManageTemplates" />
    </h3>
</div>
<div class="product-template__item">
    <asp:Repeater ID="repTemplates" runat="server" EnableViewState="false">
        <HeaderTemplate>
            <table class="table table--opposite table--inside-border table--hover product-list">
                <tbody>
                    <tr>
                        <th>
                            <cms:localizedliteral runat="server" enableviewstate="false" resourcestring="Kadena.Product.Name" />
                        </th>
                        <th>
                            <cms:localizedliteral runat="server" enableviewstate="false" resourcestring="Kadena.Product.DateCreated" />
                        </th>
                        <th>
                            <cms:localizedliteral runat="server" enableviewstate="false" resourcestring="Kadena.Product.DateUpdated" />
                        </th>
                        <th>&nbsp;</th>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="product-list__row js-redirection" data-url="<%# Eval("EditorUrl") %>">
                <td><a class="link weight--normal" href="<%# Eval("EditorUrl") %>"><%# Eval("Name") %></a></td>
                <td><%# Eval("DateCreated") %></td>
                <td><%# ((DateTime)Eval("DateUpdated")).Year == 1 ? string.Empty : Eval("DateUpdated") %></td>
                <td>
                    <div class="product-list__btn-group">
                        <a href="<%# Eval("EditorUrl") %>" class="btn-action product-list__btn--primary">
                            <cms:localizedlabel runat="server" enableviewstate="false" resourcestring="Kadena.Product.OpenInDesign" />
                        </a>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
         </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
