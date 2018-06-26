import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, ORDER_DETAIL, CHANGE_STATUS, EDIT_ORDERS } from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { ORDER_DETAIL as ORDER_DETAIL_URL } from 'app.globals';

export const getUI = (orderID = '') => {
  return (dispatch) => {
    dispatch({ type: ORDER_DETAIL + INIT_UI + FETCH });

    axios({
      method: 'get',
      url: `${ORDER_DETAIL_URL.orderDetailUrl}/${orderID}`
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: ORDER_DETAIL + INIT_UI + FAILURE,
          alert: errorMessage
        });
      } else {
        dispatch({
          type: ORDER_DETAIL + INIT_UI + SUCCESS,
          payload: {
            ui: payload
          }
        });
      }
    })
      .catch((error) => {
        dispatch({ type: ORDER_DETAIL + INIT_UI + FAILURE });
      });
  };
};

export const changeStatus = ({ status, note }) => {
  return {
    type: ORDER_DETAIL + CHANGE_STATUS,
    payload: {
      status,
      note
    }
  };
};

export const editOrders = ({
  pricingInfo,
  orderedItems,
  ordersPrice
}) => {
  return {
    type: ORDER_DETAIL + EDIT_ORDERS,
    payload: {
      pricingInfo,
      orderedItems,
      ordersPrice
    }
  };
};
