import Tippy from '../tippy';
import findParentBySelector from '../../helpers/nodes';

class Confirmation {
  constructor(container) {
    const popperClass = 'js-confirmation-popper';
    const popper = container.querySelector(`.${popperClass}`);
    const clickerClass = 'js-confirmation-clicker';
    const clicker = container.querySelector(`.${clickerClass}`);

    const { confirmationPosition, confirmationButtonText, confirmationActiveElement, confirmationActiveClass } = container.dataset;

    const activeElement = findParentBySelector(container, confirmationActiveElement);
    const primaryButtonText = clicker.innerHTML;

    new Tippy(container, { // eslint-disable-line
      beforeShown: () => {
        clicker.innerHTML = confirmationButtonText;
        activeElement.classList.add(confirmationActiveClass);
      },
      beforeHidden: () => {
        clicker.innerHTML = primaryButtonText;
        activeElement.classList.remove(confirmationActiveClass);
      },
      animation: 'fade',
      arrow: true,
      trigger: 'click',
      theme: 'light',
      html: popper,
      position: confirmationPosition
    });
  }
}

export default Confirmation;
