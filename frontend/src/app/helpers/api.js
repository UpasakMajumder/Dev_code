import axios from 'axios';
/* wc */
import { newState } from 'app.ws/cartPreviewUI';
/* constants */
import { CART_PREVIEW_CHANGE_ITEMS } from 'app.consts';
/* globals */
import { ADD_TO_CART_URL } from 'app.globals';

export const addToCartRequest = (body) => {
  const dispatch = store.dispatch; // eslint-disable-line no-undef

  // return new Promise((resolve) => {
  //   if (true) {
  //     dispatch({
  //       type: CART_PREVIEW_CHANGE_ITEMS,
  //       payload: {
  //         items: newState.items,
  //         summaryPrice: newState.summaryPrice
  //       }
  //     });
  //   } else {
  //     resolve('hi');
  //   }
  // });


  return new Promise((resolve) => {
    axios.post(ADD_TO_CART_URL, body)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          resolve(errorMessage);
          return;
        }

        dispatch({
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
  });
};

export const bla = 1;
