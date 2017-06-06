<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BidsList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.KSource.BidsList" %>

<div class="bids-list__block">
    <h2 runat="server" id="lblOpenProject" class="bids-list__block-header">You have 2 open projects</h2>
    <table runat="server" id="tblOpenProjects" class="show-table">
        <!-- TODO: Remove ".hidden" after sorting is avalible-->
        <tbody>
            <tr>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon hidden">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>
                    Order date</th>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon hidden">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>
                    Project name</th>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon hidden">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>
                    Request ID</th>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon hidden">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>
                    Project status</th>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon hidden">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>
                    Last update</th>
            </tr>
        </tbody>
    </table>
</div>
