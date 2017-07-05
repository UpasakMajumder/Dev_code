<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BidsList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.KSource.BidsList" %>

<asp:Panel runat="server" ID="pnlOpenProject" CssClass="bids-list__block">
    <h2 runat="server" id="lblOpenProject" class="bids-list__block-header"></h2>
    <div class="js-table-paginator">
        <table runat="server" id="tblOpenProjects" class="show-table show-table--hide-rows">
            <!-- TODO: Remove ".hidden" after sorting is avalible-->
            <tbody>
                <tr>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.ProjectName" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.RequestId" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.ProjectStatus" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.LastUpdate" />
                    </th>
                </tr>
            </tbody>
        </table>
        <asp:PlaceHolder runat="server" ID="phOpenPagination" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlCompletedProjects" CssClass="bids-list__block">
    <h2 runat="server" id="lblCompletedProjects" class="bids-list__block-header"></h2>
    <div class="js-table-paginator">
        <table runat="server" id="tblCompletedProjects" class="show-table show-table--hide-rows">
            <!-- TODO: Remove ".hidden" after sorting is avalible-->
            <tbody>
                <tr>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.ProjectName" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.RequestId" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.ProjectStatus" />
                    </th>
                    <th>
                        <svg class="icon icon-sort show-table__sort-icon hidden">
                            <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                        </svg>
                        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.KSource.LastUpdate" />
                    </th>
                </tr>
            </tbody>
        </table>
        <asp:PlaceHolder runat="server" ID="phCompletedPagination" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlMicroserviceError" CssClass="bids-list__block" Visible="false">
    <h2 class="bids-list__block-header">
        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.Error.MicroserviceFailed" />
    </h2>
</asp:Panel>
