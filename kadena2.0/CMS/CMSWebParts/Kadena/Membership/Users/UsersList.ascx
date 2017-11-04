<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Kadena_Membership_Users_UsersList"  Codebehind="~/CMSWebParts/Kadena/Membership/Users/UsersList.ascx.cs" %>
<%@ Register Src="~/CMSWebParts/Kadena/Membership/Users/UsersFilterControl.ascx"
    TagName="UsersFilterControl" TagPrefix="uc1" %>
<uc1:UsersFilterControl ID="filterUsers" runat="server" />
<cms:BasicRepeater ID="repUsers" runat="server" />
<cms:UsersDataSource ID="srcUsers" runat="server" />
<div class="Pager">
    <cms:UniPager ID="pagerElem" runat="server" />
</div>
