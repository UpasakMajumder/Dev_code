// <span class="js-tooltip" data-tooltip-placement="top" title="Top">Hover me</span>
import Tippy from '../tippy';

export default class Tooltip {
  constructor(container) {
    const placement = container.dataset.tooltipPlacement;

    new Tippy(container, { // eslint-disable-line
      animation: 'fade',
      arrow: true,
      theme: 'dark',
      position: placement
    });
  }
}
