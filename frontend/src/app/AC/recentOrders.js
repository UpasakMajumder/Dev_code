import axios from 'axios';
import { GET_RECENT_ORDERS_HEADINGS_FETCH, GET_RECENT_ORDERS_ROWS_FETCH, GET_RECENT_ORDERS_HEADINGS_SUCCESS,
         GET_RECENT_ORDERS_ROWS_SUCCESS, GET_RECENT_ORDERS_HEADINGS_FAILURE, GET_RECENT_ORDERS_ROWS_FAILURE } from '../constants';
import { RECENT_ORDERS } from '../globals';
import { headings, pagination, rows } from '../testServices/recentOrders';

export const getHeadings = () => {
  return (dispatch) => {
    dispatch({ type: GET_RECENT_ORDERS_HEADINGS_FETCH });

    axios({
      method: 'get',
      url: RECENT_ORDERS.getHeaders
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({ type: GET_RECENT_ORDERS_HEADINGS_FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
      } else {
        dispatch({
          type: GET_RECENT_ORDERS_HEADINGS_SUCCESS,
          payload: {
            headings: payload.headings,
            pagination: payload.pagination
          }
        });
      }
    }).catch((error) => {
      dispatch({ type: GET_RECENT_ORDERS_HEADINGS_FAILURE });
      alert(error.message); // eslint-disable-line no-alert
    });


    // setTimeout(() => {
    //   dispatch({
    //     type: GET_RECENT_ORDERS_HEADINGS_SUCCESS,
    //     payload: {
    //       headings: headings.headings,
    //       pagination: pagination.pagination
    //     }
    //   });
    // }, 2000);
  };
};

export const getRows = (page) => {
  return (dispatch) => {
    dispatch({ type: GET_RECENT_ORDERS_ROWS_FETCH });

    axios({
      method: 'get',
      url: `${RECENT_ORDERS.getPageItems}/${page}`
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({ type: GET_RECENT_ORDERS_ROWS_FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
      } else {
        dispatch({
          type: GET_RECENT_ORDERS_ROWS_SUCCESS,
          rows: {
            [page - 1]: payload.rows
          }
        });
      }
    }).catch((error) => {
      dispatch({ type: GET_RECENT_ORDERS_ROWS_FAILURE });
      alert(error.message); // eslint-disable-line no-alert
    });

    // setTimeout(() => {
    //   dispatch({
    //     type: GET_RECENT_ORDERS_ROWS_SUCCESS,
    //     payload: {
    //       rows: {
    //         [page - 1]: rows.rows
    //       }
    //     }
    //   });
    // }, 2500);
  };
};
