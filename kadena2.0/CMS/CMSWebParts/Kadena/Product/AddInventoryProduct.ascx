<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddInventoryProduct.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.AddInventoryProduct" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link rel="stylesheet" href="/resources/demos/style.css">
<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<div class="css-login">
    <div class="form">
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">
                    <cms:LocalizedLabel ID="lblPosNumber" runat="server" CssClass="input__label" ResourceString="Kadena.InvProductForm.PosNo" />
                </span>
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlPosNo" runat="server" EnableViewState="false"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvPosNo" runat="server" CssClass="" InitialValue="0" ForeColor="Red" ControlToValidate="ddlPosNo">

                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblBrand" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblBrand" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlBrand" runat="server" EnableViewState="false"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvBrand" runat="server" CssClass="" InitialValue="0" ForeColor="Red" ControlToValidate="ddlBrand">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblShortDes" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblShortDes" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtShortDes" runat="server" EnableViewState="false"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvShortDes" runat="server" CssClass="" ForeColor="Red" ControlToValidate="txtShortDes">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblLongDes" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblLongDes" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtLongDes" runat="server" EnableViewState="false"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvLongDes" runat="server" ForeColor="Red" ControlToValidate="txtLongDes">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblBundleQnt" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblBundleQnt" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtBundleQnt" runat="server" EnableViewState="false"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvBundleQnt" runat="server" ForeColor="Red" ControlToValidate="txtBundleQnt">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblExpDate" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblExpDate" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtExpDate" runat="server" EnableViewState="false" TextMode="DateTime"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvExpDate" runat="server" CssClass="" ForeColor="Red" ControlToValidate="txtExpDate">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblImage" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblImage" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtImage" runat="server" EnableViewState="false" TextMode="DateTime"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvImage" runat="server" ForeColor="Red" ControlToValidate="txtImage">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblProductAllcation" runat="server" EnableViewState="False" CssClass="input__label"
                    ResourceString="Kadena.InvProductForm.lblProductAllcation" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtProdAllocation" runat="server" EnableViewState="false" TextMode="DateTime"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvProdAllocation" runat="server" CssClass="" ForeColor="Red" ControlToValidate="txtProdAllocation">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblCancel" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblCancel" />
                <div class="input__inner">
                    <cms:CMSCheckBoxList ID="chkCancel" runat="server" EnableViewState="false"></cms:CMSCheckBoxList>
                </div>
            </div>
        </div>

        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblEstPrice" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblEstPrice" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtEstPrice" runat="server" EnableViewState="false" TextMode="DateTime"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvEstPrice" runat="server" CssClass="" ForeColor="Red" ControlToValidate="txtEstPrice">

                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblActualPrice" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblActualPrice" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtActualPrice" runat="server" EnableViewState="false"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvActualPrice" runat="server" CssClass="" ForeColor="Red" ControlToValidate="txtActualPrice">

                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblCVOProductId" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblCVOProductId" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtCVOProductId" runat="server" EnableViewState="false"></cms:CMSTextBox>

                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblStoreFrontId" CssClass="input__label" runat="server" EnableViewState="False"
                    ResourceString="Kadena.InvProductForm.lblStoreFrontId" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtStroeFrontId" runat="server" EnableViewState="false"></cms:CMSTextBox>

                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblQuantity" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblQuantity" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtQuantity" runat="server" EnableViewState="false"></cms:CMSTextBox>

                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblState" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblState" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlState" runat="server" EnableViewState="false"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvState" runat="server" CssClass="" InitialValue="0" ForeColor="Red" ControlToValidate="ddlState">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblProdCategory" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblProdCategory" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlProdCategory" runat="server" EnableViewState="false"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvProdCategory" runat="server" CssClass="" InitialValue="0" ForeColor="Red" ControlToValidate="ddlProdCategory">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblStatus" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblStatus" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlStatus" runat="server" EnableViewState="false">
                        <asp:ListItem Text="Enabled" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Disabled" Value="0"></asp:ListItem>
                    </cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" CssClass="" InitialValue="0" ForeColor="Red" ControlToValidate="ddlStatus">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
    </div>
    <div class="mb-3 form_btns">
        <div class="">
            <cms:LocalizedButton ID="btnSave" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary" EnableViewState="false"
                ResourceString="Kadena.Form.SaveButtonText" />
            <cms:LocalizedButton ID="btnCancel" CausesValidation="false" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary"
                EnableViewState="false"
                ResourceString="Kadena.Form.CancelButtonText" />
        </div>
    </div>
    <cms:LocalizedLabel ID="lblSuccessMsg" Visible="false" runat="server" CssClass="input__label" EnableViewState="False" ResourceString="Kadena.CampaignForm.SaveMsg" />
    <cms:LocalizedLabel ID="lblFailureText" runat="server" EnableViewState="False" CssClass="error-label input__error" Visible="false" ResourceString="Kadena.CampaignForm.FailureMsg" />
</div>

<div id="dialog" style="display: none" title="Basic dialog">
    <p>This is the default dialog which is useful for displaying information. The dialog window can be moved, resized and closed with the 'x' icon.</p>
    <div class="cms-bootstrap">
        <%-- <ajaxToolkit:ToolkitScriptManager ID="manScript" runat="server" EnableViewState="false" />--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <cms:UniGrid ID="UniGrid1" runat="server" ObjectType="cms.user" EnableViewState="false">
                                <GridOptions ShowSelection="true" />
                                <GridColumns>
                                    <ug:Column Source="UserName" Caption="User name" Wrap="false" runat="server">
                                    </ug:Column>
                                    <ug:Column Source="UserID" Caption="User ID" Wrap="false" runat="server">
                                    </ug:Column>
                                </GridColumns>
                            </cms:UniGrid>
                            <td>
                               <cms:LocalizedButton ID="OKButton" UseSubmitBehavior="false"  CausesValidation="false" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ButtonStyle="Primary"
                EnableViewState="false"
                ResourceString="Kadena.Form.CancelButtonText" /> 
                            </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblButton" Visible="false"/>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<asp:Button runat="server" CausesValidation="false" ID="OKButton" OnClick="OKButton_Click" CssClass="btn btn-primary" Text="OK" />--%>
        
    </div>
</div>
