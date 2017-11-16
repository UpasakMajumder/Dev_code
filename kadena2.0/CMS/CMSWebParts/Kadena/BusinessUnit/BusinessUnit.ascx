<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_BusinessUnit_BusinessUnit"  CodeBehind="~/CMSWebParts/Kadena/BusinessUnit/BusinessUnit.ascx.cs" %>

<span id="lblBUNumber" runat="server"></span>
<asp:TextBox ID="txtBUNumber" runat="server" MaxLength="10"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfBUNumber" ControlToValidate="txtBUNumber" runat="server" ></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="revBUNumber" runat="server" ControlToValidate="txtBUNumber" ValidationExpression="^[0-9]{8,10}$" />
<asp:CustomValidator ID="cvBUNumber" runat="server" OnServerValidate="cvBUNumber_ServerValidate" ControlToValidate="txtBUNumber" Enabled="true"></asp:CustomValidator>

<span id="lblBUName" runat="server"></span>
<asp:TextBox ID="txtBUName" runat="server" MaxLength="50" ></asp:TextBox>
<asp:RequiredFieldValidator runat="server" ID="rfBUName" ControlToValidate="txtBUName"></asp:RequiredFieldValidator><br />
<%--<asp:RegularExpressionValidator ID="revBUName" runat="server" ControlToValidate="txtBUName" ErrorMessage="Business unit Name should be in between 1 and 50 characters"
ValidationExpression="^[a-zA-Z]{1,50}$" />--%>

<span id="lblBUStatus" runat="server"></span>
<asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
 
<asp:Button ID="btnSave"  runat="server" OnClick="btnSave_Click" Text="Save"/>
<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />