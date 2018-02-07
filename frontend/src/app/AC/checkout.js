import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, START, FINISH, APP_LOADING, CHECKOUT_PRICING,
  CHANGE_CHECKOUT_DATA, INIT_CHECKED_CHECKOUT_DATA, RECALCULATE_CHECKOUT_PRICE, SUBMIT_CHECKOUT, REMOVE_PRODUCT,
  CHANGE_PRODUCT_QUANTITY, CHECKOUT_STATIC, CART_PREVIEW_CHANGE_ITEMS, ADD_NEW_ADDRESS } from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { CHECKOUT as CHECKOUT_URL, NOTIFICATION } from 'app.globals';
/* web service */
import { staticUI, staticUI2, priceUI, completeUI, newAddress } from 'app.ws/checkoutUI';
import { newState } from 'app.ws/cartPreviewUI';

const getTotalPrice = (dispatch) => {
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
          type: CHECKOUT_PRICING + INIT_UI + FAILURE,
          alert: errorMessage
        });
        return;
      }

      dispatch({
        type: CHECKOUT_PRICING + INIT_UI + SUCCESS,
        payload: {
          ui: payload
        }
      });
    })
    .catch((error) => {
      dispatch({ type: CHECKOUT_PRICING + INIT_UI + FAILURE });
    });
};

const getTotalPriceDev = (dispatch) => {
  setTimeout(() => {
    dispatch({
      type: CHECKOUT_PRICING + INIT_UI + SUCCESS,
      payload: {
        ui: priceUI.payload
      }
    });
  }, 3000);
};

export const getUI = () => {
  return (dispatch) => {
    dispatch({ type: CHECKOUT_STATIC + INIT_UI + FETCH });

    axios.get(CHECKOUT_URL.initRenderUIURL)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: CHECKOUT_STATIC + INIT_UI + FAILURE,
            alert: errorMessage
          });
          return;
        }

        getTotalPrice(dispatch);

        dispatch({
          type: CHECKOUT_STATIC + INIT_UI + SUCCESS,
          payload: {
            ui: payload
          }
        });
      })
      .catch((error) => {
        dispatch({ type: CHECKOUT_STATIC + INIT_UI + FAILURE });
      });
  };
};

export const initCheckedShoppingData = (data) => {
  return (dispatch) => {
    dispatch({
      type: INIT_CHECKED_CHECKOUT_DATA,
      payload: { ...data }
    });
  };
};


export const removeProduct = (id) => {
  return (dispatch) => {
    const dev = () => {
      dispatch({
        type: REMOVE_PRODUCT + SUCCESS,
        payload: {
          ui: staticUI2.payload
        }
      });

      getTotalPriceDev(dispatch);

      toastr.success(NOTIFICATION.removeProduct.title, NOTIFICATION.removeProduct.text);
    };

    const prod = () => {
      const url = CHECKOUT_URL.removeProductURL;
      axios.post(url, { id })
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({
              type: REMOVE_PRODUCT + FAILURE,
              alert: errorMessage
            });
            return;
          }

          getTotalPrice(dispatch);

          dispatch({
            type: REMOVE_PRODUCT + SUCCESS,
            payload: {
              ui: payload
            }
          });

          toastr.success(NOTIFICATION.removeProduct.title, NOTIFICATION.removeProduct.text);
        })
        .catch((error) => {
          dispatch({ type: REMOVE_PRODUCT + FAILURE });
        });
    };

    callAC(dev, prod);
  };
};

export const changeProductQuantity = (id, quantity) => {
  return (dispatch) => {
    const prod = () => {
      const url = CHECKOUT_URL.changeQuantityURL;
      axios.post(url, { id, quantity })
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({
              type: CHANGE_PRODUCT_QUANTITY + FAILURE,
              alert: errorMessage
            });
            return;
          }

          getTotalPrice(dispatch);

          dispatch({
            type: CHANGE_PRODUCT_QUANTITY + SUCCESS,
            payload: {
              ui: payload
            }
          });
        })
        .catch((error) => {
          dispatch({ type: CHANGE_PRODUCT_QUANTITY + FAILURE });
        });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({ type: APP_LOADING + FINISH });
      }, 2000);
    };

    callAC(dev, prod);
  };
};

