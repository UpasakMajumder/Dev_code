import axios from 'axios';
/* constants */
import { CART_PREVIEW, INIT_UI, FETCH, SUCCESS, FAILURE, TOGGLE, SHOW, HEADER_SHADOW, HIDE } from 'app.consts';
/* globals */
import { CART_PREVIEW as CART_PREVIEW_GLOBAL } from 'app.globals';
/* web service */
import ui from 'app.ws/cartPreviewUI';

export const getUI = () => {
  return (dispatch) => {
    dispatch({ type: CART_PREVIEW + INIT_UI + FETCH });

    const prod = () => {
      axios({
        method: 'get',
        url: CART_PREVIEW_GLOBAL.cartPreviewUrl
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: CART_PREVIEW + INIT_UI + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
        } else {
          dispatch({
            type: CART_PREVIEW + INIT_UI + SUCCESS,
            payload: {
              emptyCartMessage: payload.emptyCartMessage,
              cart: payload.cart,
              items: payload.items
            }
          });
        }
      }).catch((error) => {
        dispatch({ type: CART_PREVIEW + INIT_UI + FAILURE });
        alert(error); // eslint-disable-line no-alert
      });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: CART_PREVIEW + INIT_UI + SUCCESS,
          payload: {
            emptyCartMessage: ui.emptyCartMessage,
            cart: ui.cart,
            items: ui.items,
            totalPrice: ui.totalPrice
          }
        });
      }, 0);
    };

    // dev();
    prod();
  };
};

export const togglePreview = (isVisible) => {
  return (dispatch) => {
    if (isVisible) {
      dispatch({ type: HEADER_SHADOW + SHOW });
    } else {
      dispatch({ type: HEADER_SHADOW + HIDE });
    }

    dispatch({
      type: CART_PREVIEW + TOGGLE,
      payload: { isVisible }
    });
  };
};
