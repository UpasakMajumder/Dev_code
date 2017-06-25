import axios from 'axios';
import { GET_RECENT_ORDERS_HEADINGS_FETCH, GET_RECENT_ORDERS_ROWS_FETCH, GET_RECENT_ORDERS_HEADINGS_SUCCESS,
         GET_RECENT_ORDERS_ROWS_SUCCESS, GET_RECENT_ORDERS_HEADINGS_FAILURE, GET_RECENT_ORDERS_ROWS_FAILURE,
         APP_LOADING_START, APP_LOADING_FINISH } from '../constants';
import { RECENT_ORDERS } from '../globals';
// import { headings, pageInfo, rows1, rows2, noOrdersMessage } from '../testServices/recentOrders';

export const getHeadings = () => {
  return (dispatch) => {
    dispatch({ type: GET_RECENT_ORDERS_HEADINGS_FETCH });

    axios({
      method: 'get',
      url: RECENT_ORDERS.getHeaders
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      console.log(response);

      if (!success) {
        dispatch({ type: GET_RECENT_ORDERS_HEADINGS_FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
      } else {
        dispatch({
          type: GET_RECENT_ORDERS_HEADINGS_SUCCESS,
          payload: {
            headings: payload.headings,
            pageInfo: payload.pageInfo,
            noOrdersMessage: payload.noOrdersMessage
          }
        });
      }
    }).catch((error) => {
      dispatch({ type: GET_RECENT_ORDERS_HEADINGS_FAILURE });
      alert(error); // eslint-disable-line no-alert
    });


    // setTimeout(() => {
    //   dispatch({
    //     type: GET_RECENT_ORDERS_HEADINGS_SUCCESS,
    //     payload: {
    //       headings: headings.headings,
    //       pageInfo: pageInfo.pageInfo,
    //       noOrdersMessage
    //     }
    //   });
    // }, 2000);
  };
};

export const getRows = (page) => {
  return (dispatch) => {
    dispatch({ type: GET_RECENT_ORDERS_ROWS_FETCH });
    dispatch({ type: APP_LOADING_START });

    axios({
      method: 'get',
      url: `${RECENT_ORDERS.getPageItems}/${page}`
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      console.log(response);

      if (!success) {
        dispatch({ type: GET_RECENT_ORDERS_ROWS_FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
        dispatch({ type: APP_LOADING_FINISH });
      } else {
        dispatch({
          type: GET_RECENT_ORDERS_ROWS_SUCCESS,
          rows: {
            [page - 1]: payload.rows
          }
        });
        dispatch({ type: APP_LOADING_FINISH });
      }
    }).catch((error) => {
      dispatch({ type: GET_RECENT_ORDERS_ROWS_FAILURE });
      alert(error); // eslint-disable-line no-alert
      dispatch({ type: APP_LOADING_FINISH });
    });

    // setTimeout(() => {
    //   if (page % 2 === 0) {
    //     dispatch({
    //       type: GET_RECENT_ORDERS_ROWS_SUCCESS,
    //       payload: {
    //         rows: {
    //           [page - 1]: rows1.payload.rows
    //         }
    //       }
    //     });
    //   } else {
    //     dispatch({
    //       type: GET_RECENT_ORDERS_ROWS_SUCCESS,
    //       payload: {
    //         rows: {
    //           [page - 1]: rows2.payload.rows
    //         }
    //       }
    //     });
    //   }
    //
    //   dispatch({ type: APP_LOADING_FINISH });
    // }, 2500);
  };
};
