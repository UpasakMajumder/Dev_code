<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdersList.aspx.cs"
    Inherits="Kadena.CMSModules.Kadena.Pages.Orders.OrdersList"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Order list" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<%@ Register
    Src="~/CMSFormControls/Sites/SiteSelector.ascx"
    TagName="SiteSelector"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">

    <%= RenderTableStyle() %>

    <asp:HiddenField runat="server" ID="orderByOrderDateDesc" ClientIDMode="Static" Value="1" />

    <asp:PlaceHolder runat="server" ID="messageContainer" Visible="false">
        <div class="alert-dismissable alert-error alert">
            <span class="alert-icon">
                <i class="icon-i-circle"></i>
            </span>
            <div class="alert-label">
                <asp:Literal runat="server" ID="message"></asp:Literal>
            </div>
        </div>
    </asp:PlaceHolder>

    <div class="form-group">
        <label class="control-label" for="siteSelector" style="text-align: left">Site:</label>
        <cms:SiteSelector ClientIDMode="Static" ID="siteSelector" runat="server" AllowAll="false" />
    </div>

    <div class="form-group">
        <label class="control-label" for="dateFrom" style="text-align: left">Date from:</label>
        <cms:DateTimePicker ID="dateFrom" runat="server" EditTime="false" />
    </div>

    <div class="form-group">
        <label class="control-label" for="dateTo" style="text-align: left">Date to:</label>
        <cms:DateTimePicker ID="dateTo" runat="server" EditTime="false" />
    </div>

    <div class="form-group">
        <asp:Button Text="Search" CssClass="btn btn-primary" runat="server"
            ClientIDMode="Static" ID="btnApplyFilter" OnClick="btnApplyFilter_Click" />
        <asp:Button Text="Export as xlsx" CssClass="btn btn-primary" runat="server"
            ClientIDMode="Static" ID="btnExport" OnClick="btnExport_Click" />
    </div>

    <div class="form-group">
        <cms:BasicDataGrid runat="server" ClientIDMode="Static" ID="ordersDatagrid" AutoGenerateColumns="false" 
            CssClass="table table-hover" BorderStyle="None" 
            HeaderStyle-BackColor="#e5e5e5" HeaderStyle-Font-Bold="true" HeaderStyle-BorderStyle="None" 
            >
            <Columns>
                <asp:BoundColumn HeaderText="Site" DataField="Site"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Number" DataField="Number"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Ordering Date" DataField="OrderingDate" HeaderStyle-CssClass="OrderingDate order-by-enabled" ></asp:BoundColumn>
                <asp:BoundColumn HeaderText="User" DataField="User"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Name" DataField="Name"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="SKU" DataField="SKU"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Quantity" DataField="Quantity"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Price" DataField="Price"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Status" DataField="Status"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Shipping Date" DataField="ShippingDate"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Tracking Number" DataField="TrackingNumber"></asp:BoundColumn>
            </Columns>
        </cms:BasicDataGrid>
    </div>

    <div class="form-group">
        <div class="pagination">

            <ul class="pagination-list">
                <li>
                    <asp:LinkButton runat="server" 
                        ToolTip="Previous page" ID="previousPage" OnClick="previousPage_Click">
                        <i class="icon-chevron-left" aria-hidden="true"></i>
                        <span class="sr-only">Previous page</span>
                    </asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton runat="server" 
                        ToolTip="Next page" ID="nextPage" OnClick="nextPage_Click">
                        <i class="icon-chevron-right" aria-hidden="true"></i>
                        <span class="sr-only">Next page</span>
                    </asp:LinkButton>
                </li>
            </ul>

            <div class="pagination-pages">
                <label id="m_c_grdOrders_p_p_ctl00_ctl00_lblPage" for="jumpToPage">Page</label>
                <asp:DropDownList runat="server" ClientIDMode="Static" ID="jumpToPage" CssClass="form-control" 
                    AutoPostBack="true" OnSelectedIndexChanged="jumpToPage_SelectedIndexChanged">
                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                </asp:DropDownList>
                <span class="pages-max">/ <asp:Literal runat="server" ID="totalPages" Text="1" /></span>
            </div>

        </div>
    </div>

    <script>
        // hide loader
        window.document.getElementById('btnExport').addEventListener('click', function () {            
            window.setTimeout(function () { window.Loader.hide(); }, 2000);
        }, false);

        // order by create date toggle
        let orderingDateHeader = window.document.getElementsByClassName('OrderingDate')[0];
        if (orderingDateHeader) {
            // add direction glyph
            orderingDateHeader.textContent = `${orderingDateHeader.textContent} ${isOrderByOrderDateDesc() ? '▼' : '▲'}`;

            orderingDateHeader.addEventListener('click', function () {
                // get current direction
                let isDesc = isOrderByOrderDateDesc();
                // toggle
                let orderByElement = window.document.getElementById('orderByOrderDateDesc');
                orderByElement.value = isDesc ? '0' : '1';
                // reload
                window.document.getElementById('btnApplyFilter').click();
            }, false);
        }
        function isOrderByOrderDateDesc() {
            return window.document.getElementById('orderByOrderDateDesc').value == '1';
        }
    </script>

</asp:Content>
