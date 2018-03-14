import axios from 'axios';
/* constants */
import {
  APP_LOADING,
  START,
  FINISH,
  FAILURE,
  SUCCESS,
  CHECKOUT_INIT_ITEMS,
  CHECKOUT_REMOVE_PRODUCT,
  CHECKOUT_CHANGE_PRODUCT_QUANTITY
} from 'app.consts';

export const initItems = ({
  products,
  quantityText
}) => {
  return {
    type: CHECKOUT_INIT_ITEMS,
    payload: {
      products,
      quantityText
    }
  };
};

export const removeProduct = (url, id) => {
  return async (dispatch) => {
    dispatch({ type: APP_LOADING + START });

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
    } finally {
      dispatch({ type: APP_LOADING + FINISH });
    }
  };
};

export const changeProductQuantity = (url, id, quantity) => {
  return async (dispatch) => {
    dispatch({ type: APP_LOADING + START });

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
    } finally {
      dispatch({ type: APP_LOADING + FINISH });
    }
  };
};
