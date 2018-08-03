// @flow

/* helpers */
import { consoleException } from 'app.helpers/io';

export default class Collapse {
  container: HTMLElement;

  constructor(container: HTMLElement) {
    this.container = container;
    const expandedCssClass: string = 'isOpen';
    const toggler: ?HTMLElement = this.container.querySelector('.js-toggle');

    const targetSelector: string = this.container.dataset.target;
    const target: ?HTMLElement = document.querySelector(targetSelector);

    if (!toggler) {
      consoleException('No toggler with .js-toggle class in', container);
      return;
    }

    let toggle = () => {
      if (this.container.classList.contains(expandedCssClass)) {
        this.container.classList.remove(expandedCssClass);
      } else {
        this.container.classList.add(expandedCssClass);
      }
    };

    if (target) {
      toggle = () => {
        if (target.classList.contains(expandedCssClass)) {
          target.classList.remove(expandedCssClass);
        } else {
          target.classList.add(expandedCssClass);
        }
      };
    }

    toggler.addEventListener('click', toggle);
  }
}
