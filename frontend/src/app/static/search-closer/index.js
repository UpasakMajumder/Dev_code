export default class SearchCloser {
  constructor(closeBtn) {
    const activeClass = closeBtn.dataset.activeClass;
    const targetClass = closeBtn.dataset.targetClass;
    const targetElement = document.querySelector(`.${targetClass}`);

    closeBtn.addEventListener('click', () => {
      targetElement.classList.remove(activeClass);
    });
  }
}
