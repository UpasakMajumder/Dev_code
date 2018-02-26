import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* constants */
import {
  FETCH,
  SUCCESS,
  FAILURE,
  INIT_UI,
  START,
  FINISH,
  APP_LOADING,
  CHECKOUT,
  CHECKOUT_INIT_CHECKED_DATA,
  CHECKOUT_CHANGE_PAYMENT,
  CHECKOUT_CHANGE_QUANTITY,
  CHECKOUT_REMOVE_PRODUCT,
  CHECKOUT_CHANGE_ADDRESS,
  CHECKOUT_GET_TOTALS,
  CHECKOUT_PROCEED,
  CHECKOUT_CHANGE_DELIVERY,
  ADD_NEW_ADDRESS
} from 'app.consts';

/* globals */
import { CHECKOUT as CHECKOUT_URL, NOTIFICATION } from 'app.globals';

const getTotalPrice = (dispatch) => {
  dispatch({ type: CHECKOUT_GET_TOTALS + FETCH });

  const state = window.store.getState();
  let promise;

  if (state.checkout.checkedData.deliveryAddress === -1) {
    // for new custom address we have to pass newAddress data
    promise = axios.post(CHECKOUT_URL.initTotalDeliveryUIURL, state.checkout.newAddress);
  } else {
    promise = axios.get(CHECKOUT_URL.initTotalDeliveryUIURL);
  }

  promise
    .then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: CHECKOUT_GET_TOTALS + FAILURE,
          alert: errorMessage
        });

        return;
      }

      dispatch({
        type: CHECKOUT_GET_TOTALS + SUCCESS,
        payload
      });
    })
    .catch((error) => {
      dispatch({ type: CHECKOUT_GET_TOTALS + FAILURE });
    });
};

export const getUI = () => {
  return (dispatch) => {
    dispatch({ type: CHECKOUT + INIT_UI + FETCH });

    axios.get(CHECKOUT_URL.initRenderUIURL)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT + INIT_UI + FAILURE,
            alert: errorMessage
          });
          return;
        }

        getTotalPrice(dispatch);

        dispatch({
          type: CHECKOUT + INIT_UI + SUCCESS,
          payload
        });
      })
      .catch((error) => {
        dispatch({ type: CHECKOUT + INIT_UI + FAILURE });
      });
  };
};

export const initCheckedShoppingData = (data) => {
  return {
    type: CHECKOUT_INIT_CHECKED_DATA,
    payload: data
  };
};


export const removeProduct = (id) => {
  return (dispatch) => {
    dispatch({ type: CHECKOUT_REMOVE_PRODUCT + FETCH });

    const url = CHECKOUT_URL.removeProductURL;
    axios.post(url, { id })
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT_REMOVE_PRODUCT + FAILURE,
            alert: errorMessage
          });
          return;
        }

        getTotalPrice(dispatch);

        dispatch({
          type: CHECKOUT_REMOVE_PRODUCT + SUCCESS,
          payload
        });

        toastr.success(NOTIFICATION.removeProduct.title, NOTIFICATION.removeProduct.text);
      })
      .catch((error) => {
        dispatch({ type: CHECKOUT_REMOVE_PRODUCT + FAILURE });
      });
  };
};

export const changeProductQuantity = (id, quantity) => {
  return (dispatch) => {
    dispatch({ type: CHECKOUT_CHANGE_QUANTITY + FETCH });

    const url = CHECKOUT_URL.changeQuantityURL;
    axios.post(url, { id, quantity })
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT_CHANGE_QUANTITY + FAILURE,
            alert: errorMessage
          });
          return;
        }

        getTotalPrice(dispatch);

        dispatch({
          type: CHECKOUT_CHANGE_QUANTITY + SUCCESS,
          payload
        });
      })
      .catch((error) => {
        dispatch({ type: CHECKOUT_CHANGE_QUANTITY + FAILURE });
      });
  };
};

export const changeDeliveryMethod = (id) => {
  return (dispatch) => {
    const url = CHECKOUT_URL.changeDeliveryMethodURL;

    dispatch({ type: CHECKOUT_CHANGE_DELIVERY + FETCH });

    axios.post(url, { id })
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT_CHANGE_DELIVERY + FAILURE,
            alert: errorMessage
          });
          return;
        }

        dispatch({
          type: CHECKOUT_CHANGE_DELIVERY + SUCCESS,
          payload: {
            ...payload,
            id
          }
        });
      })
      .catch((error) => {
        dispatch({ type: CHECKOUT_CHANGE_DELIVERY + FAILURE });
      });
  };
};

export const changeDeliveryAddress = (id) => {
  return (dispatch) => {
    const url = CHECKOUT_URL.changeAddressURL;

    dispatch({ type: CHECKOUT_CHANGE_ADDRESS + FETCH });

    axios.post(url, { id })
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT_CHANGE_ADDRESS + FAILURE,
            alert: errorMessage
          });
          return;
        }

        getTotalPrice(dispatch);

        dispatch({
          type: CHECKOUT_CHANGE_ADDRESS + SUCCESS,
          payload: {
            deliveryAddresses: payload.deliveryAddresses,
            id
          }
        });
      })
      .catch((error) => {
        dispatch({ type: CHECKOUT_CHANGE_ADDRESS + FAILURE });
      });
  };
};

export const changePaymentMethod = (id, invoice, card) => {
  return {
    type: CHECKOUT_CHANGE_PAYMENT,
    payload: {
      id,
      invoice,
      card
    }
  };
};

export const sendData = (data) => {
  return (dispatch) => {
    dispatch({ type: CHECKOUT_PROCEED + FETCH });
    dispatch({ type: APP_LOADING + START });

    axios.post(CHECKOUT_URL.submitURL, data)
      .then((response) => {
        dispatch({ type: APP_LOADING + FINISH });

        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT_PROCEED + FAILURE,
            alert: errorMessage
          });
          return;
        }

        dispatch({
          type: CHECKOUT_PROCEED + SUCCESS,
          payload: {
            status: success,
            redirectURL: payload.redirectURL
          }
        });
      })
      .catch((error) => {
        dispatch({ type: APP_LOADING + FINISH });
        dispatch({ type: CHECKOUT_PROCEED + FAILURE });
      });
  };
};

export const addNewAddress = (data) => {
  return (dispatch) => {
    dispatch({ type: ADD_NEW_ADDRESS + FETCH });
    dispatch({ type: APP_LOADING + START });

    dispatch({
      type: ADD_NEW_ADDRESS + SUCCESS,
      payload: data
    });

    axios.post(CHECKOUT_URL.saveAddressURL, data)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT_GET_TOTALS + INIT_UI + FAILURE,
            alert: errorMessage
          });
          dispatch({ type: APP_LOADING + FINISH });
          return;
        }

        dispatch({
          type: CHECKOUT_GET_TOTALS + INIT_UI + SUCCESS,
          payload
        });
        dispatch({ type: APP_LOADING + FINISH });
      })
      .catch((error) => {
        dispatch({ type: CHECKOUT_GET_TOTALS + INIT_UI + FAILURE });
        dispatch({ type: APP_LOADING + FINISH });
      });
  };
};
