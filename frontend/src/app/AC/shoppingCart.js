import axios from 'axios';
import { SHOPPING_CART_UI_FETCH, SHOPPING_CART_UI_SUCCESS, SHOPPING_CART_UI_FAILURE, CHANGE_SHOPPING_DATA,
  INIT_CHECKED_SHOPPING_DATA, RECALCULATE_SHOPPING_PRICE_FETCH, RECALCULATE_SHOPPING_PRICE_SUCCESS,
  RECALCULATE_SHOPPING_PRICE_FAILURE, SEND_SHOPPING_DATA_FETCH, SEND_SHOPPING_DATA_FAILURE,
  SEND_SHOPPING_DATA_SUCCESS, ERROR_SHOPPING_VALIDATION } from '../constants';
import { CHECKOUT } from '../globals';
// import ui from './ui';

export const getUI = () => {
  return (dispatch) => {
    dispatch({
      type: SHOPPING_CART_UI_FETCH
    });

    // dispatch({
    //   type: SHOPPING_CART_UI_SUCCESS,
    //   payload: {
    //     ui: ui.payload
    //   }
    // });

    axios.get(CHECKOUT.initUIURL)
      .then((response) => {
        dispatch({
          type: SHOPPING_CART_UI_SUCCESS,
          payload: {
            ui: response.data.payload
          }
        });
      })
      .catch(() => {
        dispatch({
          type: SHOPPING_CART_UI_FAILURE
        });
      });
  };
};

export const initCheckedShoppingData = (data) => {
  return (dispatch) => {
    dispatch({
      type: INIT_CHECKED_SHOPPING_DATA,
      payload: {
        ...data
      }
    });
  };
};

export const changeShoppingData = (field, id, invoice) => {
  return (dispatch) => {
    dispatch({
      type: CHANGE_SHOPPING_DATA,
      payload: {
        field, id, invoice
      }
    });

    if (field === 'paymentMethod') return;

    dispatch({
      type: RECALCULATE_SHOPPING_PRICE_FETCH
    });

    const url = field === 'deliveryMethod'
      ? CHECKOUT.changeDeliveryMethodURL
      : (field === 'deliveryAddress')
      ? CHECKOUT.changeAddressURL
      : '';

    axios.post(url, { id })
      .then((response) => {
        dispatch({
          type: RECALCULATE_SHOPPING_PRICE_SUCCESS,
          payload: {
            ui: response.data.payload
          }
        });
      })
      .catch(() => {
        dispatch({
          type: RECALCULATE_SHOPPING_PRICE_FAILURE
        });
      });
  };
};

export const sendData = (data) => {
  return (dispatch) => {
    dispatch({
      type: SEND_SHOPPING_DATA_FETCH
    });

    const invalidFields = Object.keys(data).filter(key => !data[key]);

    if (!data.paymentMethod.id) invalidFields.push('paymentMethod');

    if (data.paymentMethod.id === 3) {
      if (!data.paymentMethod.invoice) {
        invalidFields.push('invoice');
      }
    }

    if (invalidFields.length) {
      dispatch({
        type: ERROR_SHOPPING_VALIDATION,
        payload: {
          fields: invalidFields
        }
      });
      return;
    }

    axios.post(CHECKOUT.submitURL, { data })
      .then((response) => {
        dispatch({
          type: SEND_SHOPPING_DATA_SUCCESS,
          payload: {
            status: response.data.success,
            redirectURL: response.data.payload.redirectURL
          }
        });
      })
      .catch(() => {
        dispatch({
          type: SEND_SHOPPING_DATA_FAILURE
        });
      });
  };
};
