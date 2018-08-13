// @flow
import { SPOTFIRE } from '../../globals';

export default class Spotfire {
  customisation: any;

  constructor(container: HTMLElement) { // container is a card block
    const { serverUrl, url, customerId } = SPOTFIRE;
    const parameters = '';
    const reloadAnalysisInstance = false;

    this.customisation = new window.spotfire.webPlayer.Customization();

    const app = new window.spotfire.webPlayer.Application(
      serverUrl,
      this.customisation,
      url,
      parameters,
      reloadAnalysisInstance);

    if (container.dataset.report) {
      const { id } = container;
      this.initCustomization(true);

      app.openDocument(id, 0, this.customisation);
    } else {
      const tabs = Array.from(container.querySelectorAll('.js-spotfire-tab'));
      this.initCustomization(false);

      tabs.forEach((tab) => {
        const { id, dataset } = tab;
        const { doc } = dataset;

        app.openDocument(id, doc, this.customisation);
      });
    }

    const filterData = [
      {
        dataTableName: 'CDH_Inventory_Extract_VW_IL',
        dataColumnName: 'Client_ID'
      },
      {
        dataTableName: 'CDH_Sales_Order_Extract_VW_IL',
        dataColumnName: 'ClientID'
      },
      {
        dataTableName: 'CDH_Material_Usage_VW_IL',
        dataColumnName: 'Client_ID'
      },
      {
        dataTableName: 'Material_Receipt_Adjustment_Destruction_IL',
        dataColumnName: 'Client ID'
      }
    ];

    // prefilters
    filterData.forEach((data) => {
      const filteringSchemeName = 'Filtering scheme';
      const { dataTableName, dataColumnName } = data;
      const filteringOperation = window.spotfire.webPlayer.filteringOperation.REPLACE;

      const filterColumn = {
        filteringSchemeName,
        dataTableName,
        dataColumnName,
        filteringOperation,
        filterSettings: { values: [customerId] }
      };

      app.analysisDocument.filtering.setFilter(filterColumn, filteringOperation);
    });
  }

  initCustomization(showPageNavigation: boolean) {
    this.customisation.showClose = false;
    this.customisation.showUndoRedo = true;
    this.customisation.showToolBar = false;
    this.customisation.showDodPanel = false;
    this.customisation.showStatusBar = false;
    this.customisation.showExportFile = false;
    this.customisation.showFilterPanel = false;
    this.customisation.showAnalysisInfo = true;
    this.customisation.showPageNavigation = showPageNavigation;
    this.customisation.showExportVisualization = false;
  }
}
