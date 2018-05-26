import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* constants */
import { CART_PREVIEW_CHANGE_ITEMS, HEADER_SHADOW, HIDE, FAILURE } from 'app.consts';
/* helpers */
import { getSecondLevelDomain } from 'app.helpers/location';
import { toggleDialogAlert } from 'app.helpers/ac';
/* globals */
import { CHILI_SAVE, NOTIFICATION, ADD_TO_CART_URL, BUTTONS_UI } from 'app.globals';

/**
 * frame load
 * init editor
 * if editor okay
 * init actions
 *
 * handle save
 * trigger chili
 * callback invoked
 * send request
 * show toastr
 *
 * handle add to cart
 * trigger chili
 * callback invoked
 * send request to save
 * show toastr
 * send request to add to cart
 * show dialog
 */


class ChiliEditor {
  constructor(frame) {
    this.editor = null;
    this.isChiliAvailbale = false;

    // cross-origin frame
    const newDomain = getSecondLevelDomain();
    if (newDomain) document.domain = newDomain;

    window.chiliCallback = this.chiliCallback;

    // init editor and init actions
    frame.addEventListener('load', () => {
      this.initEditor(frame);
      this.initActions();
    });

    this.showMessageClass = 'input--error';
    this.nameElement = document.querySelector('.js-add-to-cart-name');
    this.quantityElement = document.querySelector('.js-add-to-cart-quantity');
    this.wrappers = [...document.querySelectorAll('.js-add-to-cart-error')];
    this.properyFields = [...document.querySelectorAll('.js-add-to-cart-property')];
  }

  initEditor = (frame) => {
    try {
      const { contentWindow } = frame;
      contentWindow.GetEditor(() => {
        this.editor = contentWindow.editorObject;
      });
      this.isChiliAvailbale = true;
    } catch (e) {
      toastr.error(NOTIFICATION.chiliNotAvailable.title, NOTIFICATION.chiliNotAvailable.text);
      this.isChiliAvailbale = false;
    }
  };

  initActions = () => {
    const saveBtn = document.querySelector('.js-chili-save');
    if (saveBtn && this.isChiliAvailbale) {
      saveBtn.disabled = false;
      saveBtn.addEventListener('click', () => this.triggerChiliSave());
    }

    const addToCartBtn = document.querySelector('.js-chili-addtocart');
    if (addToCartBtn) {
      addToCartBtn.disabled = false;
      addToCartBtn.addEventListener('click', event => this.addToCart());
    }

    const revertBtn = document.querySelector('.js-chili-revert');
    if (revertBtn && this.isChiliAvailbale) {
      revertBtn.disabled = false;
      revertBtn.addEventListener('click', this.revertTemplate);
    }
  };

  revertTemplate = () => {
    if (this.isChiliAvailbale) this.editor.ExecuteFunction('document', 'Revert');
  };

  triggerChiliSave = () => {
    this.editor.ExecuteFunction('document', 'Save');
  };

  addToCart = () => {
    if (this.isChiliAvailbale) {
      this.chiliEventType = 'add';
      this.triggerChiliSave();
    } else {
      this.addToCartCallback();
    }
  }

  // callback from HTML `product-editor.nunj`
  // callback method for Chili editor save action
  chiliCallback = async () => {
    await this.saveChiliTemplateCallback();
    if (this.chiliEventType !== 'add') return;
    await this.addToCartCallback();
  }

  getBody = () => {
    const customProductName = this.nameElement && this.nameElement.value;
    const quantity = this.quantityElement ? this.quantityElement.value : 0;

    const body = { customProductName, quantity, options: {} };

    this.properyFields.forEach((field) => {
      const { name, value } = field;

      if (field.classList.contains('js-product-option')) {
        if (field.type === 'radio') {
          if (field.checked) body.options[name] = value;
        } else {
          body.options[name] = value;
        }
      } else {
        body[name] = value;
      }
    });

    return body;
  };

  saveChiliTemplateCallback = () => {
    return axios
      .post(CHILI_SAVE.url, this.getBody())
      .then((response) => {
        const { success, errorMessage } = response.data;

        if (success) {
          toastr.success(NOTIFICATION.chiliSaved.title, NOTIFICATION.chiliSaved.text);
        } else {
          toastr.error(errorMessage);
        }
      })
      .catch(() => {
        toastr.error(NOTIFICATION.serverNotAvailable.title, NOTIFICATION.serverNotAvailable.text);
      });
  }

  addToCartCallback = () => {
    this.wrappers.forEach(wrapper => wrapper.classList.remove(this.showMessageClass));

    return axios
      .post(ADD_TO_CART_URL, this.getBody())
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          const { confirmation, cartPreview } = payload;

          window.store.dispatch({
            type: CART_PREVIEW_CHANGE_ITEMS,
            payload: {
              items: cartPreview.items,
              summaryPrice: cartPreview.summaryPrice
            }
          });

          const confirmBtn = [
            {
              label: BUTTONS_UI.products.text,
              func: () => window.location.assign(BUTTONS_UI.products.url)
            },
            {
              label: BUTTONS_UI.checkout.text,
              func: () => window.location.assign(BUTTONS_UI.checkout.url)
            }
          ];

          const closeDialog = () => {
            toggleDialogAlert(false);
            window.store.dispatch({ type: HEADER_SHADOW + HIDE });
          };

          toggleDialogAlert(true, confirmation.alertMessage, closeDialog, confirmBtn);
        } else {
          this.wrappers.forEach(wrapper => wrapper.classList.add(this.showMessageClass));
          const messageElement = document.querySelector('.js-add-to-cart-message');
          messageElement.innerHTML = errorMessage;
        }
      })
      .catch(() => {
        window.store.dispatch({ type: CART_PREVIEW_CHANGE_ITEMS + FAILURE });
      });
  };
}

export default ChiliEditor;
