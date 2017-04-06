<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderServiceTestPage.aspx.cs" Inherits="CMSApp.CMSPages.Kadena.OrderServiceTestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order Service test page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblEndpointUrl" runat="server" />
        <br />
        <textarea id="txtValue" runat="server" cols="80" rows="50" />
        <br />
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
        <br />
        Result message: <asp:Literal ID="ltlResultMessage" runat="server" />
    </div>
    </form>
</body>
</html>
