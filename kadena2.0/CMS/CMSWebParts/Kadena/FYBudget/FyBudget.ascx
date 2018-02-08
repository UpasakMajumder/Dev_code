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

<script type="text/javascript" >
    $(".js-UserBudget").focusout(function () {
        var itemID = parseInt($(this).siblings().first().val());
        var userBudget = parseInt($(this).val());
        if (itemID != undefined && userBudget != undefined) {
            UpdateItem(itemID, userBudget);
        }
        $(".js-CloseMesaage").on("click", function () {
            $(this).closest(".dialog").removeClass("active");
        })
      function UpdateItem(itemID,userBudget){
          var request = { ItemID: itemID, UserBudget: userBudget }
            $.ajax({
              type: "POST",
              data :JSON.stringify(request),
              url: "api/userbudget",
              contentType: "application/json",
              success: function (data) {
                $('#autoSave_Dialog').addClass('active');
                if (data == 200) {
                  $('.response_Success').show();
                  $('.response_failure').hide();
                } 
                else{
                  $('.response_Success').hide();
                  $('.response_failure').show();
                }
              },
              error:function (xhr, ajaxOptions, thrownError) {
                $('.response_Success').hide();
                $('.response_failure').show();
              }
            });
      }
    }); 
</script>
