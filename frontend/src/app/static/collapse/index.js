/* eslint-disable import/first, import/no-webpack-loader-syntax, import/no-named-default */
export default class Collapse {
  constructor(container) {
    this.container = container;
    const togglers = Array.from(this.container.getElementsByClassName('js-toggle'));
    const expandedCssClass = 'isOpen';

    togglers.forEach((toggler) => {
      toggler.addEventListener('click', () => {
        if (this.container.classList.contains(expandedCssClass)) {
          this.container.classList.remove(expandedCssClass);
        } else {
          this.container.classList.add(expandedCssClass);
        }
      });
    });
  }
}
/* eslint-enable */
