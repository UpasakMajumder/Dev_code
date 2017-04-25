export default class Spotfire {
  constructor(container) { // container is a card block
    const serverUrl = 'https://spotfire.cloud.tibco.com/spotfire/wp/';
    const analysisPath = '/Gallery/Mashup';
    const parameters = '';
    const reloadAnalysisInstance = false;
    const dataset = container.dataset;
    const containerId = container.id;

    /* spotfire API from global */
    this.customization = new spotfire.webPlayer.Customization(); // eslint-disable-line no-undef
    this.initCustomization();

    window.addEventListener('load', () => {
      const app = new spotfire.webPlayer.Application(  // eslint-disable-line no-undef
        serverUrl,
        this.customization,
        analysisPath,
        parameters,
        reloadAnalysisInstance);

      const { spotfireDoc, spotfireFull } = dataset;

      spotfireFull && this.initFullScreenCustomization();

      app.openDocument(containerId, spotfireDoc, this.customization);
    });
  }

  initCustomization() {
    this.customization.showClose = false;
    this.customization.showUndoRedo = true;
    this.customization.showToolBar = false;
    this.customization.showDodPanel = false;
    this.customization.showStatusBar = false;
    this.customization.showExportFile = false;
    this.customization.showFilterPanel = false;
    this.customization.showAnalysisInfo = true;
    this.customization.showPageNavigation = false;
    this.customization.showExportVisualization = false;
  }

  initFullScreenCustomization() {
    this.customization.showClose = true;
    this.customization.showToolBar = true;
    this.customization.showDodPanel = true;
    this.customization.showStatusBar = true;
    this.customization.showExportFile = true;
    this.customization.showFilterPanel = true;
    this.customization.showExportVisualization = true;
  }
}
