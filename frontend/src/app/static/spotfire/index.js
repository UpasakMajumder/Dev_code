export default class Spotfire {
  constructor(wrapper) { // container is a card block

    /* Init Tibco */
    // const serverUrl = 'http://spotfire.kadenadev.com/spotfire/wp/';
    // const analysisPath = '/Kadena/DIRECTMAILCDH-ReceivedMailPerformance';
    const serverUrl = 'https://spotfire.cloud.tibco.com/spotfire/wp/';
    const analysisPath = '/Gallery/Mashup';

    const parameters = '';
    const reloadAnalysisInstance = false;

    this.customization = new spotfire.webPlayer.Customization(); // eslint-disable-line no-undef
    this.initCustomization();

    /* Init DOM */

    const container = wrapper.querySelector('.js-spotfire-container');
    const btns = wrapper.querySelectorAll('.js-spotfire-btn');

    console.log(container.id);

    window.addEventListener('load', () => {
      const app = new spotfire.webPlayer.Application(  // eslint-disable-line no-undef
        serverUrl,
        this.customization,
        analysisPath,
        parameters,
        reloadAnalysisInstance);

      app.openDocument(container.id, 2, this.customization);
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
