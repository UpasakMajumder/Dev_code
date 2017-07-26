// @flow

/* helpers */
import { consoleException } from 'app.helpers/io';

export default class AddTr {
  firstRowTemplate: HTMLElement;
  firstRow: ?HTMLElement;
  firstRowClass: string;
  lastRow: HTMLElement;
  tbody: ?Node;
  count: number;


  constructor(container: HTMLElement) {
    this.lastRow = container;
    this.count = 1;
    this.firstRowClass = 'js-first-tr';

    this.tbody = this.lastRow.parentNode;

    if (!(this.tbody instanceof HTMLElement)) {
      consoleException('No parent node in', this.lastRow);
      return;
    }

    this.firstRow = this.tbody.querySelector(`.${this.firstRowClass}`);

    if (!(this.firstRow instanceof HTMLElement)) {
      consoleException(`No element with .${this.firstRowClass} inside`);
      return;
    }

    this.firstRowTemplate = this.firstRow.cloneNode(true);

    const togglers: HTMLElement[] = Array.from(this.lastRow.getElementsByClassName('js-add-tr-toggler'));

    togglers.forEach((toggler) => {
      toggler.addEventListener('click', () => {
        this.count += 1;
        const clonnedRow: HTMLElement = this.firstRowTemplate.cloneNode(true);
        const newNode: ?HTMLElement = this.getNewRow(clonnedRow);

        if (!this.tbody) {
          consoleException('No parent node in', this.lastRow);
          return;
        }

        if (!newNode) return;

        this.tbody.insertBefore(newNode, this.lastRow);
      });
    });
  }

  getNewRow(oldRow: HTMLElement): ?HTMLElement {
    oldRow.classList.remove(this.firstRowClass);
    const elements: HTMLElement[] = Array.from(oldRow.querySelectorAll('[name]'));

    if (!elements.length) {
      consoleException('No elements with selector [name]');
      return null;
    }

    elements.forEach((element) => {
      const name: string = `${element.dataset.name}-${this.count}`;
      element.setAttribute('name', name);
    });
    return oldRow;
  }
}
