// <button type="button" data-dialog="DIALOG SELECTOR" class="js-dialog"></button>

import { HAS_DIALOG } from 'app.consts';

class Dialog {
  constructor(clicker) {
    this.clicker = clicker;
    this.activeClass = 'active';
    this.html = document.querySelector('html');
    this.bodyEl = document.body;

    const dialogSelector = clicker.dataset.dialog;
    this.dialog = document.querySelector(dialogSelector);
    this.closerNodes = this.dialog.querySelectorAll('.dialog__closer'); // could be many

    this.clicker.addEventListener('click', () => {
      !this.dialog.classList.contains(this.activeClass) && this.dialog.classList.add(this.activeClass);
      this.html.classList.add('css-overflow-hidden');
      this.bodyEl.classList.add(HAS_DIALOG);
    });

    Array.from(this.closerNodes).forEach((closer) => {
      closer.addEventListener('click', () => {
        this.dialog.classList.contains(this.activeClass) && this.dialog.classList.remove(this.activeClass);
        this.html.classList.remove('css-overflow-hidden');
        this.bodyEl.classList.remove(HAS_DIALOG);
      });
    });
  }
}

export default Dialog;
