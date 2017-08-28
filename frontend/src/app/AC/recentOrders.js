import axios from 'axios';
/* constants */
import { INIT_UI, FETCH, SUCCESS, FAILURE, START, FINISH, RECENT_ORDERS_CHANGE_PAGE, APP_LOADING, RECENT_ORDERS } from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { RECENT_ORDERS as RECENT_ORDERS_GLOBAL, NOTIFICATION } from 'app.globals';
/* web service */
import { headings, pageInfo, rows1, rows2, noOrdersMessage } from 'app.ws/recentOrders';

export const initUI = () => {
  return (dispatch) => {
    dispatch({ type: RECENT_ORDERS + INIT_UI + FETCH });

    const prod = () => {
      axios({
        method: 'get',
        url: RECENT_ORDERS_GLOBAL.getHeaders
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: RECENT_ORDERS + INIT_UI + FAILURE,
            alert: errorMessage
          });
        } else {
          dispatch({
            type: RECENT_ORDERS + INIT_UI + SUCCESS,
            payload: {
              headings: payload.headings,
              pageInfo: payload.pageInfo,
              rows: { 0: payload.rows },
              noOrdersMessage: payload.noOrdersMessage
            }
          });
        }
      }).catch((error) => {
        dispatch({
          type: RECENT_ORDERS + INIT_UI + FAILURE,
          alert: NOTIFICATION.recentOrdersError.title
        });
      });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: RECENT_ORDERS + INIT_UI + SUCCESS,
          payload: {
            headings: headings.headings,
            pageInfo: pageInfo.pageInfo,
            rows: { 0: rows2.payload.rows },
            noOrdersMessage
          }
        });
      }, 2000);
    };

    callAC(dev, prod);
  };
};

export const changePage = (page, isNotFirst) => {
  return (dispatch) => {
    dispatch({ type: RECENT_ORDERS_CHANGE_PAGE + FETCH });
    if (isNotFirst) dispatch({ type: APP_LOADING + START });

    const prod = () => {
      axios({
        method: 'get',
        url: `${RECENT_ORDERS_GLOBAL.getPageItems}/${page}`
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: RECENT_ORDERS_CHANGE_PAGE + FAILURE,
            alert: errorMessage
          });
          if (isNotFirst) dispatch({ type: APP_LOADING + FINISH });
        } else {
          dispatch({
            type: RECENT_ORDERS_CHANGE_PAGE + SUCCESS,
            payload: {
              rows: {
                [page - 1]: payload.rows
              }
            }
          });
          if (isNotFirst) dispatch({ type: APP_LOADING + FINISH });
        }
      }).catch((error) => {
        dispatch({ type: RECENT_ORDERS_CHANGE_PAGE + FAILURE });
        if (isNotFirst) dispatch({ type: APP_LOADING + FINISH });
      });
    };

    const dev = () => {
      setTimeout(() => {
        if (page % 2 === 0) {
          dispatch({
            type: RECENT_ORDERS_CHANGE_PAGE + SUCCESS,
            payload: {
              rows: {
                [page - 1]: rows1.payload.rows
              }
            }
          });
        } else {
          dispatch({
            type: RECENT_ORDERS_CHANGE_PAGE + SUCCESS,
            payload: {
              rows: {
                [page - 1]: rows2.payload.rows
              }
            }
          });
        }

        if (isNotFirst) dispatch({ type: APP_LOADING + FINISH });
      }, 2500);
    };

    callAC(dev, prod);
  };
};
