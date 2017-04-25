// <span class="js-tooltip" data-tooltip-placement="top" data-tooltip-title="Top">Hover me</span>

import TooltipJS from 'tooltip.js';

export default class Tooltip {
  constructor(container) {
    const title = container.dataset.tooltipTitle;
    const placement = container.dataset.tooltipPlacement;

    new TooltipJS(container, { // eslint-disable-line
      title,
      placement
    });
  }
}
