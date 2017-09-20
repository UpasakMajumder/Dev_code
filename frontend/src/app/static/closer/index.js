// @flow
export default class Close {
  constructor(closeContainer: HTMLElement) {
    const closeTogglerClass: string = 'js-close-this-trigger';
    const animateClass: string = 'isAnimated';
    const hideClass: string = 'isHidden';
    const togglers: HTMLElement[] = Array.from(closeContainer.querySelectorAll(`.${closeTogglerClass}`));
    const { animationLength }: { animationLength: string } = closeContainer.dataset;

    const animationLengthNumber: number = +animationLength;

    togglers.forEach((toggler: EventTarget) => {
      toggler.addEventListener('click', () => {
        closeContainer.classList.add(animateClass);
        setTimeout(() => {
          closeContainer.classList.add(hideClass);
        }, animationLengthNumber);

      });
    });
  }
}
