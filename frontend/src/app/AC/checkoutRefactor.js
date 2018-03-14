import axios from 'axios';
/* constants */
import {
  FAILURE,
  SUCCESS,
  CHECKOUT_INIT_ITEMS,
  CHECKOUT_REMOVE_PRODUCT,
  CHECKOUT_CHANGE_PRODUCT_QUANTITY,
  CHECKOUT_GET_TOTALS
} from 'app.consts';

export const initItems = (payload) => {
  return {
    type: CHECKOUT_INIT_ITEMS,
    payload
  };
};

export const removeProduct = (url, id) => {
  return async (dispatch) => {
    try {
      const { data } = await axios.delete(url);
      const { success, payload, errorMessage } = data;
      if (success) {
        dispatch({
          type: CHECKOUT_REMOVE_PRODUCT + SUCCESS,
          payload: {
            id,
            quantityText: payload.quantityText
          }
        });
      } else {
        dispatch({
          type: CHECKOUT_REMOVE_PRODUCT + FAILURE,
          alert: errorMessage
        });
      }
    } catch (e) {
      dispatch({ type: CHECKOUT_REMOVE_PRODUCT + FAILURE });
    }
  };
};

export const changeProductQuantity = (url, id, quantity) => {
  return async (dispatch) => {
    try {
      const { data } = await axios.put(url, { quantity });
      const { success, errorMessage } = data;
      if (success) {
        dispatch({
          type: CHECKOUT_CHANGE_PRODUCT_QUANTITY + SUCCESS,
          payload: {
            id,
            quantity
          }
        });
      } else {
        dispatch({
          type: CHECKOUT_CHANGE_PRODUCT_QUANTITY + FAILURE,
          alert: errorMessage
        });
      }
    } catch (e) {
      dispatch({ type: CHECKOUT_CHANGE_PRODUCT_QUANTITY + FAILURE });
    }
  };
};

export const getTotals = (url) => {
  return async (dispatch) => {
    try {
      const { data } = await axios.get(url);
      const { success, errorMessage, payload } = data;
      if (success) {
        dispatch({
          type: CHECKOUT_GET_TOTALS + SUCCESS,
          payload: {
            totals: payload.totals
          }
        });
      } else {
        dispatch({
          type: CHECKOUT_GET_TOTALS + FAILURE,
          alert: errorMessage
        });
      }
    } catch (e) {
      dispatch({ type: CHECKOUT_GET_TOTALS + FAILURE });
    }
  };
};
