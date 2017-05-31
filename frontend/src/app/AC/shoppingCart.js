import ui from './ui';
import axios from 'axios';
import { SHOPPING_CART_UI_FETCH, SHOPPING_CART_UI_SUCCESS, SHOPPING_CART_UI_FAILURE, CHANGE_SHOPPING_DATA,
  INIT_CHECKED_SHOPPING_DATA, RECALCULATE_SHIPPING_PRICE_FETCH, RECALCULATE_SHIPPING_PRICE_SUCCESS,
  RECALCULATE_SHIPPING_PRICE_FAILURE, SEND_SHIPPING_DATA_FETCH, SEND_SHIPPING_DATA_FAILURE,
  SEND_SHIPPING_DATA_SUCCESS, ERROR_SHIPPING_VALIDATION } from '../constants';

export const getUI = () => {
  return (dispatch) => {
    dispatch({
      type: SHOPPING_CART_UI_FETCH
    });

    axios.get('/user?ID=12345') // TODO: GLOBAL
      .then((response) => {
        dispatch({
          type: SHOPPING_CART_UI_SUCCESS,
          payload: {
            ui: response.data
          }
        });
      })
      .catch(() => {
        dispatch({
          type: SHOPPING_CART_UI_FAILURE
        });
      });

    dispatch({
      type: SHOPPING_CART_UI_SUCCESS,
      payload: {
        ui
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
      type: RECALCULATE_SHIPPING_PRICE_FETCH
    });

    // selectaddress
    // selectshipping

    // axios.get('/user?ID=12345', { field, id }) // TODO: GLOBAL
    //   .then((response) => {
    //     dispatch({
    //       type: RECALCULATE_SHIPPING_PRICE_SUCCESS,
    //       payload: {
    //         ui: response.data
    //       }
    //     });
    //   })
    //   .catch(() => {
    //     dispatch({
    //       type: RECALCULATE_SHIPPING_PRICE_FAILURE
    //     });
    //   });
  };
};

export const initCheckedShoppingData = () => {
  return (dispatch) => {
    dispatch({
      type: INIT_CHECKED_SHOPPING_DATA
    });
  };
};

export const sendData = (data) => {
  return (dispatch) => {
    dispatch({
      type: SEND_SHIPPING_DATA_FETCH
    });
    //
    // const invalidField = Object.keys(data).filter(key => !data[key])[0];
    //
    // if (invalidField) {
    //   dispatch({
    //     type: ERROR_SHIPPING_VALIDATION,
    //     payload: {
    //       field: invalidField
    //     }
    //   });
    //   return;
    // }

    if (data.paymentMethod.id === 3) {
      if (!data.paymentMethod.invoice) {
        dispatch({
          type: ERROR_SHIPPING_VALIDATION,
          payload: {
            field: 'invoice'
          }
        });
        return;
      }
    }

    // axios.get('/user?ID=12345', { data }) // TODO: GLOBAL
    //   .then((response) => {
    //     dispatch({
    //       type: SEND_SHIPPING_DATA_SUCCESS,
    //       payload: {
    //         status: response.status,
    //         redirectURL: response.redirectUrl
    //       }
    //     });
    //   })
    //   .catch(() => {
    //     dispatch({
    //       type: SEND_SHIPPING_DATA_FAILURE
    //     });
    //   });
  };
};
