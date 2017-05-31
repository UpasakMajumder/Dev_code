class Dropzone {
  constructor(container) {
    this.container = container;
    this.file = container.querySelector('.js-drop-zone-file');
    this.btns = Array.from(container.querySelectorAll('.js-drop-zone-btn'));
    this.nameNode = container.querySelector('.js-drop-zone-name');
    this.nameInput = document.querySelector('.js-drop-zone-name-input');
    this.selector = 'isDropped';
    this.reverseSelector = 'isNotDropped';

    this.file.addEventListener('change', (event) => {

      if (!this.file.value) {
        this.container.classList.contains(this.reverseSelector) && this.container.classList.remove(this.reverseSelector);
        this.container.classList.contains(this.selector) && this.container.classList.remove(this.selector);
        this.nameInput.value = '';
        return;
      }

      const { name } = event.target.files[0];
      const arrayName = name.split('.');
      const type = arrayName[arrayName.length - 1];

      if (type !== 'csv') {
        this.container.classList.contains(this.selector) && this.container.classList.remove(this.selector);
        !this.container.classList.contains(this.reverseSelector) && this.container.classList.add(this.reverseSelector);
        this.nameInput.value = name;
        return;
      }

      this.container.classList.contains(this.reverseSelector) && this.container.classList.remove(this.reverseSelector);
      !this.container.classList.contains(this.selector) && this.container.classList.add(this.selector);
      this.nameNode.innerHTML = name;
      this.nameInput.value = name;
    });

    this.btns.forEach((btn) => {

      btn.addEventListener('click', (event) => {
        this.container.classList.remove(this.selector);
        this.container.classList.remove(this.reverseSelector);
        this.file.value = '';
        this.nameInput.value = '';
        event.preventDefault();
      });
    });
  }
}

export default Dropzone;
