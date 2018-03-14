import axios from 'axios';
/* constants */
import { CART_PREVIEW, INIT_UI, FETCH, SUCCESS, FAILURE, TOGGLE, SHOW, HEADER_SHADOW, HIDE, CART_PREVIEW_CHANGE_ITEMS } from 'app.consts';
/* globals */
import { CART_PREVIEW as CART_PREVIEW_GLOBAL } from 'app.globals';

export const getUI = () => {
  return (dispatch) => {
    dispatch({ type: CART_PREVIEW + INIT_UI + FETCH });

    axios({
      method: 'get',
      url: CART_PREVIEW_GLOBAL.cartPreviewUrl
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: CART_PREVIEW + INIT_UI + FAILURE,
          alert: errorMessage
        });
      } else {
        dispatch({
          type: CART_PREVIEW + INIT_UI + SUCCESS,
          payload: {
            emptyCartMessage: payload.emptyCartMessage,
            cart: payload.cart,
            items: payload.items,
            summaryPrice: payload.summaryPrice
          }
        });
      }
    }).catch((error) => {
      dispatch({ type: CART_PREVIEW + INIT_UI + FAILURE });
    });
  };
};

export const changeProducts = (items, summaryPrice) => {
  return (dispatch) => {
    dispatch({
      type: CART_PREVIEW_CHANGE_ITEMS,
      payload: {
        items,
        summaryPrice
      }
    });
  };
};
