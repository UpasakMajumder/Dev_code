<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplatesList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Chili.TemplatesList" %>

<div class="product-template__item">
    <h3>
        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.Product.ManageTemplates" />
    </h3>
</div>
<div class="product-template__item">
    <asp:Repeater ID="repTemplates" runat="server" EnableViewState="false">
        <HeaderTemplate>
            <table class="table table--opposite table--inside-border table--hover product-list">
                <tbody>
                    <tr>
                        <th>ID</th>
                        <th>Date changed</th>
                        <th>&nbsp;</th>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="product-list__row js-redirection" data-url="<%# Eval("EditorUrl") %>">
                <td><a class="link weight--normal" href="<%# Eval("EditorUrl") %>"><%# Eval("TemplateID") %></a></td>
                <td><%# Eval("Date") %></td>
                <td>
                    <div class="product-list__btn-group">
                        <a href="<%# Eval("EditorUrl") %>" class="btn-action product-list__btn--primary">
                            <cms:LocalizedLabel runat="server" EnableViewState="false" ResourceString="Kadena.Product.OpenInDesign" />
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
