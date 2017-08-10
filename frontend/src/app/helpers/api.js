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
    return new Promise((resolve) => {
      dispatch({
        type: CART_PREVIEW_CHANGE_ITEMS,
        payload: {
          items: newState.items,
          summaryPrice: newState.summaryPrice
        }
      });

      const confirmBtn = [
        {
          label: newState.cart.btns.cancel,
          func: () => window.location.assign(newState.cart.productUrl)
        },
        {
          label: newState.cart.btns.checkout,
          func: () => window.location.assign(newState.cart.url)
        }
      ];

      toggleDialogAlert(true, newState.alertMessage, closeDialog, confirmBtn);
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

        const { alertMessage, cart, items, summaryPrice } = payload;

        dispatch({
          type: CART_PREVIEW_CHANGE_ITEMS,
          payload: { items, summaryPrice }
        });

        const confirmBtn = [
          {
            label: cart.btns.cancel,
            func: () => window.location.assign(cart.productUrl)
          },
          {
            label: cart.btns.checkout,
            func: () => window.location.assign(cart.url)
          }
        ];

        toggleDialogAlert(true, alertMessage, closeDialog, confirmBtn);
      })
      .catch((error) => {
        alert(error); // eslint-disable-line no-alert
      });
  });
};

export const bla = 1;
