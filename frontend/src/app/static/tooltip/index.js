// @flow
// <span class="js-tooltip" data-tooltip-placement="top" title="Top">Hover me</span>

/* helpers */
import { consoleException } from 'app.helpers/io';
/* 3rd-part libraries */
import Tippy from '../tippy';

export default class Tooltip {
  constructor(container: HTMLElement) {
    const placement: string = container.dataset.tooltipPlacement;

    if (!placement) {
      consoleException('No element data-tooltip-placement in', container);
      return;
    }

    new Tippy(container, { // eslint-disable-line
      animation: 'fade',
      arrow: true,
      theme: 'dark',
      position: placement
    });
  }
}
