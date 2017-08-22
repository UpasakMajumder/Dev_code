import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, START, FINISH, APP_LOADING, CHECKOUT_PRICING,
  CHANGE_CHECKOUT_DATA, INIT_CHECKED_CHECKOUT_DATA, RECALCULATE_CHECKOUT_PRICE, SUBMIT_CHECKOUT, REMOVE_PRODUCT,
  CHANGE_PRODUCT_QUANTITY, CHECKOUT_STATIC, CART_PREVIEW_CHANGE_ITEMS } from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { CHECKOUT as CHECKOUT_URL, NOTIFICATION } from 'app.globals';
/* web service */
import { staticUI, staticUI2, priceUI, completeUI } from 'app.ws/checkoutUI';
import { newState } from 'app.ws/cartPreviewUI';

const getTotalPrice = (dispatch) => {
  axios.get(CHECKOUT_URL.initTotalDeliveryUIURL)
    .then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({ type: CHECKOUT_PRICING + INIT_UI + FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
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
      alert(error); // eslint-disable-line no-alert
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

    const prod = () => {
      axios.get(CHECKOUT_URL.initRenderUIURL)
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({ type: CHECKOUT_STATIC + INIT_UI + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
          dispatch({ type: CHECKOUT_STATIC + INIT_UI + FAILURE });
        });
    };

    const dev = () => {
      setTimeout(() => {
        getTotalPriceDev(dispatch);

        dispatch({
          type: CHECKOUT_STATIC + INIT_UI + SUCCESS,
          payload: {
            ui: staticUI.payload
          }
        });
      }, 1500);
    };

    callAC(dev, prod);
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
            dispatch({ type: REMOVE_PRODUCT + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
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
            dispatch({ type: CHANGE_PRODUCT_QUANTITY + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
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

export const changeShoppingData = (field, id, invoice) => {
  return (dispatch) => {
    dispatch({
      type: CHANGE_CHECKOUT_DATA,
      payload: {
        field, id, invoice
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
            dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
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
      axios.post(CHECKOUT_URL.submitURL, { ...data })
        .then((response) => {
          dispatch({ type: APP_LOADING + FINISH });

          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({ type: SUBMIT_CHECKOUT + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
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
