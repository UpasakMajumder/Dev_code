<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Membership_Users_UsersList" CodeBehind="~/CMSWebParts/Kadena/Membership/Users/UsersList.ascx.cs" %>
<%@ Register Src="~/CMSWebParts/Kadena/Membership/Users/UsersFilterControl.ascx"
    TagName="UsersFilterControl" TagPrefix="uc1" %>

<cms:CMSPanel runat="server" ID="pnlListView">
    <uc1:UsersFilterControl ID="filterUsers" runat="server" />
    <cms:BasicRepeater ID="repUsers" runat="server" />
    <cms:UsersDataSource ID="srcUsers" runat="server" />
    <div class="Pager">
        <cms:UniPager ID="pagerElem" runat="server" />
    </div>
</cms:CMSPanel>

<cms:CMSPanel runat="server" ID="pnlUserForm">
    <div class="login__form-content js-login">
        <div class="css-login form_section">
            <cms:DataForm ID="formElem" runat="server" IsLiveSite="true" DefaultFormLayout="SingleTable" ValidateRequestMode="Enabled" ShowValidationErrorMessage="true" />
            <div class="mb-3 form_btns">
                <div>
                    <asp:Button runat="server" ID="btnSaveNew" CssClass="btn-action login__login-button btn--no-shadow js-btnSmarty" Text="Save" OnClick="btnSave_Click"/>
                    <cms:LocalizedLinkButton runat="server" ID="btnCancel" CssClass="btn-action login__login-button btn--no-shadow" Text="Cancel" OnClick="btnCancel_Click"></cms:LocalizedLinkButton>
                </div>
            </div>
        </div>
    </div>
</cms:CMSPanel>