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
    this.initCustomization();

    const app = new window.spotfire.webPlayer.Application(
      serverUrl,
      this.customisation,
      url,
      parameters,
      reloadAnalysisInstance);

    const docEls = Array.from(container.querySelectorAll('.js-spotfire-tab'));

    const docs = []; // keep docs

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

    window.filterColumn = filterColumn;
    window.app = app;
    window.filteringOperation = filteringOperation;


    docEls.forEach((docEl) => {
      const { id, dataset } = docEl;
      const { tab } = dataset;

      const doc = app.openDocument(id, tab, this.customisation);

      docs.push(doc);
    });

    app.analysisDocument.filtering.setFilter(filterColumn, filteringOperation);

    // Past here
  }

  initCustomization() {
    this.customisation.showClose = false;
    this.customisation.showUndoRedo = true;
    this.customisation.showToolBar = false;
    this.customisation.showDodPanel = false;
    this.customisation.showStatusBar = false;
    this.customisation.showExportFile = false;
    this.customisation.showFilterPanel = false;
    this.customisation.showAnalysisInfo = true;
    this.customisation.showPageNavigation = false;
    this.customisation.showExportVisualization = false;
  }
}

// const filterBtns = document.querySelectorAll('.js-filter-spotfire');
// Array.from(filterBtns).forEach((btn) => {
//   btn.addEventListener('click', (event) => {
//     const { target } = event;
//     const { filterTime } = target.dataset;
//
//     if (filterTime === 'all') {
//       doc.filtering.resetAllFilters();
//     } else {
//       doc.data.getActiveDataTable((dataTable) => {
//         const filterColumn = {
//           filteringSchemeName: "Filtering scheme",
//           dataTableName: dataTable.dataTableName,
//           dataColumnName: filterColumnNameInput.value, ///// COLUMN
//           filteringOperation: spotfire.webPlayer.filteringOperation.REPLACE,
//           filterSettings: {
//             includeEmpty: true,
//             values: filterValuesInput.value.split(',').map(item => item.trim()) // filterTime
//           }
//         };
//
//         const filteringOperation = spotfire.webPlayer.filteringOperation.REPLACE;
//
//         doc.filtering.setFilter(
//           filterColumn,
//           filteringOperation);
//       });
//     }
//
//   });
// });
