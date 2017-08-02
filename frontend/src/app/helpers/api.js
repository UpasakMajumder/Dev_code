import axios from 'axios';
/* wc */
import { newState } from 'app.ws/cartPreviewUI';
/* constants */
import { CART_PREVIEW_CHANGE_ITEMS } from 'app.consts';
/* globals */
import { ADD_TO_CART_URL } from 'app.globals';

export const addToCartRequest = (id, number) => {
  const dev = () => {
    setTimeout(() => {
      store.dispatch({
        type: CART_PREVIEW_CHANGE_ITEMS,
        payload: {
          items: newState.items,
          summaryPrice: newState.summaryPrice
        }
      });
    }, 2000);
  };

  const prod = () => {
    axios.post(ADD_TO_CART_URL, { id, number })
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          // show errorMessage
          return;
        }

        store.dispatch({
          type: CART_PREVIEW_CHANGE_ITEMS,
          payload: {
            items: payload.items,
            summaryPrice: payload.summaryPrice
          }
        });
      })
      .catch((error) => {
        alert(error); // eslint-disable-line no-alert
      });
  };


  dev();
  // prod();
};

export const bla = 1;
