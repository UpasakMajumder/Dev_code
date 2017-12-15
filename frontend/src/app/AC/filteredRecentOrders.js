import axios from 'axios';

import {
  FILTERED_RECENT_ORDERS_GET_CAMPAIGNS,
  FILTERED_RECENT_ORDERS_GET_ORDERS,
  FETCH,
  FAILURE,
  SUCCESS
} from 'app.consts';

export const getCampaigns = (apiUrl, selectedOrderType) => {
  return (dispatch) => {
    dispatch({
      type: FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + FETCH,
      payload: {
        value: selectedOrderType
      }
    });

    axios.get(`${apiUrl}/${selectedOrderType}`)
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

export const getOrders = (apiUrl, selectedOrderType, selectedCampaign) => {
  return (dispatch) => {
    dispatch({
      type: FILTERED_RECENT_ORDERS_GET_ORDERS + FETCH,
      payload: {
        selectedOrderType,
        selectedCampaign
      }
    });

    const parametres = selectedCampaign ? `${selectedOrderType}/${selectedCampaign}` : selectedOrderType;

    axios.get(`${apiUrl}/${parametres}`)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          dispatch({
            type: FILTERED_RECENT_ORDERS_GET_ORDERS + SUCCESS,
            payload
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
