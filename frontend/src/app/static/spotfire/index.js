import { SPOTFIRE } from '../../globals';

export default class Spotfire {
  constructor(wrapper) { // container is a card block
    this.spotfireItemClassName = 'js-spotfire-item';
    this.container = wrapper.querySelector('.js-spotfire-container');
    const button = document.querySelector('.js-spotfire-report');

    const parameters = '';
    const reloadAnalysisInstance = false;

    this.customisation = new spotfire.webPlayer.Customization(); // eslint-disable-line no-undef
    this.initCustomization();

    this.component = this.createComponent();
    const { reportUrl, serverUrl, analysisPaths } = SPOTFIRE;

    button.setAttribute('href', reportUrl);

    analysisPaths.forEach((analysisPath, index) => {
      const component = this.component.cloneNode(true);
      const item = component.querySelector(`.${this.spotfireItemClassName}`);
      const id = `spotfire-${index}`;

      item.setAttribute('id', id);
      item.setAttribute('href', reportUrl);

      this.container.appendChild(component);

      const app = new spotfire.webPlayer.Application(  // eslint-disable-line no-undef
        serverUrl,
        this.customisation,
        analysisPath,
        parameters,
        reloadAnalysisInstance);

      app.openDocument(id, 2, this.customisation);
    });

  }

  createComponent() {
    const wrapper = document.createElement('div');
    wrapper.setAttribute('class', 'col-lg-6');

    const item = document.createElement('a');
    item.setAttribute('class', `${this.spotfireItemClassName} spotfire__item`);

    const spinner = document.createElement('div');
    spinner.setAttribute('class', 'spinner');

    const svg = '<svg class="icon "><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/gfx/svg/sprites/icons.svg#spinner"></use></svg>';

    spinner.innerHTML = svg;
    item.appendChild(spinner);
    wrapper.appendChild(item);

    return wrapper;
  }

  initCustomization() {
    this.customisation.showTopHeader = true;
    this.customisation.showToolBar = true;
    this.customisation.showExportFile = true;
    this.customisation.showExportVisualization = true;
    this.customisation.showCustomizableHeader = true;
    this.customisation.showPageNavigation = true;
    this.customisation.showStatusBar = true;
    this.customisation.showDodPanel = true;
    this.customisation.showFilterPanel = true;
  }
}
