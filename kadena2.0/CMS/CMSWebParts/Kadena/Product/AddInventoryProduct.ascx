<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddInventoryProduct.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.AddInventoryProduct" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link rel="stylesheet" href="/resources/demos/style.css">

<div class="css-login">
    <div class="form form form_width100 formerrormsgs">
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <span class="input__label">
                    <cms:LocalizedLabel ID="lblPosNumber" runat="server" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblPosNo" />
                </span>
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlPosNo" runat="server" AutoPostBack="true" EnableViewState="True" OnSelectedIndexChanged="ddlPosNo_SelectedIndexChanged" CssClass="input__select"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvPosNo" runat="server" CssClass="input__error" InitialValue="0" ForeColor="Red" ControlToValidate="ddlPosNo">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblBrand" CssClass="input__label" runat="server" EnableViewState="false" ResourceString="Kadena.InvProductForm.lblBrand" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlBrand" runat="server" EnableViewState="True"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvBrand" runat="server" CssClass="input__error" InitialValue="0" ForeColor="Red" ControlToValidate="ddlBrand">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblShortDes" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblShortDes" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtShortDes" runat="server" EnableViewState="false" CssClass="input__text"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvShortDes" runat="server" CssClass="input__error" ForeColor="Red" ControlToValidate="txtShortDes">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblLongDes" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblLongDes" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtLongDes" runat="server" EnableViewState="false" CssClass="input__text" Rows="5" Columns="5" TextMode="MultiLine"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvLongDes" CssClass="input__error" runat="server" ForeColor="Red" ControlToValidate="txtLongDes">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblBundleQnt" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblBundleQnt" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtBundleQnt" runat="server" EnableViewState="false" CssClass="input__text"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvBundleQnt" CssClass="input__error" runat="server" ForeColor="Red" ControlToValidate="txtBundleQnt">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revBundleQnt" CssClass="input__error" runat="server" 
                        ControlToValidate="txtBundleQnt" ValidationExpression="^[0-9]*$" ForeColor="Red">
                          </asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblExpDate" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblExpDate" />
                <div class="input__inner date_picker">
                    <cms:CMSTextBox ID="txtExpDate" runat="server" EnableViewState="false" CssClass="input__text" TextMode="DateTime"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvExpDate" runat="server" CssClass="input__error" ForeColor="Red" ControlToValidate="txtExpDate">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblEstPrice" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblEstPrice" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtEstPrice" runat="server" EnableViewState="false" TextMode="DateTime" CssClass="input__text"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvEstPrice" runat="server" CssClass="input__error" ForeColor="Red" ControlToValidate="txtEstPrice"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblActualPrice" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblActualPrice" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtActualPrice" runat="server" EnableViewState="false" CssClass="input__text"></cms:CMSTextBox>
                    <asp:RequiredFieldValidator ID="rfvActualPrice" runat="server" CssClass="input__error" ForeColor="Red" ControlToValidate="txtActualPrice"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblQuantity" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblQuantity" />
                <div class="input__inner">
                    <cms:CMSTextBox ID="txtQuantity" runat="server" EnableViewState="false" CssClass="input__text"></cms:CMSTextBox>
                    <asp:RegularExpressionValidator ID="revQuantity" runat="server" 
                        ControlToValidate="txtQuantity" ValidationExpression="^[0-9]*$" ForeColor="Red" CssClass="input__error"></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblState" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblState" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlState" runat="server" EnableViewState="True"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvState" runat="server" CssClass="input__error" InitialValue="0" ForeColor="Red" ControlToValidate="ddlState"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblProdCategory" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblProdCategory" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlProdCategory" runat="server" EnableViewState="True"></cms:CMSDropDownList>
                    <asp:RequiredFieldValidator ID="rfvProdCategory" runat="server" CssClass="input__error" InitialValue="0" ForeColor="Red" ControlToValidate="ddlProdCategory"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblStatus" CssClass="input__label" runat="server" EnableViewState="False" ResourceString="Kadena.InvProductForm.lblStatus" />
                <div class="input__inner">
                    <cms:CMSDropDownList ID="ddlStatus" runat="server" EnableViewState="True" CssClass="input__select"></cms:CMSDropDownList>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper">
                <cms:LocalizedLabel ID="lblImage" runat="server" EnableViewState="False" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblImage" />
                <div class="input__inner">
                    <asp:FileUpload ID="productImage" runat="server" CssClass="input__file" />
                    <div class="product-img">
                        <asp:Image ID="imgProduct" runat="server" Height="100" Width="100" Visible="false" />
                    </div>
                </div>
            </div>
        </div>
        <div class="mb-2 form_block">
            <div class="input__wrapper allocated_block ">
                <cms:LocalizedLabel ID="lblProductAllcation" runat="server" EnableViewState="False" CssClass="input__label"
                    ResourceString="Kadena.InvProductForm.lblProductAllcation" />
                <a href="#" onclick="$('.modal_popup').show();"><i class="fa fa-plus" aria-hidden="true"></i>User</a>
            </div>
            <div class="Business_Assigned_user">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Repeater ID="RepSelectedUser" runat="server">
                            <HeaderTemplate>
                                <table class="show-table">
                                    <tbody>
                                        <tr>
                                            <th><%# CMS.Helpers.ResHelper.GetString("Kadena.InvProductRepeater.NameText") %></th>
                                            <th><%# CMS.Helpers.ResHelper.GetString("Kadena.InvProductRepeater.EmailText") %></th>
                                            <th><%# CMS.Helpers.ResHelper.GetString("Kadena.InvProductRepeater.QuantityText") %></th>
                                        </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserID") %>' Style="display: none" />
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("UserName") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("EmailID") %>' /></td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Quantity") %>' /></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                                 </table>
                               
                            </FooterTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
            <asp:HiddenField ID="hdnDatepickerUrl" runat="server" />
        </div>
    </div>
    <cms:LocalizedLabel ID="lblSuccessMsg" Visible="false" runat="server" CssClass="input__label" EnableViewState="False" ResourceString="Kadena.InvProductForm.SaveMsg" ForeColor="Green" />
    <cms:LocalizedLabel ID="lblFailureText" runat="server" EnableViewState="False" CssClass="error-label input__error" Visible="false" ResourceString="Kadena.InvProductForm.FailureMsg" ForeColor="Red" />
