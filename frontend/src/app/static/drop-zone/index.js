class Dropzone {
  constructor(container) {
    this.container = container;
    this.file = container.querySelector('.js-drop-zone-file');
    this.btns = Array.from(container.querySelectorAll('.js-drop-zone-btn'));
    this.nameNode = container.querySelector('.js-drop-zone-name');
    this.extensionNode = container.querySelector('.js-drop-zone-ext');
    this.nameInput = document.querySelector('.js-drop-zone-name-input');
    this.selector = 'isDropped';
    this.reverseSelector = 'isNotDropped';
    this.acceptedFormatsStr = container.dataset.accepted;
    this.acceptedFormats = this.acceptedFormatsStr ? this.acceptedFormatsStr.split(',') : [];

    this.file.addEventListener('change', (event) => {

      if (!this.file.value) {
        this.container.classList.remove(this.reverseSelector);
        this.container.classList.remove(this.selector);
        this.changeNameInput('');
        return;
      }

      const { name } = event.target.files[0];
      const arrayName = name.split('.');
      const extension = arrayName[arrayName.length - 1];

      if (!this.isFormatAccepted(extension)) {
        this.container.classList.remove(this.selector);
        this.container.classList.add(this.reverseSelector);
        this.changeNameInput(name);
        return;
      }

      this.container.classList.remove(this.reverseSelector);
      this.container.classList.add(this.selector);
      this.nameNode.innerHTML = name;
      this.extensionNode.innerHTML = `.${extension.toUpperCase()}`;
      this.changeNameInput(name);
    });

    this.btns.forEach((btn) => {

      btn.addEventListener('click', (event) => {
        this.container.classList.remove(this.selector);
        this.container.classList.remove(this.reverseSelector);
        this.file.value = '';
        this.changeNameInput('');
        event.preventDefault();
      });
    });
  }

  changeNameInput(value) {
    if (this.nameInput) if (!this.nameInput.hasAttribute('disabled')) this.nameInput.value = value;
  }

  isFormatAccepted(extension) {
    if (!this.acceptedFormatsStr) return true;
    return this.acceptedFormats.includes(extension);
  }
}

export default Dropzone;
