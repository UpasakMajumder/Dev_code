class Dropzone {
  constructor(container) {
    this.container = container;
    this.file = container.querySelector('.js-drop-zone-file');
    this.btn = container.querySelector('.js-drop-zone-btn');
    this.nameNode = container.querySelector('.js-drop-zone-name');
    this.nameInput = document.querySelector('.js-drop-zone-name-input');
    this.selector = 'isDropped';


    this.file.addEventListener('change', (event) => {
      const name = event.target.files[0].name;

      !this.container.classList.contains(this.selector) && this.container.classList.add(this.selector);
      this.nameNode.innerHTML = name;
      this.nameInput.value = name;
    });

    this.btn.addEventListener('click', (event) => {
      this.container.classList.remove(this.selector);
      this.file.value = '';
      this.nameInput.value = '';
      event.preventDefault();
    });
  }
}

export default Dropzone;
