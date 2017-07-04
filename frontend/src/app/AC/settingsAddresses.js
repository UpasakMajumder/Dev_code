import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, SHOW, HIDE, START, FINISH, INIT_UI, SETTINGS_ADDRESSES, MODIFY_SHIPPING_ADDRESS,
  APP_LOADING, DIALOG } from 'app.consts';

/* globals */
import { USER_SETTINGS } from 'app.globals';
/* web service */
import ui from 'app.ws/settingsUI';

const getUITest = (dispatch) => {
  dispatch({ type: SETTINGS_ADDRESSES + INIT_UI + FETCH });

  axios({
    method: 'get',
    url: USER_SETTINGS.addresses.initUIURL
  }).then((response) => {
    const { payload, success, errorMessage } = response.data;

    if (!success) {
      dispatch({ type: SETTINGS_ADDRESSES + INIT_UI + FAILURE });
      alert(errorMessage); // eslint-disable-line no-alert
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
    alert(error); // eslint-disable-line no-alert
  });

  // setTimeout(() => {
  //   dispatch({
  //     type: SETTINGS_ADDRESSES + INIT_UI + SUCCESS,
  //     payload: { ui }
  //   });
  // }, 2000);
};

export const getUI = () => dispatch => getUITest(dispatch);

export const modifyAddress = (data) => {
  return (dispatch) => {
    dispatch({ type: MODIFY_SHIPPING_ADDRESS + FETCH });
    dispatch({ type: APP_LOADING + START });

    axios({
      method: 'post',
      url: USER_SETTINGS.addresses.editAddressURL,
      data
    }).then((response) => {
      const { success, errorMessage, payload } = response.data;

      if (!success) {
        dispatch({ type: MODIFY_SHIPPING_ADDRESS + FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
        dispatch({ type: APP_LOADING + FINISH });
        return;
      }

      const { id } = payload;
      data.id = id;

      dispatch({
        type: MODIFY_SHIPPING_ADDRESS + SUCCESS,
        payload: { data }
      });

      dispatch({ type: DIALOG + HIDE });
      dispatch({ type: APP_LOADING + FINISH });
    })
    .catch((error) => {
      dispatch({ type: MODIFY_SHIPPING_ADDRESS + FAILURE });
      alert(error); // eslint-disable-line no-alert
      dispatch({ type: APP_LOADING + FINISH });
    });

    // setTimeout(() => {
    //   dispatch({
    //     type: MODIFY_SHIPPING_ADDRESS + SUCCESS,
    //     payload: { data }
    //   });
    //
    //   dispatch({ type: DIALOG + HIDE });
    //
    //   if (id !== -1) {
    //     dispatch({ type: APP_LOADING + FINISH });
    //     return;
    //   }
    //
    //   dispatch({ type: MODIFY_SHIPPING_ADDRESS + FETCH });
    //
    //   setTimeout(() => {
    //     dispatch({
    //       type: MODIFY_SHIPPING_ADDRESS + SUCCESS,
    //       payload: { ui }
    //     });
    //     dispatch({ type: APP_LOADING + FINISH });
    //
    //   }, 2000);
    //
    // }, 2000);
  };
};
