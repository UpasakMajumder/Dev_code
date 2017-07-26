import { consoleException } from 'app.helpers/io';

class ChiliEditor {
  constructor(frame) {
    this.editor = null;
    this.frameWindow = null;

    this.initEditor = this.initEditor.bind(this);
    this.initActions = this.initActions.bind(this);
    this.editorLoaded = this.editorLoaded.bind(this);
    this.saveTemplate = this.saveTemplate.bind(this);
    this.revertTemplate = this.revertTemplate.bind(this);

    frame.addEventListener('load', () => {
      this.initEditor(frame);
      this.initActions();
    });
  }

  editorLoaded() {
    this.editor = this.frameWindow.editorObject;
  }

  initEditor(frame) {
    this.frameWindow = frame.contentWindow;
    this.frameWindow.GetEditor(this.editorLoaded);
  }

  initActions() {
    const saveBtn = document.querySelector('.js-chili-save');
    if (!saveBtn) {
      consoleException('No found save btn with .js-chili-save');
      return;
    }

    const addToCartBtn = document.querySelector('.js-chili-addtocart');
    if (!addToCartBtn) {
      consoleException('No found add to cart btn with .js-chili-addtocart');
      return;
    }

    const revertBtn = document.querySelector('.js-chili-revert');
    if (!addToCartBtn) {
      consoleException('No found revert btn with .js-chili-revert');
      return;
    }

    saveBtn.addEventListener('click', this.saveTemplate);
    addToCartBtn.addEventListener('click', this.saveTemplate);
    revertBtn.addEventListener('click', this.revertTemplate);
  }

  saveTemplate() {
    this.editor.ExecuteFunction('document', 'Save');
  }

  revertTemplate() {
    this.editor.ExecuteFunction('document', 'Revert');
  }
}

export default ChiliEditor;
