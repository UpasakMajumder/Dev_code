import axios from 'axios';
/* helpers */
import { consoleException } from 'app.helpers/io';
import { getSecondLevelDomain } from 'app.helpers/location';
/* globals */
import { CHILI_SAVE } from 'app.globals';
/* classes */
import AddToCart from '../add-to-cart';

class ChiliEditor extends AddToCart {
  constructor(frame) {
    super();
    this.editor = null;
    this.frameWindow = null;

    // document.domain = getSecondLevelDomain();

    this.initEditor = this.initEditor.bind(this);
    this.addToCart = this.addToCart.bind(this);
    this.initActions = this.initActions.bind(this);
    this.editorLoaded = this.editorLoaded.bind(this);
    this.saveTemplate = this.saveTemplate.bind(this);
    this.revertTemplate = this.revertTemplate.bind(this);

    window.addToCart = this.addToCart;

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

  async addToCart(isAddToCart) {
    try {
      const { data: { success, errorMessage } } = await axios.post(CHILI_SAVE.url, this.getBody());
      if (success) {
        if (isAddToCart) this.addToCartRequest();
      } else {
        alert(errorMessage); // eslint-disable-line no-alert
      }
    } catch (e) {
      alert(e); // eslint-disable-line no-alert
    }
  }

  initActions() {
    const saveBtn = document.querySelector('.js-chili-save');
    if (saveBtn) {
      saveBtn.disabled = false;
      saveBtn.addEventListener('click', () => this.saveTemplate(false));
    }

    const addToCartBtn = document.querySelector('.js-chili-addtocart');
    if (addToCartBtn) {
      saveBtn.disabled = false;
      addToCartBtn.addEventListener('click', () => this.saveTemplate(true));
    }

    const revertBtn = document.querySelector('.js-chili-revert');
    if (revertBtn) revertBtn.addEventListener('click', this.revertTemplate);
  }

  saveTemplate(isAddToCart) {
    this.editor.ExecuteFunction('document', 'Save');
    this.addToCart(isAddToCart);
  }

  revertTemplate() {
    this.editor.ExecuteFunction('document', 'Revert');
  }
}

export default ChiliEditor;
