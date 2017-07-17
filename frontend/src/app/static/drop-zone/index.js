// @flow

/* helpers */
import { consoleException } from 'app.helpers/io';

class Dropzone {
  container: HTMLElement;
  fileContainer: ?HTMLElement;
  inputFilesCount: ?HTMLElement;
  nameInput: ?HTMLElement;
  acceptedFormatsStr: ?string;
  acceptedFormats: [];
  selector: string;
  reverseSelector: string;
  maxItems: number;
  idealItem: HTMLElement;
  idealInput: HTMLElement;
  count: number;
  number: number;
  data: {};

  constructor(container: HTMLElement) {
    this.container = container;

    const input: ?HTMLElement = container.querySelector('.js-drop-zone-file');
    const item: ?HTMLElement = container.querySelector('.js-drop-zone-item');

    this.fileContainer = container.querySelector('.js-drop-zone-droppped');
    this.inputFilesCount = container.querySelector('.js-drop-zone-files-count');
    this.nameInput = document.querySelector('.js-drop-zone-name-input');

    if (!item || !input) {
      consoleException('No found item or input');
      return;
    }

    this.acceptedFormatsStr = container.dataset.accepted;

    // $FlowIgnore
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

    const submitBtn: ?HTMLElement = document.querySelector('.js-drop-zone-submit');

    if (!submitBtn) {
      consoleException('No submit button found with selector .js-drop-zone-submit');
      return;
    }

    submitBtn.addEventListener('click', this.submit.bind(this));
  }

  submit() {
    if (!this.inputFilesCount) {
      consoleException('No found inputFilesCount with selector .js-drop-zone-files-count');
      return;
    }
    this.inputFilesCount.setAttribute('value', this.count.toString());
    let index: number = 1;

    const inputs: ?HTMLElement[] = Array.from(this.container.querySelectorAll('.js-drop-zone-file'));

    if (!inputs) {
      consoleException('No inputs with .js-drop-zone-file');
      return;
    }

    inputs.forEach((input) => {
      if (!input.value) return;
      input.setAttribute('name', `file${index}`);
      index += 1;
    });
  }

  addFile(event: Event): void {
    const target: EventTarget = event.target;
    if (!(target instanceof HTMLInputElement)) return;

    const { name, ext }: { name: string, ext: string } = Dropzone.getFileFullName(target.files[0]);

    if (!this.isFormatAccepted(ext)) {
      this.container.classList.remove(this.selector);
      this.container.classList.add(this.reverseSelector);
      this.changeNameInput('');

      const invalidBtn: ?HTMLElement = this.container.querySelector('.js-drop-zone-invalid-btn');

      if (!invalidBtn) {
        consoleException('No btns with .js-drop-zone-invalid-btn');
        return;
      }

      invalidBtn.addEventListener('click', () => {
        this.container.classList.remove(this.selector);
        this.container.classList.remove(this.reverseSelector);

        const file: ?HTMLElement = this.container.querySelector('.js-drop-zone-file');
        if (file) {
          file.setAttribute('value', '');
        }

        this.changeNameInput('');
      });

      return;
    }

    const id: string = target.dataset.id;

    Dropzone.setNameToItem(name, ext, this.data[id].item);
    this.changeNameInput(name);

    this.container.classList.remove(this.reverseSelector);
    this.container.classList.add(this.selector);


    this.count += 1;
    this.number += 1;

    const item: HTMLElement = this.idealItem.cloneNode(true);
    this.createRemover(item);

    if (!this.fileContainer) {
      consoleException('No fileContainer with .js-drop-zone-droppped');
      return;
    }

    this.fileContainer.insertBefore(this.data[id].item, this.fileContainer.firstChild);

    if (this.count === this.maxItems) {
      this.data[id].input.style.display = 'none';

      const input: HTMLElement = this.idealInput.cloneNode(true);
      input.setAttribute('id', 'last');
      input.style.display = 'none';
      this.container.insertBefore(input, target);

      return;
    }

    const input: HTMLElement = this.idealInput.cloneNode(true);
    input.setAttribute('data-id', this.number.toString());
    input.addEventListener('change', this.addFile.bind(this));

    this.data[this.number] = {
      input, item
    };

    this.data[id].input.style.display = 'none';
    this.container.insertBefore(input, target);
  }

  removeFile(event: Event): void {
    const target: EventTarget = event.target;
    if (!(target instanceof HTMLElement)) return;

    const id: string = target.dataset.id;

    if (this.count === this.maxItems) {
      const lastInput: ?HTMLElement = this.container.querySelector('#last');

      if (!lastInput || !lastInput.parentNode) {
        consoleException('No lastInput with #last');
        return;
      }

      lastInput.parentNode.removeChild(lastInput);

      const prevInput: ?HTMLElement = this.container.querySelector('.js-drop-zone-file');

      if (!prevInput) {
        consoleException('No prevInput with .js-drop-zone-file');
        return;
      }

      prevInput.style.display = 'none';

      const item: HTMLElement = this.idealItem.cloneNode(true);
      this.createRemover(item);

      const input: HTMLElement = this.idealInput.cloneNode(true);
      input.setAttribute('data-id', this.number.toString());
      input.addEventListener('change', this.addFile.bind(this));

      this.data[this.number] = {
        input, item
      };

      this.container.insertBefore(input, prevInput);
      this.changeNameInput('');
    }

    this.count -= 1;
    const { item, input }: { item: HTMLElement, input: HTMLElement } = this.data[id];

    if (!input.parentNode) {
      consoleException('No parentNode of', input);
      return;
    }
    input.parentNode.removeChild(input);

    if (!item.parentNode) {
      consoleException('No parentNode of', item);
      return;
    }
    item.parentNode.removeChild(item);

    const fileElement: ?HTMLElement = this.container.querySelector('.js-drop-zone-file');

    if (!fileElement) {
      consoleException('No file with .js-drop-zone-file');
      return;
    }

    fileElement.style.display = 'block';

    if (this.count === 0) {
      this.container.classList.remove(this.selector);
      this.container.classList.remove(this.reverseSelector);
    }

    delete this.data[id];
  }

  createRemover(item: HTMLElement): void {
    const remover: ?HTMLElement = item.querySelector('.js-drop-zone-btn');

    if (!remover) {
      consoleException('No btn with .js-drop-zone-btn');
      return;
    }

    remover.setAttribute('data-id', this.number.toString());
    remover.addEventListener('click', this.removeFile.bind(this));
  }

  changeNameInput(value: string): void {
    if (this.nameInput && !this.nameInput.hasAttribute('disabled')) {
      this.nameInput.setAttribute('value', value);
    }
  }

  isFormatAccepted(extension: string): boolean {
    if (!this.acceptedFormatsStr) return true;
    return this.acceptedFormats.includes(extension);
  }

  static getFileFullName(file: File): { name: string, ext: string } {
    const { name }: { name: string } = file;
    const arrayName: string[] = name.split('.');
    const ext: string = arrayName[arrayName.length - 1];
    return { name, ext };
  }

  static setNameToItem(name: string, ext: string, item: HTMLElement): void {
    const dropZoneNameElement: ?HTMLElement = item.querySelector('.js-drop-zone-name');
    const dropZoneExtElement: ?HTMLElement = item.querySelector('.js-drop-zone-ext');
    if (!dropZoneNameElement || !dropZoneExtElement) {
      consoleException('No found fields for name and ext for the file');
      return;
    }

    dropZoneNameElement.innerHTML = name;
    dropZoneExtElement.innerHTML = `.${ext.toUpperCase()}`;
  }
}

export default Dropzone;
