import { SPOTFIRE } from '../../globals';

export default class Spotfire {
  constructor(container) { // container is a card block
    const { id, dataset } = container;
    const { url } = dataset;
    const { serverUrl } = SPOTFIRE;

    const parameters = '';
    const reloadAnalysisInstance = false;

    this.customisation = new spotfire.webPlayer.Customization(); // eslint-disable-line no-undef
    this.initCustomization();

    const app = new spotfire.webPlayer.Application(  // eslint-disable-line no-undef
      serverUrl,
      this.customisation,
      url,
      parameters,
      reloadAnalysisInstance);

    const doc = app.openDocument(id, 0, this.customisation);

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
