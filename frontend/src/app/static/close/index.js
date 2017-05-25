export default class Close {
  constructor(closeContainer) {
    const closeTogglerClass = 'js-close-trigger';
    const hideClass = 'hide';
    const { animationLength } = closeContainer.dataset;
    const togglers = Array.from(closeContainer.querySelectorAll(`.${closeTogglerClass}`));

    togglers.forEach((toggler) => {
      toggler.addEventListener('click', () => {

        closeContainer.classList.add(hideClass);
        setTimeout(() => {
          closeContainer.style.display = 'none';
        }, animationLength);

      });
    });
  }
}
