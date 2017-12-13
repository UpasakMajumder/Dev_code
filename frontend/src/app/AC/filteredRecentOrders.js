import axios from 'axios';

import {
  FILTERED_RECENT_ORDERS_GET_CAMPAIGNS,
  FILTERED_RECENT_ORDERS_GET_ORDERS,
  FETCH,
  FAILURE,
  SUCCESS
} from 'app.consts';

export const getCampaigns = (url, selectedOrderType) => {
  return (dispatch) => {
    dispatch({
      type: FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + FETCH,
      payload: {
        selected: selectedOrderType
      }
    });

    axios.get(`${url}/${selectedOrderType}`)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          dispatch({
            type: FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + SUCCESS,
            payload: {
              placeholder: payload.placeholder,
              items: payload.items
            }
          });
        } else {
          dispatch({
            type: FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch(() => {
        dispatch({ type: FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + FAILURE });
      });
  };
};

export const getOrders = (url, selectedOrderType, selectedCampaign) => {
  return (dispatch) => {
    dispatch({
      type: FILTERED_RECENT_ORDERS_GET_ORDERS + FETCH,
      payload: {
        selected: selectedCampaign
      }
    });

    axios.get(`${url}/${selectedOrderType}/${selectedCampaign}`)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          dispatch({
            type: FILTERED_RECENT_ORDERS_GET_ORDERS + SUCCESS,
            payload: {
              orders: payload.orders
            }
          });
        } else {
          dispatch({
            type: FILTERED_RECENT_ORDERS_GET_ORDERS + FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch(() => {
        dispatch({ type: FILTERED_RECENT_ORDERS_GET_ORDERS + FAILURE });
      });
  };
};
