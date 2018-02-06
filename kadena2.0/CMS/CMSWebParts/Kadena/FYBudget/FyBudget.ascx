<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_FYBudget_FyBudget" CodeBehind="~/CMSWebParts/Kadena/FYBudget/FyBudget.ascx.cs" %>

<asp:Repeater runat="server" ID="fiscalDatagrid">
    <HeaderTemplate>
        <table class="table show__table-bottom">
            <tbody>
                <tr>
                    <th><%= BudgetYearText %> </th>
                    <th><%= UserBudgetText%> </th>
                    <th><%= UserRemainingBudgetText %></th>
                </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:Label runat="server" ID="fiscalYear" Text='<%#Eval("Year") %>'></asp:Label>
            </td>
            <td>
                <asp:HiddenField runat="server" ID="hdnItemID" Value='<%#Eval("ItemID") %>' />
                <asp:Label runat="server" ID="userBudget" Visible='<%# (ValidationHelper.GetBoolean(Eval("IsYearEnded"),true)) %>' Text='<%#Eval("Budget") %>'></asp:Label>
                <asp:TextBox runat="server" CssClass="js-UserBudget"  ID="txtUserBudget" Visible='<%# !(ValidationHelper.GetBoolean(Eval("IsYearEnded"),true)) %>' TextMode="Number" Text='<%#Eval("Budget") %>'></asp:TextBox>
            </td>
            <td>
                <asp:Label runat="server" ID="userRemainingBudget" Text='<%#Eval("UserRemainingBudget") %>'></asp:Label>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
       </table>
    </FooterTemplate>
</asp:Repeater>


