<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewMailingList.aspx.cs" Inherits="CMSApp.CMSPages.Kadena.NewMailingList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblFilePath" runat="server">Select file</asp:Label><br />
            <asp:FileUpload ID="flFile" runat="server" /><br />
            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" /><br />
            <asp:Panel ID="pnlColumns" runat="server" Visible="false">
                <asp:Label ID="Label1" runat="server">Title</asp:Label><asp:DropDownList ID="ddlTitle" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Label ID="Label2" runat="server">Name</asp:Label><asp:DropDownList ID="ddlName" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Label ID="Label3" runat="server">Second name</asp:Label><asp:DropDownList ID="ddlSecondName" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Label ID="Label4" runat="server">First address line</asp:Label><asp:DropDownList ID="ddlAddress1" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Label ID="Label5" runat="server">Second address line</asp:Label><asp:DropDownList ID="ddlAddress2" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Label ID="Label6" runat="server">City</asp:Label><asp:DropDownList ID="ddlCity" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Label ID="Label7" runat="server">State</asp:Label><asp:DropDownList ID="ddlState" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Label ID="Label8" runat="server">ZIP Code</asp:Label><asp:DropDownList ID="ddlZipCode" runat="server" DataTextField="Key" DataValueField="Value" /><br />
                <asp:Button ID="btnProcess" runat="server" Text="Process" OnClick="btnProcess_Click" /><br />
            </asp:Panel>
            <asp:Label ID="lblUploadStatus" runat="server" ForeColor="Blue" />
        </div>
    </form>
</body>
</html>
