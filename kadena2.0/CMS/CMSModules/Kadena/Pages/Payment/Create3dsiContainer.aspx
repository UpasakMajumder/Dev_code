<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create3dsiContainer.aspx.cs" Inherits="Kadena.CMSModules.Kadena.Pages.Payment.Create3dsiContainer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label for="tbCode">Code</label>
        <asp:TextBox runat="server" ID="tbCode"/>
    </div>
        <div>
            <asp:Button runat="server" Text="Submit" ID="btnSubmit" OnClick="btnSubmit_Click"/>
        </div>
    </form>
</body>
</html>
