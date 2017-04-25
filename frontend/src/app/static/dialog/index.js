// <button type="button" data-dialog="DIALOG SELECTOR" class="js-dialog"></button>

class Dialog {
  constructor(clicker) {
    this.clicker = clicker;
    this.activeClass = 'active';

    const dialogSelector = clicker.dataset.dialog;
    this.dialog = document.querySelector(dialogSelector);
    this.closerNodes = this.dialog.querySelectorAll('.dialog__closer'); // could be many

    this.clicker.addEventListener('click', () => {
      !this.dialog.classList.contains(this.activeClass) && this.dialog.classList.add(this.activeClass);
    });

    Array.from(this.closerNodes).forEach((closer) => {
      closer.addEventListener('click', () => {
        this.dialog.classList.contains(this.activeClass) && this.dialog.classList.remove(this.activeClass);
      });
    });
  }
}

export default Dialog;
