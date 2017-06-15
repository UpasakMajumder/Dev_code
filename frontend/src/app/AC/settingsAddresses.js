import axios from 'axios';
import { SETTINGS_ADDRESSES_UI_FAILURE, SETTINGS_ADDRESSES_UI_FETCH, SETTINGS_ADDRESSES_UI_SUCCESS,
  MODIFY_SHIPPING_ADDRESS_FETCH, MODIFY_SHIPPING_ADDRESS_SUCCESS, MODIFY_SHIPPING_ADDRESS_FAILURE, APP_LOADING_START,
  APP_LOADING_FINISH, DIALOG_CLOSE } from '../constants';
import { USER_SETTINGS } from '../globals';
// import ui from '../testServices/settingsUI';

const getUITest = (dispatch) => {
  dispatch({ type: SETTINGS_ADDRESSES_UI_FETCH });

  axios({
    method: 'get',
    url: USER_SETTINGS.addresses.initUIURL
  }).then((response) => {
    const { payload, success, errorMessage } = response.data;

    if (!success) {
      dispatch({ type: SETTINGS_ADDRESSES_UI_FAILURE });
      alert(errorMessage); // eslint-disable-line no-alert
    } else {
      dispatch({
        type: SETTINGS_ADDRESSES_UI_SUCCESS,
        payload: {
          ui: payload
        }
      });
    }
  })
  .catch(() => {
    dispatch({ type: SETTINGS_ADDRESSES_UI_FAILURE });
  });

  // setTimeout(() => {
  //   dispatch({
  //     type: SETTINGS_ADDRESSES_UI_SUCCESS,
  //     payload: { ui }
  //   })
  // }, 2000);
};

export const getUI = () => dispatch => getUITest(dispatch);

export const modifyAddress = (data) => {
  return (dispatch) => {
    dispatch({ type: MODIFY_SHIPPING_ADDRESS_FETCH });
    dispatch({ type: APP_LOADING_START });

    axios({
      method: 'post',
      url: USER_SETTINGS.addresses.editAddressURL,
      data
    }).then((response) => {
      const { success, errorMessage, payload } = response.data;

      if (!success) {
        dispatch({ type: MODIFY_SHIPPING_ADDRESS_FAILURE });
		    dispatch({ type: APP_LOADING_FINISH });
        alert(errorMessage); // eslint-disable-line no-alert
        return;
      }

      const { id } = payload;
      data.id = id;

      dispatch({
        type: MODIFY_SHIPPING_ADDRESS_SUCCESS,
        payload: { data }
      });

      dispatch({ type: DIALOG_CLOSE });
      dispatch({ type: APP_LOADING_FINISH });
    })
    .catch(() => {
      dispatch({ type: MODIFY_SHIPPING_ADDRESS_FAILURE });
    });

    // setTimeout(() => {
    //   dispatch({
    //     type: MODIFY_SHIPPING_ADDRESS_SUCCESS,
    //     payload: { data }
    //   });
    //
    //   dispatch({ type: DIALOG_CLOSE });
    //
    //   if (id !== -1) {
    //     dispatch({ type: APP_LOADING_FINISH });
    //     return;
    //   }
    //
    //   dispatch({ type: SETTINGS_ADDRESSES_UI_FETCH });
    //
    //   setTimeout(() => {
    //     dispatch({
    //       type: SETTINGS_ADDRESSES_UI_SUCCESS,
    //       payload: { ui }
    //     });
    //     dispatch({ type: APP_LOADING_FINISH });
    //
    //   }, 2000);
    //
    // }, 2000);
  };
};
