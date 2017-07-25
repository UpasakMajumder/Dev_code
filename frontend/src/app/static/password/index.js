// @flow

/* helpers */
import { consoleException } from 'app.helpers/io';

class Password {
  constructor(container: HTMLElement) {
    const classInput: string = 'js-password-input';
    const classToggler: string = 'js-password-toggler';

    const input: ?HTMLElement = container.querySelector(`.${classInput}`);
    const toggler: ?HTMLElement = container.querySelector(`.${classToggler}`);

    if (!input) {
      consoleException(`No element with .${classInput} selector`, container);
      return;
    }

    if (!toggler) {
      consoleException(`No element with .${classToggler} selector`, container);
      return;
    }

    const {
      passwordShow,
      passwordHide
    }: {
      passwordShow: string,
      passwordHide: string
    } = toggler.dataset;

    toggler.addEventListener('click', () => {
      if (input.getAttribute('type') === 'text') {
        input.setAttribute('type', 'password');
        toggler.innerHTML = passwordShow;
      } else {
        input.setAttribute('type', 'text');
        toggler.innerHTML = passwordHide;
      }
    });
  }
}

export default Password;
