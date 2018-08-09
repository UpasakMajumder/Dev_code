<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Product_InboundTracking" CodeBehind="~/CMSWebParts/Kadena/Product/InboundTracking.ascx.cs" %>

<div class="custom__block">
    <div class="custom__select">
        <asp:DropDownList ID="ddlCampaign" runat="server" OnSelectedIndexChanged="ddlCampaign_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:DropDownList ID="ddlProgram" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </div>
    <div class="custom__btns">
        <asp:Button ID="btnRefresh" CssClass="btn-action login__login-button btn--no-shadow" runat="server" Enabled="false" OnClick="btnRefresh_Click" />
        <asp:Button ID="btnExport" runat="server" CssClass="btn-action login__login-button btn--no-shadow" Enabled="false" OnClick="btnExport_Click" />
    </div>
    <div class="custom__btns float-right">
        <asp:Button runat="server" Enabled="false" ID="btnClose" CssClass="btn-action login__login-button btn--no-shadow" OnClientClick="javascript:document.getElementById('dialog_Close_IBTF').classList.add('active'); return false;" />
    </div>
</div>
<div class="inbound__track" runat="server" id="inBoundGrid">
    <asp:GridView ID="gdvInboundProducts" runat="server" AutoGenerateColumns="false" OnRowDataBound="gdvInboundProducts_RowDataBound" OnRowEditing="inboundProducts_RowEditing" OnRowUpdating="inboundProducts_RowUpdating"
        OnRowCancelingEdit="gdvInboundProducts_RowCancelingEdit" ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25" OnPageIndexChanging="gdvInboundProducts_PageIndexChanging" PagerSettings-Mode="NumericFirstLast"
        class="table show__table-bottom" PagerStyle-CssClass="pagination__table">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblSkuNumber" Text='<%#Eval("SKUNumber") %>'></asp:Label>
                    <asp:HiddenField runat="server" ID="hfSKUID" Value='<%#Eval("SKUID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblItem" Text='<%#Eval("SKUName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblCustomerReferenceNumber" Text='<%#Eval("ProductCustomerReferenceNumber") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblItemSpecs" Text='<%# Eval("ItemSpec") != null ? (Eval("ItemSpec")?.ToString().Length > 20 ? Eval("ItemSpec").ToString().Substring(0,20) : Eval("ItemSpec"))   : Eval("ItemSpec") %>' class="js-tooltip" data-tooltip-placement="bottom" ToolTip='<%# Eval("ItemSpec") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblQtyOrdered" Text='<%#Eval("QtyOrdered") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblDemandGoal" Text='<%#Eval("DemandGoal") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtDemandGoal" Text='<%#Eval("DemandGoal") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblQtyReceived" Text='<%#Eval("QtyReceived") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtQtyReceived" Text='<%#Eval("QtyReceived") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblQtyProduced" Text='<%#Eval("QtyProduced") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtQtyProduced" Text='<%#Eval("QtyProduced") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblOverage" Text='<%#Eval("Overage") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtOverage" ReadOnly="true" Text='<%#Eval("Overage") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblVendor" Text='<%#Eval("Vendor") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtVendor" Text='<%#Eval("Vendor") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblExpArrivalToCenveo" Text='<%#Eval("ExpArrivalToCenveo") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtExpArrivalToCenveo" Text='<%#Eval("ExpArrivalToCenveo") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblDeliveryToDistBy" Text='<%#Eval("DeliveryToDistBy") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtDeliveryToDistBy" Text='<%#Eval("DeliveryToDistBy") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblShippedToDist" Text='<%#Eval("ShippedToDist") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtShippedToDist" Text='<%#Eval("ShippedToDist") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblCenveoComments" Text='<%#Eval("CenveoComments") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtCenveoComments" Text='<%#Eval("CenveoComments") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblTweComments" Text='<%#Eval("TweComments") != null ? Eval("TweComments")?.ToString().Length > 20 ?Eval("TweComments")?.ToString().Substring(0,20) : Eval("TweComments") : Eval("TweComments") %>' class="js-tooltip" data-tooltip-placement="bottom" ToolTip='<%# Eval("TweComments") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtTweComments" Text='<%#Eval("TweComments") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblActualPrice" Text='<%# $"{CMS.Ecommerce.CurrencyInfoProvider.GetFormattedPrice(EvalDouble("ActualPrice"), CurrentSite.SiteID,true)}"  %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" CssClass="js-ActualPrice" ID="txtActualPrice" Text='<%#Eval("ActualPrice") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblEstimatedPrice" Text='<%# $"{CMS.Ecommerce.CurrencyInfoProvider.GetFormattedPrice(EvalDouble("EstimatedPrice"), CurrentSite.SiteID,true)}"  %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblStatus" Text='<%#ValidationHelper.GetBoolean(Eval("Status"),false)==true?ActiveText:InActiveText %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label Visible="false" runat="server" ID="lbleditStatus" Text='<%#ValidationHelper.GetBoolean(Eval("Status"),false)==true?ActiveText:InActiveText %>' />
                    <asp:DropDownList runat="server" ID="ddlStatus">
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" runat="server" Enabled='<%#ValidationHelper.GetBoolean(Eval("IsClosed"),false)==true?false:true%>' CommandName="edit" Text='<%#EditLinkText %>'></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="update" Text='<%#UpdateText %>'></asp:LinkButton>
                    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="cancel" Text='<%#CancelText %>'></asp:LinkButton>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
<div class=" mt-2" runat="server" id="divNodatafound" visible="false">
        <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen"><cms:LocalizedLabel runat="server" ResourceString="Kadena.Inbound.NoDataText"></cms:LocalizedLabel></div>
    </div>
    <div class=" mt-2" runat="server" id="divSelectCampaign" visible="false">
        <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen"><cms:LocalizedLabel runat="server" ResourceString="Kadena.Inbound.SelectCampaignText"></cms:LocalizedLabel></div>
    </div>
<div class="dialog " id="dialog_Close_IBTF">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__header"></div>
        <div class="dialog__content">
            <cms:LocalizedLabel runat="server" ResourceString="Kadena.Inbound.PopUPLabelMsgText"></cms:LocalizedLabel>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
                <cms:LocalizedLinkButton runat="server" CssClass="btn-action btn-action--secondary" ResourceString="Kadena.Inbound.PopUpYesButtonText" OnClick="popUpYes_ServerClick"></cms:LocalizedLinkButton>
                <cms:LocalizedLinkButton runat="server" CssClass="btn-action btn-action--secondary" ResourceString="Kadena.Inbound.PopUpNoButtonText" OnClientClick="javascript:document.getElementById('dialog_Close_IBTF').classList.remove('active'); return false;"></cms:LocalizedLinkButton>
            </div>
        </div>
    </div>
</div>