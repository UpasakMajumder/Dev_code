<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdersList.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Orders.OrdersList" %>

<div class="dashboard__block">
    <div class="dashboard__block-heading">
        <h2>
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.RecentOrders" />
        </h2>
        <a href="#" style="display:none;">
            <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.ViewRecentOrders" />
        </a>
    </div>
    <table class="show-table">
        <tbody>
            <tr>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon" style="display:none;">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>                    
                    <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderDate" />
                </th>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon" style="display:none;">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>                                       
                    <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderedItems" />
                </th>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon" style="display:none;">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>                    
                    <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.OrderStatus" />
                </th>
                <th>
                    <svg class="icon icon-sort show-table__sort-icon" style="display:none;">
                        <use xlink:href="/gfx/svg/sprites/icons.svg#sort-arrows" />
                    </svg>                    
                    <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.OrdersList.DeliveryDate" />
                </th>
                <th></th>
            </tr>
            <tr>
                <td>Mar 3 2017</td>
                <td class="show-table__text-appear">
                    <span class="badge badge--s badge--empty badge--bold">3</span> 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74 / 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74
                </td>
                <td class="show-table__will-hide">In progress</td>
                <td class="show-table__will-hide">May 24 2017</td>
                <td class="show-table__will-appear">
                    <button type="button" class="btn-action">View</button>
                </td>
            </tr>
            <tr>
                <td>Mar 3 2017</td>
                <td class="show-table__text-appear">
                    <span class="badge badge--s badge--empty badge--bold">3</span> 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74 / 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74
                </td>
                <td class="show-table__will-hide">In progress</td>
                <td class="show-table__will-hide">May 24 2017</td>
                <td class="show-table__will-appear">
                    <button type="button" class="btn-action">View</button>
                </td>
            </tr>
            <tr>
                <td>Mar 3 2017</td>
                <td class="show-table__text-appear">
                    <span class="badge badge--s badge--empty badge--bold">3</span> 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74 / 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74
                </td>
                <td class="show-table__will-hide">In progress</td>
                <td class="show-table__will-hide">May 24 2017</td>
                <td class="show-table__will-appear">
                    <button type="button" class="btn-action">View</button>
                </td>
            </tr>
            <tr>
                <td>Mar 3 2017</td>
                <td class="show-table__text-appear">
                    <span class="badge badge--s badge--empty badge--bold">3</span> 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74 / 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74
                </td>
                <td class="show-table__will-hide">In progress</td>
                <td class="show-table__will-hide">May 24 2017</td>
                <td class="show-table__will-appear">
                    <button type="button" class="btn-action">View</button>
                </td>
            </tr>
            <tr>
                <td>Mar 3 2017</td>
                <td class="show-table__text-appear">
                    <span class="badge badge--s badge--empty badge--bold">3</span> 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74 / 1,300 x Business cards / 350 x Monthly magazine / 12 x User manual HF746H74
                </td>
                <td class="show-table__will-hide">In progress</td>
                <td class="show-table__will-hide">May 24 2017</td>
                <td class="show-table__will-appear">
                    <button type="button" class="btn-action">View</button>
                </td>
            </tr>
        </tbody>
    </table>
</div>
