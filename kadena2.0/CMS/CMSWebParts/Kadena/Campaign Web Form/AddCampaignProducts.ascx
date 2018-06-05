<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts" CodeBehind="~/CMSWebParts/Kadena/Campaign Web Form/AddCampaignProducts.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagPrefix="cms" TagName="UniSelector" %>

<asp:HiddenField ID="hdnDatepickerUrl" runat="server" />
<div class=" mt-2" id="Emptydata" runat="server" visible="false">
    <div data-reactroot="" class="alert--info alert--full alert--smaller isOpen">
        <cms:LocalizedLabel ResourceString="Kadena.CampaignProduct.NoProgramfoundText" runat="server">
        </cms:LocalizedLabel>
    </div>
</div>
<div class="login__form-content js-login" runat="server" id="AddProductdiv">
    <div class="css-login">
        <div class="form form__lg">
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProgramName"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlProgram" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqProgram" runat="server" CssClass="EditingFormErrorLabel" ControlToValidate="ddlProgram" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label">
                        <cms:LocalizedLabel ID="lblPosCategory" runat="server" CssClass="input__label" ResourceString="Kadena.InvProductForm.lblPosCategory" />
                    </span>
                    <div class="input__inner">
                        <cms:CMSDropDownList ID="ddlPosCategory" runat="server" AutoPostBack="true" EnableViewState="True" OnSelectedIndexChanged="ddlPosCategory_SelectedIndexChanged"></cms:CMSDropDownList>
                        <asp:RequiredFieldValidator ID="rfvPosCategory"
                            runat="server" CssClass="EditingFormErrorLabel" InitialValue="0" ControlToValidate="ddlPosCategory">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblPosNumber"></span>
                    <div class="input__inner">
                        <asp:DropDownList runat="server" ID="ddlPos" CssClass="input__select"></asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtPOSNumber" Visible="false" Enabled="false" CssClass="input__text form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqPOS" runat="server" CssClass="EditingFormErrorLabel" ControlToValidate="ddlPos" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProductName"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtProductName" CssClass="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqProductName" CssClass="EditingFormErrorLabel" ControlToValidate="txtProductName"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblState"></span>
                    <a href="#" class="state__link" onclick="$('#StateGroupInfoPopup').toggleClass('active');">State Group Information</a>
                    <div class="input__inner">
                        <asp:DropDownList runat="server" ID="ddlState" CssClass="input__select"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblExpirationDate"></span>
                    <div class="input__inner date_picker">
                        <asp:TextBox runat="server" ID="txtExpireDate" EnableViewState="true" CssClass="input__text js-datepicker"></asp:TextBox>
                        <asp:CompareValidator ID="compareDate" runat="server" CssClass="EditingFormErrorLabel" Operator="GreaterThanEqual" ControlToValidate="txtExpireDate" Type="date" />
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblLongDescription"></span>
                    <div class="input__inner long__desc">
                        <asp:TextBox ID="txtLongDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="input__textarea"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqLongDescription" CssClass="EditingFormErrorLabel" runat="server" ControlToValidate="txtLongDescription"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblBrand"></span>
                    <div class="input__inner">
                        <cms:CMSDropDownList ID="ddlBrand" runat="server" class="input__select" Enabled="false"></cms:CMSDropDownList>
                        <asp:HiddenField runat="server" ID="hfBrandItemID" />
                        <asp:RequiredFieldValidator ID="rqBrand" CssClass="EditingFormErrorLabel" runat="server" ControlToValidate="ddlBrand" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblEstimatedPrice"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtEstimatedprice" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqEstimatePrice" CssClass="EditingFormErrorLabel" ControlToValidate="txtEstimatedprice"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEstPrice" runat="server" CssClass="EditingFormErrorLabel"
                            ControlToValidate="txtEstimatedprice" ValidationExpression="((\d+)((\.\d{1,100})?))$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblActualPrice"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtActualPrice" class="input__text" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblProductCategory"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlProductcategory" runat="server" CssClass="input__select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqProductCategory" CssClass="EditingFormErrorLabel" runat="server" ControlToValidate="ddlProductcategory" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblQtyPerPack"></span>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtQty" class="input__text"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqQty" CssClass="EditingFormErrorLabel" runat="server" ControlToValidate="txtQty"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revQty" runat="server" CssClass="EditingFormErrorLabel"
                            ControlToValidate="txtQty" ValidationExpression="^[0-9]*$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>

            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblItemSpecs"><%#ResHelper.GetString("Kadena.CampaignProduct.ItemSpecsText")%></span>
                    <div class="input__inner">
                        <asp:DropDownList runat="server" ID="ddlItemSpecs" CssClass="input__select" Font-Size="11px" OnSelectedIndexChanged="ddlItemSpecs_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblStatus"></span>
                    <div class="input__inner">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input__select"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="mb-2 form__block" id="divItemSpecs" runat="server" visible="false">
                <div class="input__wrapper">
                    <cms:LocalizedLabel ID="lblOthersItemSpec" runat="server" ResourceString="Kadena.CampaignProduct.OtherItemSpecsText" class="input__label"></cms:LocalizedLabel>
                    <div class="input__inner">
                        <asp:TextBox runat="server" ID="txtItemSpec" CssClass="input__text"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="mb-2 form__block">
                <div class="input__wrapper">
                    <span class="input__label" runat="server" id="lblImage"></span>
                    <div class="input__inner">
                        <asp:FileUpload ID="productImage" runat="server" class="input__file" EnableViewState="true" />
                        <div class="product-img">
                            <asp:Image ID="imgProduct" runat="server" Height="100" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="mb-3 form__btns">
            <div class="">
                <asp:Button ID="btnSave" runat="server" class="btn-action login__login-button btn--no-shadow" OnClick="btnSave_Click" />
                <asp:Button ID="btnUpdate" runat="server" class="btn-action login__login-button btn--no-shadow" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancel" runat="server" class="btn-action login__login-button btn--no-shadow" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</div>
<div class="dialog" id="StateGroupInfoPopup">
    <div class="dialog__shadow"></div>
    <div class="dialog__block">
        <div class="dialog__header">
            <span><%# CMS.Helpers.ResHelper.GetString("Kadena.ProductStateInfo.StateGroupPopupHeading") %></span>
            <a onclick="$('#StateGroupInfoPopup').toggleClass('active');" class="btn__close js-btnClose"><i class="fa fa-close"></i></a>
        </div>
        <div class="dialog__content">
            <div class="modal__body business__assigned-user">
                <asp:Repeater ID="RepStateInfo" runat="server">
                    <HeaderTemplate>
                        <table class="show-table">
                            <tbody>
                                <tr>
                                    <th><%# CMS.Helpers.ResHelper.GetString("Kadena.ProductStateInfo.GroupNameText") %></th>
                                    <th><%# CMS.Helpers.ResHelper.GetString("Kadena.ProductStateInfo.StateText") %></th>
                                </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="state__group">
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("GroupName") %>' /></td>
                            <td class="state__group">
                                <asp:Label ID="lblUserid" runat="server" CssClass="trstyle" Text='<%# Eval("States") %>' />

                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
               
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Label runat="server" ID="Label3" Visible="false" />
            </div>
        </div>
        <div class="dialog__footer">
            <div class="btn-group btn-group--right">
            </div>
        </div>
    </div>
</div>
