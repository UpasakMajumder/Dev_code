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

    app.openDocument(id, 0, this.customisation);
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
