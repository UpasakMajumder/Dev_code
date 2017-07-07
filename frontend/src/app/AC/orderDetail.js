import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, ORDER_DETAIL } from 'app.consts';
/* globals */
import { ORDER_DETAIL as ORDER_DETAIL_URL } from 'app.globals';
/* web service */
import ui from 'app.ws/orderDetail';

export default (orderID) => {
  return (dispatch) => {
    dispatch({ type: ORDER_DETAIL + INIT_UI + FETCH });

    const prod = () => {
      axios({
        method: 'get',
        url: `${ORDER_DETAIL_URL.orderDetailUrl}/${orderID}`
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: ORDER_DETAIL + INIT_UI + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
        } else {
          dispatch({
            type: ORDER_DETAIL + SUCCESS,
            payload: {
              ui: payload
            }
          });
        }
      })
        .catch((error) => {
          dispatch({ type: ORDER_DETAIL + INIT_UI + FETCH });
          alert(error); // eslint-disable-line no-alert
        });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: ORDER_DETAIL + INIT_UI + SUCCESS,
          payload: { ui }
        });
      }, 2000);
    };

    // dev();
    prod();
  };
};
