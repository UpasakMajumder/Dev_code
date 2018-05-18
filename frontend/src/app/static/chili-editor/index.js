import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* helpers */
import { consoleException } from 'app.helpers/io';
import { getSecondLevelDomain } from 'app.helpers/location';
/* globals */
import { CHILI_SAVE, NOTIFICATION } from 'app.globals';
/* classes */
import AddToCart from '../add-to-cart';

class ChiliEditor extends AddToCart {
  constructor(frame) {
    super();
    this.editor = null;
    this.frameWindow = null;
    this.chiliWorks = false;

    const newDomain = getSecondLevelDomain();
    if (newDomain) document.domain = newDomain;

    this.initEditor = this.initEditor.bind(this);
    this.addToCart = this.addToCart.bind(this);
    this.initActions = this.initActions.bind(this);
    this.editorLoaded = this.editorLoaded.bind(this);
    this.saveChiliTemplate = this.saveChiliTemplate.bind(this);
    this.revertTemplate = this.revertTemplate.bind(this);
    this.triggerChiliSave = this.triggerChiliSave.bind(this);

    window.saveChiliTemplate = this.saveChiliTemplate;

    frame.addEventListener('load', () => {
      this.initEditor(frame);
    });
  }

  editorLoaded() {
    this.editor = this.frameWindow.editorObject;
  }

  initEditor(frame) {
    try {
      this.frameWindow = frame.contentWindow;
      this.frameWindow.GetEditor(this.editorLoaded);
      this.chiliWorks = true;
      this.initActions();
    } catch (e) {
      toastr.error(NOTIFICATION.chiliNotAvailable.title, NOTIFICATION.chiliNotAvailable.text);
      this.chiliWorks = false;
    }
  }

  initActions() {
    const saveBtn = document.querySelector('.js-chili-save');
    if (saveBtn && this.chiliWorks) {
      saveBtn.disabled = false;
      saveBtn.addEventListener('click', () => this.triggerChiliSave());
    }

    const addToCartBtn = document.querySelector('.js-chili-addtocart');
    if (addToCartBtn) {
      addToCartBtn.disabled = false;
      addToCartBtn.addEventListener('click', event => this.addToCart(event));
    }

    const revertBtn = document.querySelector('.js-chili-revert');
    if (revertBtn) revertBtn.addEventListener('click', this.revertTemplate);
  }

  addToCart(event) {
    // callback from HTML `product-editor.nunj`
    this.cartEvent = event;
    this.triggerChiliSave();
  }

  triggerChiliSave() {
    this.editor.ExecuteFunction('document', 'Save');
  }

  // callback method for Chili editor save action
  async saveChiliTemplate() {
    try {
      if (this.chiliWorks) {
        const { data: { success, errorMessage } } = await axios.post(CHILI_SAVE.url, this.getBody());
        if (success) {
          toastr.success(NOTIFICATION.chiliSaved.title, NOTIFICATION.chiliSaved.text);
          this.addToCartRequest(this.cartEvent);
        } else {
          toastr.error(errorMessage);
        }
      }
    } catch (e) {
      toastr.error(NOTIFICATION.serverNotAvailable.title, NOTIFICATION.serverNotAvailable.text);
    }
  }

  revertTemplate() {
    if (this.chiliWorks) this.editor.ExecuteFunction('document', 'Revert');
  }
}

export default ChiliEditor;
