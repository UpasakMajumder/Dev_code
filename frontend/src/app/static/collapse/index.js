// @flow

/* helpers */
import { consoleException } from 'app.helpers/io';

export default class Collapse {
  container: HTMLElement;

  constructor(container: HTMLElement) {
    this.container = container;
    const expandedCssClass: string = 'isOpen';
    const toggler: ?HTMLElement = this.container.querySelector('.js-toggle');

    if (!toggler) {
      consoleException('No toggler with .js-toggle class in', container);
      return;
    }

    toggler.addEventListener('click', () => {
      if (this.container.classList.contains(expandedCssClass)) {
        this.container.classList.remove(expandedCssClass);
      } else {
        this.container.classList.add(expandedCssClass);
      }
    });
  }
}
