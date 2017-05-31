export default class Close {
  constructor(closeContainer) {
    const closeTogglerClass = 'js-close-this-trigger';
    const animateClass = 'isAnimated';
    const hideClass = 'isHidden';
    const { animationLength } = closeContainer.dataset;
    const togglers = Array.from(closeContainer.querySelectorAll(`.${closeTogglerClass}`));

    togglers.forEach((toggler) => {
      toggler.addEventListener('click', () => {

        closeContainer.classList.add(animateClass);
        setTimeout(() => {
          closeContainer.classList.add(hideClass);
        }, animationLength);

      });
    });
  }
}
