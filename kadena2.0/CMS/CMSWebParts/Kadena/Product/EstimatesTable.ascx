<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EstimatesTable.ascx.cs" Inherits="Kadena.CMSWebParts.Kadena.Product.EstimatesTable" %>

<table id="tblEstimates" runat="server" class="table" enableviewstate="false">
  <thead>
    <tr>
      <th colspan="2">
        <svg class="icon icon-deliver">
          <use xlink:href="/gfx/svg/sprites/icons.svg#deliver-car" />
        </svg>
        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.Product.Estimates" />
      </th>
    </tr>
  </thead>
  <tbody>
    <tr id="rowProductionTime" runat="server" enableviewstate="false">
      <td>
        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.Product.ProductionTime" />
      </td>
      <td>
        <asp:Literal ID="ltlProductionTime" runat="server" EnableViewState="false" />    
      </td>
    </tr>
    <tr id="rowShipTime" runat="server" enableviewstate="false">
      <td>
        <cms:LocalizedLabel runat="server" EnableViewState="false" ResourceString="Kadena.Product.ShipTime" />
      </td>
      <td>
          <asp:Literal ID="ltlShipTime" runat="server" EnableViewState="false" />
      </td>
    </tr>
    <tr id="rowShippingCost" runat="server" enableviewstate="false">
      <td>
        <cms:LocalizedLiteral runat="server" EnableViewState="false" ResourceString="Kadena.Product.ShippingCost" />
      </td>
      <td>
          <asp:Literal ID="ltlShippingCost" runat="server" EnableViewState="false" />
      </td>
    </tr>
  </tbody>
</table>