</div>

<div class="modal_popup dialog" style="display: none">
    <div class="modal-content">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="modal_header clearfix">
                    <cms:LocalizedButton ID="btnAllocateProduct" CausesValidation="false" UseSubmitBehavior="false" CssClass="btn-action login__login-button btn--no-shadow" runat="server" ResourceString="Kadena.InvProductForm.AddUser" OnClientClick="$('.modal_popup').hide();" />
                    <a href="#" class="btn_close js-btnClose"><i class="fa fa-close"></i></a>
                </div>
                <div class="modal_body Business_Assigned_user">
                    <asp:Repeater ID="RepterDetails" runat="server">
                        <HeaderTemplate>
                            <table class="show-table">
                                <tbody>
                                    <tr>
                                        <th></th>
                                        <th><%# CMS.Helpers.ResHelper.GetString("Kadena.InvProductRepeater.NameText") %></th>
                                        <th><%# CMS.Helpers.ResHelper.GetString("Kadena.InvProductRepeater.EmailText") %></th>
                                        <th><%# CMS.Helpers.ResHelper.GetString("Kadena.InvProductRepeater.QuantityText") %></th>
                                    </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkAllocate" runat="server" Checked='<%# Eval("Selected") %>' /></td>
                                <td>
                                    <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                    <asp:Label ID="lblUserid" runat="server" Style="display: none" Text='<%# Eval("UserID") %>' /></td>
                                <td>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("EmailID") %>' /></td>
                                <td>
                                    <asp:TextBox ID="txtAllQuantity" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revAllQuantity" runat="server" 
                                        ErrorMessage='<%# CMS.Helpers.ResHelper.GetString("Kadena.InvProductForm.NumberOnly") %>' 
                                        ControlToValidate="txtAllQuantity" ValidationExpression="^[0-9]*$" ForeColor="Red">
                          </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                                 </table>
                       
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rptPager" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>' CausesValidation="false"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label runat="server" ID="lblButton" Visible="false" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
