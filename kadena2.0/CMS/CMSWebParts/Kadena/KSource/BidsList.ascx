<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BidsList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.KSource.BidsList" %>

<div class="bids-list__block">
    <h2 runat="server" id="lblOpenProject" class="bids-list__block-header"></h2>
    <table runat="server" id="tblOpenProjects" class="show-table">
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
</div>
<div class="bids-list__block">
    <h2 runat="server" id="lblCompletedProjects" class="bids-list__block-header">You have 3 completed projects</h2>
    <table runat="server" id="tblCompletedProjects" class="show-table">
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
</div>
