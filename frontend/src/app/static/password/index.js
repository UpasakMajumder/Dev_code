class Password {
  constructor(container) {
    const classInput = 'js-password-input';
    const classToggler = 'js-password-toggler';

    const input = container.querySelector(`.${classInput}`);
    const toggler = container.querySelector(`.${classToggler}`);

    const { passwordShow, passwordHide } = toggler.dataset;

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
