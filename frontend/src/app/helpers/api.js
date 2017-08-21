import axios from 'axios';
/* wc */
import { newState } from 'app.ws/cartPreviewUI';
/* constants */
import { CART_PREVIEW_CHANGE_ITEMS, HEADER_SHADOW, HIDE, isDevelopment } from 'app.consts';
/* globals */
import { ADD_TO_CART_URL } from 'app.globals';
import { toastr } from 'react-redux-toastr';
/* helpers */
import { toggleDialogAlert } from 'app.helpers/ac';

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

      const confirmBtn = [
        {
          label: confirmation.btns.cancel.text,
          func: () => window.location.assign(confirmation.btns.cancel.url)
        },
        {
          label: confirmation.btns.checkout.text,
          func: () => window.location.assign(confirmation.btns.checkout.url)
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

        const confirmBtn = [
          {
            label: confirmation.btns.cancel.text,
            func: () => window.location.assign(confirmation.btns.cancel.url)
          },
          {
            label: confirmation.btns.checkout.text,
            func: () => window.location.assign(confirmation.btns.checkout.url)
          }
        ];

        toggleDialogAlert(true, confirmation.alertMessage, closeDialog, confirmBtn);
      })
      .catch((error) => {
        alert(error); // eslint-disable-line no-alert
      });
  });
};

export const bla = 1;