export const changeShoppingData = (field, id, invoice, card) => {
  return (dispatch) => {
    dispatch({
      type: CHANGE_CHECKOUT_DATA,
      payload: {
        field, id, invoice, card
      }
    });

    if (field === 'paymentMethod') return;

    dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FETCH });

    let url = '';
    if (field === 'deliveryMethod') {
      url = CHECKOUT_URL.changeDeliveryMethodURL;
    } else if (field === 'deliveryAddress') {
      url = CHECKOUT_URL.changeAddressURL;
    }

    const prod = () => {
      axios.post(url, { id })
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({
              type: RECALCULATE_CHECKOUT_PRICE + FAILURE,
              alert: errorMessage
            });
            return;
          }

          getTotalPrice(dispatch);

          dispatch({
            type: RECALCULATE_CHECKOUT_PRICE + SUCCESS,
            payload: {
              ui: payload
            }
          });
        })
        .catch((error) => {
          dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
        });
    };

    const dev = () => {
      setTimeout(() => {
        getTotalPriceDev(dispatch);

        dispatch({
          type: RECALCULATE_CHECKOUT_PRICE + SUCCESS,
          payload: {
            ui: completeUI.payload
          }
        });
      }, 1000);
    };

    callAC(dev, prod);
  };
};

export const sendData = (data) => {
  return (dispatch) => {
    dispatch({ type: SUBMIT_CHECKOUT + FETCH });
    dispatch({ type: APP_LOADING + START });

    const prod = () => {
      axios.post(CHECKOUT_URL.submitURL, data)
        .then((response) => {
          dispatch({ type: APP_LOADING + FINISH });

          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({
              type: SUBMIT_CHECKOUT + FAILURE,
              alert: errorMessage
            });
            return;
          }

          dispatch({
            type: SUBMIT_CHECKOUT + SUCCESS,
            payload: {
              status: success,
              redirectURL: payload.redirectURL
            }
          });
        })
        .catch((error) => {
          dispatch({ type: APP_LOADING + FINISH });
          dispatch({ type: SUBMIT_CHECKOUT + FAILURE });
        });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({ type: APP_LOADING + FINISH });
      }, 2000);
    };

    callAC(dev, prod);
  };
};

export const addNewAddress = (data) => {
  return (dispatch) => {
    dispatch({ type: ADD_NEW_ADDRESS + FETCH });
    dispatch({ type: APP_LOADING + START });

    const dev = () => {
      dispatch({
        type: ADD_NEW_ADDRESS + SUCCESS,
        payload: data
      });

      setTimeout(() => {
        dispatch({
          type: CHECKOUT_PRICING + INIT_UI + SUCCESS,
          payload: {
            ui: priceUI.payload
          }
        });
        dispatch({ type: APP_LOADING + FINISH });
      }, 3000);
    };

    const prod = () => {
      dispatch({
        type: ADD_NEW_ADDRESS + SUCCESS,
        payload: data
      });

      axios.post(CHECKOUT_URL.initTotalDeliveryUIURL, data)
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({
              type: CHECKOUT_PRICING + INIT_UI + FAILURE,
              alert: errorMessage
            });
            dispatch({ type: APP_LOADING + FINISH });
            return;
          }

          dispatch({
            type: CHECKOUT_PRICING + INIT_UI + SUCCESS,
            payload: {
              ui: payload
            }
          });
          dispatch({ type: APP_LOADING + FINISH });
        })
        .catch((error) => {
          dispatch({ type: CHECKOUT_PRICING + INIT_UI + FAILURE });
          dispatch({ type: APP_LOADING + FINISH });
        });
    };

    callAC(dev, prod);
  };
};
