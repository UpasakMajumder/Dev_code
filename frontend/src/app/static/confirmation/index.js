import Tippy from '../tippy';
import findParentBySelector from '../../helpers/nodes';

class Confirmation {
  constructor(container) {
    const popperClass = 'js-confirmation-popper';
    const popper = container.querySelector(`.${popperClass}`);
    const clickerClass = 'js-confirmation-clicker';
    const clicker = container.querySelector(`.${clickerClass}`);
    const allClickers = document.querySelectorAll(`.${clickerClass}`);

    const { confirmationPosition, confirmationButtonText, confirmationActiveElement, confirmationActiveClass } = container.dataset;

    const activeElement = findParentBySelector(container, confirmationActiveElement);
    const allActiveElements = document.querySelectorAll(confirmationActiveElement);
    const primaryButtonText = clicker.innerHTML;

    new Tippy(container, { // eslint-disable-line
      // bcz
      // hide -> show
      // without = show -> hide
      wait(show) {
        setTimeout(() => {
          clicker.innerHTML = confirmationButtonText;
          activeElement.classList.add(confirmationActiveClass);
          show();
        }, 0);
      },
      // work with all elements, bcz
      // 1st element eventlistener covers rest
      beforeHidden: () => {
        console.log('hide');
        allClickers.forEach((clicker) => {
          clicker.innerHTML = primaryButtonText;
        });
        allActiveElements.forEach((activeElement) => {
          activeElement.classList.remove(confirmationActiveClass);
        });
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
