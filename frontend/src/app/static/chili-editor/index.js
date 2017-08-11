/* helpers */
import { consoleException } from 'app.helpers/io';
import { getSecondLevelDomain } from 'app.helpers/location';

class ChiliEditor {
  constructor(frame) {
    this.editor = null;
    this.frameWindow = null;

    document.domain = getSecondLevelDomain();

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
    if (saveBtn) saveBtn.addEventListener('click', this.saveTemplate);

    const addToCartBtn = document.querySelector('.js-chili-addtocart');
    if (addToCartBtn) addToCartBtn.addEventListener('click', this.saveTemplate);

    const revertBtn = document.querySelector('.js-chili-revert');
    if (revertBtn) revertBtn.addEventListener('click', this.revertTemplate);
  }

  saveTemplate() {
    this.editor.ExecuteFunction('document', 'Save');
  }

  revertTemplate() {
    this.editor.ExecuteFunction('document', 'Revert');
  }
}

export default ChiliEditor;
