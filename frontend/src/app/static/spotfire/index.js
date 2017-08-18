// @flow
import { SPOTFIRE } from '../../globals';

export default class Spotfire {
  customisation: any;

  constructor(container: HTMLElement) { // container is a card block
    const { serverUrl, url, customerId } = SPOTFIRE;
    const parameters = '';
    const reloadAnalysisInstance = false;

    // $FlowIgnore

    this.customisation = new window.spotfire.webPlayer.Customization();

    const app = new window.spotfire.webPlayer.Application(
      serverUrl,
      this.customisation,
      url,
      parameters,
      reloadAnalysisInstance);

    // prefilter
    const filteringSchemeName = 'Filtering scheme';
    const dataTableName = 'CDH_Inventory_Extract_VW_IL';
    const dataColumnName = 'Client_ID';
    const filteringOperation = window.spotfire.webPlayer.filteringOperation.REPLACE;

    const filterColumn = {
      filteringSchemeName,
      dataTableName,
      dataColumnName,
      filteringOperation,
      filterSettings: { values: [customerId] }
    };

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

    app.analysisDocument.filtering.setFilter(filterColumn, filteringOperation);
    // Past here
  }

  initCustomization(showPageNavigation) {
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
