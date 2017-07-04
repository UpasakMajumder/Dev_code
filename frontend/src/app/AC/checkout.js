import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, START, FINISH, APP_LOADING, CHECKOUT_ASK_PDF, CHECKOUT,
  CHANGE_CHECKOUT_DATA, INIT_CHECKED_CHECKOUT_DATA, RECALCULATE_CHECKOUT_PRICE, SUBMIT_CHECKOUT, REMOVE_PRODUCT,
  CHANGE_PRODUCT_QUANTITY } from 'app.consts';

/* globals */
import { CHECKOUT as CHECKOUT_URL } from 'app.globals';
/* web service */
import ui from 'app.ws/checkoutUI';

export const getUI = () => {
  return (dispatch) => {
    dispatch({ type: CHECKOUT + INIT_UI + FETCH });

    // setTimeout(() => {
    //   dispatch({
    //     type: CHECKOUT + INIT_UI + SUCCESS,
    //     payload: {
    //       ui: ui.payload,
    //       isWaitingPDF: ui.payload.submit.isDisabled
    //     }
    //   });
    // }, 3000);

    axios.get(CHECKOUT_URL.initUIURL)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: CHECKOUT + INIT_UI + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
          return;
        }

        dispatch({
          type: CHECKOUT + INIT_UI + SUCCESS,
          payload: {
            ui: payload,
            isWaitingPDF: payload.submit.isDisabled
          }
        });
      })
      .catch((error) => {
        alert(error); // eslint-disable-line no-alert
        dispatch({ type: CHECKOUT + INIT_UI + FAILURE });
      });
  };
};

export const initCheckedShoppingData = (data) => {
  return (dispatch) => {
    dispatch({
      type: INIT_CHECKED_CHECKOUT_DATA,
      payload: {
        ...data
      }
    });
  };
};

export const removeProduct = (id) => {
  return (dispatch) => {
    dispatch({
      type: APP_LOADING + START
    });

    const url = CHECKOUT_URL.removeProductURL;

    axios.post(url, { id })
      .then((response) => {
        dispatch({ type: APP_LOADING + FINISH });

        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: REMOVE_PRODUCT + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
          return;
        }

        dispatch({
          type: REMOVE_PRODUCT + SUCCESS,
          payload: {
            ui: payload,
            isWaitingPDF: payload.submit.isDisabled
          }
        });
      })
      .catch((error) => {
        alert(error); // eslint-disable-line no-alert
        dispatch({ type: REMOVE_PRODUCT + FAILURE });
        dispatch({ type: APP_LOADING + FINISH });
      });

    // setTimeout(() => {
    //   dispatch({ type: APP_LOADING + FINISH });
    // }, 2000);

  };
};

export const changeProductQuantity = (id, quantity) => {
  return (dispatch) => {
    dispatch({
      type: APP_LOADING + START
    });

    const url = CHECKOUT_URL.changeQuantityURL;

    axios.post(url, { id, quantity })
      .then((response) => {
        dispatch({ type: APP_LOADING + FINISH });

        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: CHANGE_PRODUCT_QUANTITY + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
          return;
        }

        dispatch({
          type: CHANGE_PRODUCT_QUANTITY + SUCCESS,
          payload: {
            ui: payload,
            isWaitingPDF: payload.submit.isDisabled
          }
        });
      })
      .catch((error) => {
        alert(error); // eslint-disable-line no-alert
        dispatch({ type: CHANGE_PRODUCT_QUANTITY + FAILURE });
        dispatch({ type: APP_LOADING + FINISH });
      });

    // setTimeout(() => {
    //   dispatch({ type: APP_LOADING + FINISH });
    // }, 2000);
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

    dispatch({ type: APP_LOADING + START });

    let url = '';
    if (field === 'deliveryMethod') {
      url = CHECKOUT_URL.changeDeliveryMethodURL;
    } else if (field === 'deliveryAddress') {
      url = CHECKOUT_URL.changeAddressURL;
    }

    // setTimeout(() => {
    //   dispatch({
    //     type: RECALCULATE_CHECKOUT_PRICE + SUCCESS,
    //     payload: {
    //       ui: ui2.payload
    //     }
    //   });
    //
    //   dispatch({ type: APP_LOADING + FINISH });
    // }, 1000);

    axios.post(url, { id })
      .then((response) => {
        dispatch({ type: APP_LOADING + FINISH });

        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
          return;
        }

        dispatch({
          type: RECALCULATE_CHECKOUT_PRICE + SUCCESS,
          payload: {
            ui: payload,
            isWaitingPDF: payload.submit.isDisabled
          }
        });
      })
      .catch((error) => {
        alert(error); // eslint-disable-line no-alert
        dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
        dispatch({ type: APP_LOADING + FINISH });
      });
  };
};

export const sendData = (data) => {
  return (dispatch) => {
    dispatch({ type: SUBMIT_CHECKOUT + FETCH });
    dispatch({ type: APP_LOADING + START });

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

    // setTimeout(() => {
    //   dispatch({ type: APP_LOADING + FINISH });
    // }, 2000);
  };
};

export const checkPDFAvailability = () => {
  return (dispatch) => {
    dispatch({ type: CHECKOUT_ASK_PDF + FETCH });

    setTimeout(() => {
      axios.get(CHECKOUT_URL.submittableURL)
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({ type: CHECKOUT_ASK_PDF + FAILURE });
            if (success !== undefined) {
              alert(errorMessage); // eslint-disable-line no-alert
            } else {
              alert('ERROR: Missing PDF'); // eslint-disable-line no-alert
            }
            return;
          }

          dispatch({
            type: CHECKOUT_ASK_PDF + SUCCESS,
            payload: {
              isWaitingPDF: !payload // true -> is bad for service and good for me
            }
          });
        })
        .catch((error) => {
          alert(error); // eslint-disable-line no-alert
          dispatch({ type: CHECKOUT_ASK_PDF + FAILURE });
        });

      // dispatch({
      //   type: CHECKOUT_ASK_PDF + SUCCESS,
      //   payload: {
      //     isWaitingPDF: false
      //   }
      // });
    }, 1500);
  };
};
