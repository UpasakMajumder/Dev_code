import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* constants */
import { FETCH, SUCCESS, FAILURE, SHOW, HIDE, START, FINISH, INIT_UI, SETTINGS_ADDRESSES, ADD_SHIPPING_ADDRESS,
  MODIFY_SHIPPING_ADDRESS, APP_LOADING, DIALOG, isDevelopment, SET_ADDRESS_DEFAULT, UNSET_ADDRESS_DEFAULT, SAVE_NEW_ADDRESS, ADD_NEW_ADDRESS } from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { USER_SETTINGS, NOTIFICATION, CHECKOUT } from 'app.globals';

export const getUI = () => {
  return (dispatch) => {
    dispatch({ type: SETTINGS_ADDRESSES + INIT_UI + FETCH });

    axios({
      method: 'get',
      url: USER_SETTINGS.addresses.initUIURL
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: SETTINGS_ADDRESSES + INIT_UI + FAILURE,
          alert: errorMessage
        });
      } else {
        dispatch({
          type: SETTINGS_ADDRESSES + INIT_UI + SUCCESS,
          payload: {
            ui: payload
          }
        });
      }
    })
      .catch((error) => {
        dispatch({ type: SETTINGS_ADDRESSES + INIT_UI + FAILURE });
      });
  };
};

export const modifyAddress = (data, fromCheckout) => {
  return (dispatch) => {
    dispatch({ type: MODIFY_SHIPPING_ADDRESS + FETCH });
    dispatch({ type: APP_LOADING + START });


    axios({
      method: 'post',
      url: USER_SETTINGS.addresses.editAddressURL,
      headers: { 'Content-Type': 'application/json' },
      data
    }).then((response) => {
      const { success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: MODIFY_SHIPPING_ADDRESS + FAILURE,
          alert: errorMessage
        });
        dispatch({ type: APP_LOADING + FINISH });
        return;
      }

      dispatch({
        type: MODIFY_SHIPPING_ADDRESS + SUCCESS,
        payload: data
      });

      dispatch({ type: APP_LOADING + FINISH });
      toastr.success(NOTIFICATION.modifyAddress.title, NOTIFICATION.modifyAddress.text);
    }).catch(() => {
      dispatch({ type: MODIFY_SHIPPING_ADDRESS + FAILURE });
      dispatch({ type: APP_LOADING + FINISH });
    });
  };
};


export const addAddress = (data, fromCheckout) => {
  return async (dispatch) => {
    dispatch({ type: ADD_SHIPPING_ADDRESS + FETCH });
    dispatch({ type: APP_LOADING + START });

    try {
      const url = data.temporary ? CHECKOUT.saveAddressURL : USER_SETTINGS.addresses.editAddressURL;

      const response = await axios({
        method: 'post',
        url,
        data
      });

      const { success, errorMessage, payload } = response.data;

      if (!success) {
        dispatch({
          type: ADD_SHIPPING_ADDRESS + FAILURE,
          alert: errorMessage
        });
        dispatch({ type: APP_LOADING + FINISH });
        return;
      }

      const { id } = payload;

      data.id = id;

      if (fromCheckout) {
        const responseCheckout = await axios.post(CHECKOUT.changeAddressURL, { id });
        const { success, payload } = responseCheckout.data;

        const checkedItems = payload.deliveryAddresses.items.filter(item => item.checked);

        if (success) {
          dispatch({
            type: SAVE_NEW_ADDRESS + SUCCESS,
            payload: {
              deliveryAddresses: payload.deliveryAddresses,
              checkedId: data.temporary ? id : checkedItems[0].id
            }
          });

          if (data.temporary) {
            dispatch({
              type: ADD_NEW_ADDRESS + SUCCESS,
              payload: data
            });
          }
        }
      }

      dispatch({
        type: ADD_SHIPPING_ADDRESS + SUCCESS,
        payload: data
      });

      dispatch({ type: APP_LOADING + FINISH });
      toastr.success(NOTIFICATION.addAddress.title, NOTIFICATION.addAddress.text);
    } catch (e) {
      dispatch({ type: ADD_SHIPPING_ADDRESS + FAILURE });
      dispatch({ type: APP_LOADING + FINISH });
    }
  };
};

export const setDefault = (type, id, url) => {
  return (dispatch) => {
    dispatch({
      type: SET_ADDRESS_DEFAULT + FETCH,
      payload: {
        type, id
      }
    });

    axios.put(url, { id }).then((response) => {
      const { success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: SET_ADDRESS_DEFAULT + FAILURE,
          alert: errorMessage
        });
      }
    }).catch((error) => {
      dispatch({ type: SET_ADDRESS_DEFAULT + FAILURE });
    });
  };
};

export const unsetDefault = (type, id, url) => {
  return (dispatch) => {
    dispatch({
      type: UNSET_ADDRESS_DEFAULT + FETCH,
      payload: {
        type, id
      }
    });

    axios.put(url, { id }).then((response) => {
      const { success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: UNSET_ADDRESS_DEFAULT + FAILURE,
          alert: errorMessage
        });
      }
    }).catch((error) => {
      dispatch({ type: UNSET_ADDRESS_DEFAULT + FAILURE });
    });
  };
};
