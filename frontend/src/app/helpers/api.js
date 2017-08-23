import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* wc */
import { newState } from 'app.ws/cartPreviewUI';
/* constants */
import { CART_PREVIEW_CHANGE_ITEMS, HEADER_SHADOW, HIDE, isDevelopment, FAILURE } from 'app.consts';
/* globals */
import { ADD_TO_CART_URL, NOTIFICATION, BUTTONS_UI } from 'app.globals';
/* helpers */
import { toggleDialogAlert } from 'app.helpers/ac';
import { toastr } from 'react-redux-toastr';

export const addToCartRequest = (body) => {
  const dispatch = window.store.dispatch;
  const closeDialog = () => {
    toggleDialogAlert(false);
    dispatch({ type: HEADER_SHADOW + HIDE });
  };

  if (isDevelopment) {
    const { confirmation, cartPreview } = newState;

    return new Promise((resolve) => {
      dispatch({
        type: CART_PREVIEW_CHANGE_ITEMS,
        payload: {
          items: cartPreview.items,
          summaryPrice: cartPreview.summaryPrice
        }
      });

      toastr.success(NOTIFICATION.addProduct.title, NOTIFICATION.addProduct.text);

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

      toggleDialogAlert(true, confirmation.alertMessage, closeDialog, confirmBtn);
      // resolve('hi');
    });
  }

  return new Promise((resolve) => {
    axios.post(ADD_TO_CART_URL, body)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          resolve(errorMessage);
          return;
        }

        const { confirmation, cartPreview } = payload;

        dispatch({
          type: CART_PREVIEW_CHANGE_ITEMS,
          payload: {
            items: cartPreview.items,
            summaryPrice: cartPreview.summaryPrice
          }
        });

        toastr.success(NOTIFICATION.addProduct.title, NOTIFICATION.addProduct.text);

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

        toggleDialogAlert(true, confirmation.alertMessage, closeDialog, confirmBtn);
      })
      .catch((error) => {
        dispatch({ type: CART_PREVIEW_CHANGE_ITEMS + FAILURE });
      });
  });
};

export const bla = 1;
