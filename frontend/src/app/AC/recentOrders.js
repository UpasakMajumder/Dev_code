import axios from 'axios';
/* constants */
import {
  INIT_UI,
  FETCH,
  SUCCESS,
  FAILURE,
  START,
  FINISH,
  RECENT_ORDERS_CHANGE_PAGE,
  APP_LOADING,
  RECENT_ORDERS,
  RECENT_ORDERS_GET_ROWS
} from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { RECENT_ORDERS as RECENT_ORDERS_GLOBAL, NOTIFICATION } from 'app.globals';

export const getRows = (url, args = {}) => {
  return async (dispatch) => {
    dispatch({ type: RECENT_ORDERS_GET_ROWS + FETCH });

    let formattedUrl = url;

    if (args.page) formattedUrl = `${url}/${args.page}`;

    try {
      const { data: { payload, success, errorMessage } } = await axios.get(formattedUrl);

      if (!success) {
        dispatch({
          type: RECENT_ORDERS_GET_ROWS + FAILURE,
          alert: errorMessage
        });
      } else {
        let pagination = payload.pagination;

        if (typeof args.page !== 'undefined') {
          pagination = { ...pagination, currentPage: args.page };
        }

        dispatch({
          type: RECENT_ORDERS_GET_ROWS + SUCCESS,
          payload: {
            rows: payload.rows,
            pagination
          }
        });
      }
    } catch (error) {
      dispatch({
        type: RECENT_ORDERS_GET_ROWS + FAILURE,
        alert: false
      });
    }
  };
};

////////////////////////////////////////////////////////////////////////

export const initUI = () => {
  return (dispatch) => {
    dispatch({ type: RECENT_ORDERS + INIT_UI + FETCH });

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
        // alert: NOTIFICATION.recentOrdersError.title
        alert: false
      });
    });
  };
};

export const changePage = (page, isNotFirst) => {
  return (dispatch) => {
    dispatch({ type: RECENT_ORDERS_CHANGE_PAGE + FETCH });
    if (isNotFirst) dispatch({ type: APP_LOADING + START });

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
};
