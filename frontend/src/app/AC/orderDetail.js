import axios from 'axios';
import { ORDER_DETAIL_GET_UI_FETCH, ORDER_DETAIL_GET_UI_FAILURE, ORDER_DETAIL_GET_UI_SUCCESS } from '../constants';
import { ORDER_DETAIL } from '../globals';
// import ui from '../testServices/orderDetail';

export default () => {
  return (dispatch) => {
    dispatch({ type: ORDER_DETAIL_GET_UI_FETCH });

    axios({
      method: 'get',
      url: ORDER_DETAIL.orderDetailUrl
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({ type: ORDER_DETAIL_GET_UI_FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
      } else {
        dispatch({
          type: ORDER_DETAIL_GET_UI_SUCCESS,
          payload: {
            ui: payload
          }
        });
      }
    })
    .catch((error) => {
      dispatch({ type: ORDER_DETAIL_GET_UI_FAILURE });
      alert(error.message); // eslint-disable-line no-alert
    });

    // setTimeout(() => {
    //   dispatch({
    //     type: ORDER_DETAIL_GET_UI_SUCCESS,
    //     payload: { ui }
    //   });
    // }, 2000);
  };
};
