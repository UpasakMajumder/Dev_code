export default class Close {
  constructor(closeContainer) {
    this.closeContainer = closeContainer;
    const closeTogglerClass = '.js-close-trigger';
    const togglers = Array.from(this.closeContainer.querySelectorAll(closeTogglerClass));

    togglers.forEach((toggler) => {
      toggler.addEventListener('click', () => {
        this.closeContainer.style.display = 'none';
      });
    });
  }
}
