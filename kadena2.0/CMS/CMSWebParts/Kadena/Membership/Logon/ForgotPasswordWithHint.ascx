<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Membership_Logon_ForgotPasswordWithHint" CodeBehind="~/CMSWebParts/Kadena/Membership/Logon/ForgotPasswordWithHint.ascx.cs" %>

<asp:Panel ID="pnlBody" runat="server" CssClass="logon-page-background">
    <%-- Logon part --%>
    <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/Default.aspx" RenderOuterTable="false" Visible="false">
        <LayoutTemplate>
            <asp:Panel runat="server" ID="pnlLogin" DefaultButton="LoginButton" CssClass="logon-panel">
                <cms:LocalizedLabel ID="lblTokenInfo" runat="server" EnableViewState="False" Visible="false" CssClass="logon-token-info" />
                <%-- Form start --%>
                <div class="form-horizontal" role="form">

                    <%-- Token ID step --%>
                    <asp:PlaceHolder runat="server" ID="plcTokenInfo" Visible="false">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblTokenIDlabel" runat="server" CssClass="control-label" ResourceString="mfauthentication.label.token" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:LocalizedLabel ID="lblTokenID" runat="server" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <%-- Logon step --%>
                    <asp:PlaceHolder runat="server" ID="plcLoginInputs">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblUserName" runat="server" AssociatedControlID="UserName" CssClass="control-label" ResourceString="LogonForm.UserName" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="UserName" runat="server" MaxLength="100" />
                                <cms:CMSRequiredFieldValidator ID="rfvUserNameRequired" runat="server" ControlToValidate="UserName" Display="Dynamic" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblPassword" runat="server" AssociatedControlID="Password" CssClass="control-label" ResourceString="LogonForm.Password" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="Password" runat="server" TextMode="Password" MaxLength="110" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <%-- Passcode step --%>
                    <asp:PlaceHolder runat="server" ID="plcPasscodeBox" Visible="false">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblPasscode" runat="server" AssociatedControlID="txtPasscode" CssClass="control-label" ResourceString="mfauthentication.label.passcode" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtPasscode" runat="server" MaxLength="110" />
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <%-- Form End --%>
                </div>

                <cms:CMSCheckBox ID="chkRememberMe" runat="server" ResourceString="LogonForm.RememberMe" CssClass="logon-remember-me-checkbox" />
                <cms:LocalizedLabel ID="FailureText" runat="server" EnableViewState="False" CssClass="error-label" Visible="false" />

                <cms:LocalizedButton ID="LoginButton" runat="server" ButtonStyle="Primary" CommandName="Login" EnableViewState="false" ResourceString="LogonForm.LogOnButton" />
            </asp:Panel>
        </LayoutTemplate>
    </asp:Login>


    <%-- Password retrieval part --%>
    <cms:CMSUpdatePanel runat="server" ID="pnlUpdatePasswordRetrievalLink" UpdateMode="Conditional">
        <ContentTemplate>
            <cms:LocalizedLinkButton ID="lnkPasswdRetrieval" runat="server" EnableViewState="false" OnClick="lnkPasswdRetrieval_Click"
                CssClass="link mb-2" ResourceString="LogonForm.lnkPasswordRetrieval" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>

    <cms:CMSUpdatePanel runat="server" ID="pnlUpdatePasswordRetrieval" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlPasswdRetrieval" runat="server" CssClass="logon-panel-password-retrieval" DefaultButton="btnPasswdRetrieval" Visible="False">

                <%-- Form start --%>
                <div class="form-horizontal mb-2" role="form">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel ID="lblPasswdRetrieval" runat="server" EnableViewState="false" AssociatedControlID="txtPasswordRetrieval"
                                CssClass="control-label" ResourceString="E-MAIL" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtPasswordRetrieval" CssClass="input__text" placeholder="your@email.com" runat="server" />
                        </div>
                    </div>
                    <%-- Form End --%>
                </div>

                <cms:LocalizedButton ID="btnPasswdRetrieval" runat="server" EnableViewState="false" ButtonStyle="Default"
                    CssClass="btn-action login__login-button btn--no-shadow" ResourceString="LogonForm.btnPasswordRetrieval" />
                <asp:Label ID="lblResult" runat="server" Visible="false" EnableViewState="false" CssClass="logon-password-retrieval-result" />
                <asp:Label ID="lblForgotPwdError" runat="server" Visible="false" EnableViewState="false" CssClass="logon-password-retrieval-result input__error " />

            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger EventName="Click" ControlID="lnkPasswdRetrieval" />
        </Triggers>
    </cms:CMSUpdatePanel>

    <cms:LocalizedHyperlink runat="server" ID="lnkSignup" CssClass="link mb-2"></cms:LocalizedHyperlink>

    <%-- Password hint part  ResourceString="LogonForm.lnkPasswordRetrieval"--%>
    <cms:CMSUpdatePanel runat="server" ID="CMSUpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <cms:LocalizedLinkButton ID="lnkPasswordhint" runat="server" EnableViewState="false" OnClick="lnkPasswordhint_Click"
                CssClass="logon-password-retrieval-link link mb-2" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>

    <%--PasswordHint Panel  --%>
    <cms:CMSUpdatePanel runat="server" ID="pnlUpdatePasswordHint" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="PnlPasswordHint" runat="server" CssClass="logon-panel-password-retrieval" DefaultButton="btnPasswordHint" Visible="False">

                <%-- Form start --%>
                <div class="form-horizontal mb-2" role="form">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel ID="lblPasswordHint" runat="server" EnableViewState="false" AssociatedControlID="txtPasswordRetrieval"
                                CssClass="control-label" ResourceString="E-MAIL" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtPasswordHint" CssClass="input__text" placeholder="your@email.com" runat="server" />
                        </div>
                    </div>
                    <%-- Form End --%>
                </div>

                <cms:LocalizedButton ID="btnPasswordHint" runat="server" EnableViewState="false" ButtonStyle="Default"
                    CssClass="btn-action login__login-button btn--no-shadow" ResourceString="kadena.logonFrom.Passwordhint" />
                <asp:Label ID="lblPwdHint" runat="server" Visible="false" EnableViewState="false" CssClass="logon-password-retrieval-result" />
                <asp:Label ID="lblHintResult" runat="server" Visible="false" EnableViewState="false" CssClass="logon-password-retrieval-result" />
                <asp:Label ID="lblError" runat="server" Visible="false" EnableViewState="false" CssClass="logon-password-retrieval-result input__error" />

            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger EventName="Click" ControlID="lnkPasswordhint" />
        </Triggers>
    </cms:CMSUpdatePanel>
</asp:Panel>
