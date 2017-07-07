import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, START, FINISH, RECENT_ORDERS_ROWS, RECENT_ORDERS_HEADINGS, APP_LOADING } from 'app.consts';
/* globals */
import { RECENT_ORDERS } from 'app.globals';
/* web service */
import { headings, pageInfo, rows1, rows2, noOrdersMessage } from 'app.ws/recentOrders';

export const getHeadings = () => {
  return (dispatch) => {
    dispatch({ type: RECENT_ORDERS_HEADINGS + FETCH });

    const prod = () => {
      axios({
        method: 'get',
        url: RECENT_ORDERS.getHeaders
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: RECENT_ORDERS_HEADINGS + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
        } else {
          dispatch({
            type: RECENT_ORDERS_HEADINGS + SUCCESS,
            payload: {
              headings: payload.headings,
              pageInfo: payload.pageInfo,
              noOrdersMessage: payload.noOrdersMessage
            }
          });
        }
      }).catch((error) => {
        dispatch({ type: RECENT_ORDERS_HEADINGS + FAILURE });
        alert(error); // eslint-disable-line no-alert
      });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: RECENT_ORDERS_HEADINGS + SUCCESS,
          payload: {
            headings: headings.headings,
            pageInfo: pageInfo.pageInfo,
            noOrdersMessage
          }
        });
      }, 2000);
    };

    // dev();
    prod();
  };
};

export const getRows = (page) => {
  return (dispatch) => {
    dispatch({ type: RECENT_ORDERS_ROWS + FETCH });
    dispatch({ type: APP_LOADING + START });

    const prod = () => {
      axios({
        method: 'get',
        url: `${RECENT_ORDERS.getPageItems}/${page}`
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: RECENT_ORDERS_ROWS + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
          dispatch({ type: APP_LOADING + FINISH });
        } else {
          dispatch({
            type: RECENT_ORDERS_ROWS + SUCCESS,
            payload: {
              rows: {
                [page - 1]: payload.rows
              }
            }
          });
          dispatch({ type: APP_LOADING + FINISH });
        }
      }).catch((error) => {
        dispatch({ type: RECENT_ORDERS_ROWS + FAILURE });
        alert(error); // eslint-disable-line no-alert
        dispatch({ type: APP_LOADING + FINISH });
      });
    };

    const dev = () => {
      setTimeout(() => {
        if (page % 2 === 0) {
          dispatch({
            type: RECENT_ORDERS_ROWS + SUCCESS,
            payload: {
              rows: {
                [page - 1]: rows1.payload.rows
              }
            }
          });
        } else {
          dispatch({
            type: RECENT_ORDERS_ROWS + SUCCESS,
            payload: {
              rows: {
                [page - 1]: rows2.payload.rows
              }
            }
          });
        }

        dispatch({ type: APP_LOADING + FINISH });
      }, 2500);
    };

    // dev();
    prod();
  };
};
