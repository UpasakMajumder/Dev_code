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
  RECENT_ORDERS_GET_ROWS,
  RECENT_ORDERS_SORT
} from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { RECENT_ORDERS as RECENT_ORDERS_GLOBAL, NOTIFICATION } from 'app.globals';

export default (url, args = {}) => {
  return async (dispatch) => {
    dispatch({ type: RECENT_ORDERS_GET_ROWS + FETCH });

    let formattedUrl = url;

    if (args.page) formattedUrl = `${url}/${args.page}`;
    if (args.sort) formattedUrl = `${url}?sort=${args.sort}`;

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
            pagination,
            sortOrderAsc: args.sortOrderAsc,
            sortBy: args.sortBy
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
