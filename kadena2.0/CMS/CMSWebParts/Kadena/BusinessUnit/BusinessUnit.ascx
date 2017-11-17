<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_BusinessUnit_BusinessUnit" CodeBehind="~/CMSWebParts/Kadena/BusinessUnit/BusinessUnit.ascx.cs" %>

<div class="content-block">
    <div class="login__form-content js-login">
        <div class="css-login changepwd_block">
            <div class="form">
                <div class="mb-2">
                    <div class="input__wrapper">
                        <%--<span class="input__label">Business Unit Number</span>
                        <input type="text" class="input__text" placeholder="Enter Business Unit Number">--%>
                        <span id="lblBUNumber" runat="server" class="input__label"></span>
                        <asp:TextBox ID="txtBUNumber" runat="server" MaxLength="10" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfBUNumber" ControlToValidate="txtBUNumber" runat="server" CssClass="input__text"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revBUNumber" runat="server" ControlToValidate="txtBUNumber" ValidationExpression="^[0-9]{8,10}$" />
                        <asp:CustomValidator ID="cvBUNumber" runat="server" OnServerValidate="cvBUNumber_ServerValidate" ControlToValidate="txtBUNumber" Enabled="true"></asp:CustomValidator>
                    </div>
                </div>


                <div class="mb-2">
                    <div class="input__wrapper">
                        <%-- <span class="input__label">Business Unit Name</span>
                        <input type="text" class="input__text" placeholder="Enter Business Unit Name">--%>
                        <span id="lblBUName" runat="server" class="input__label"></span>
                        <asp:TextBox ID="txtBUName" runat="server" MaxLength="50" CssClass="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfBUName" ControlToValidate="txtBUName"></asp:RequiredFieldValidator><br />
                        <%--<asp:RegularExpressionValidator ID="revBUName" runat="server" ControlToValidate="txtBUName" ErrorMessage="Business unit Name should be in between 1 and 50 characters"
ValidationExpression="^[a-zA-Z]{1,50}$" />--%>
                    </div>
                </div>
                <div class="mb-2">
                    <div class="input__wrapper">
                        <%--<span class="input__label">Status</span>
                        <select>
                            <option>Select Status</option>
                            <option>Active</option>
                            <option>Inactive</option>
                        </select>--%>
                        <span id="lblBUStatus" runat="server" class="input__label"></span>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input__select"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="mb-3 form_btns">
                <div class="">
                    <%--<button type="button" class="btn-action login__login-button btn--no-shadow">save</button>--%>

                    <%--<button type="button" class="btn-action login__login-button btn--no-shadow">Cancel</button>--%>
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn-action login__login-button btn--no-shadow" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn-action login__login-button btn--no-shadow" />
                </div>
            </div>
        </div>

    </div>
</div>
