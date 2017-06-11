class Dropzone {
  constructor(container) {
    this.container = container;

    const input = container.querySelector('.js-drop-zone-file');
    const item = container.querySelector('.js-drop-zone-item');

    this.fileContainer = container.querySelector('.js-drop-zone-droppped');
    this.inputFilesCount = container.querySelector('.js-drop-zone-files-count');
    this.nameInput = document.querySelector('.js-drop-zone-name-input');
    this.acceptedFormatsStr = container.dataset.accepted;
    this.acceptedFormats = this.acceptedFormatsStr ? this.acceptedFormatsStr.split(',') : [];

    this.selector = 'isDropped';
    this.reverseSelector = 'isNotDropped';

    this.maxItems = !container.dataset.maxItems ? 10000000 : +container.dataset.maxItems;

    this.idealItem = item.cloneNode(true);
    this.idealInput = input.cloneNode(true);

    this.count = 0;
    this.number = 0;

    input.addEventListener('change', this.addFile.bind(this));
    this.createRemover(item);

    this.data = {
      [this.number]: { input, item }
    };

    const submitBtn = document.querySelector('.js-drop-zone-submit');
    submitBtn.addEventListener('click', this.submit.bind(this));
  }

  submit() {
    this.inputFilesCount.setAttribute('value', this.count);
    const inputs = this.container.querySelectorAll('.js-drop-zone-file');
    let index = 1;

    inputs.forEach((input) => {
      if (!input.value) return;
      input.setAttribute('name', `file${index}`);
      index += 1;
    });
  }

  addFile(event) {
    const { name, ext } = Dropzone.getFileFullName(event.target.files[0]);

    if (!this.isFormatAccepted(ext)) {
      this.container.classList.remove(this.selector);
      this.container.classList.add(this.reverseSelector);
      this.changeNameInput('');

      this.container.querySelector('.js-drop-zone-invalid-btn').addEventListener('click', () => {
        this.container.classList.remove(this.selector);
        this.container.classList.remove(this.reverseSelector);
        this.container.querySelector('.js-drop-zone-file').value = '';
        this.changeNameInput('');
      });

      return;
    }

    const id = event.target.dataset.id;

    Dropzone.setNameToItem(name, ext, this.data[id].item);
    this.changeNameInput(name);

    if (this.count === this.maxItems) return;

    if (this.count === 0) {
      this.container.classList.remove(this.reverseSelector);
      this.container.classList.add(this.selector);
    }

    this.count += 1;
    this.number += 1;

    const item = this.idealItem.cloneNode(true);
    this.createRemover(item);

    this.fileContainer.prepend(this.data[id].item);

    if (this.count === this.maxItems) return;

    const input = this.idealInput.cloneNode(true);
    input.setAttribute('data-id', this.number);
    input.addEventListener('change', this.addFile.bind(this));

    this.data[this.number] = {
      input, item
    };

    this.data[id].input.style.display = 'none';
    this.container.insertBefore(input, event.target);
  }

  removeFile(event) {
    const id = event.target.dataset.id;

    if (this.count === this.maxItems) {
      const prevInput = this.container.querySelector('.js-drop-zone-file');
      prevInput.style.display = 'none';

      const item = this.idealItem.cloneNode(true);
      this.createRemover(item);

      const input = this.idealInput.cloneNode(true);
      input.setAttribute('data-id', this.number);
      input.addEventListener('change', this.addFile.bind(this));

      this.data[this.number] = {
        input, item
      };

      this.container.insertBefore(input, prevInput);
      this.changeNameInput('');
    }

    this.count -= 1;
    const { item, input } = this.data[id];
    item.remove();
    input.remove();
    this.container.querySelector('.js-drop-zone-file').style.display = 'block';

    if (this.count === 0) {
      this.container.classList.remove(this.selector);
      this.container.classList.remove(this.reverseSelector);
    }

    delete this.data[id];

  }

  createRemover(item) {
    const remover = item.querySelector('.js-drop-zone-btn');
    remover.setAttribute('data-id', this.number);
    remover.addEventListener('click', this.removeFile.bind(this));
  }

  changeNameInput(value) {
    if (this.nameInput) if (!this.nameInput.hasAttribute('disabled')) this.nameInput.value = value;
  }

  isFormatAccepted(extension) {
    if (!this.acceptedFormatsStr) return true;
    return this.acceptedFormats.includes(extension);
  }

  static getFileFullName(file) {
    const { name } = file;
    const arrayName = name.split('.');
    const ext = arrayName[arrayName.length - 1];
    return { name, ext };
  }

  static setNameToItem(name, ext, item) {
    item.querySelector('.js-drop-zone-name').innerHTML = name;
    item.querySelector('.js-drop-zone-ext').innerHTML = `.${ext.toUpperCase()}`;
  }
}

export default Dropzone;
