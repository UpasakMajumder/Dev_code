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
  RECENT_ORDERS_CHANGE_DATE
} from 'app.consts';
/* helpers */
import { createSearchStr } from 'app.helpers/location';
/* globals */
import { RECENT_ORDERS as RECENT_ORDERS_GLOBAL, NOTIFICATION } from 'app.globals';

const getUrl = (defaultUrl, args) => {
  let url = `${defaultUrl}/${args.page}`;
  if (typeof args.page !== 'undefined') return url;

  const search = {};

  if (args.sort) search.sort = args.sort;
  if (args.dateFrom) search.dateFrom = args.dateFrom;
  if (args.dateTo) search.dateTo = args.dateTo;

  const searchUrl = Object.keys(search).length ? createSearchStr(search) : '';

  url = `${defaultUrl}${searchUrl}`;

  return url;
};

export const getRows = (url, args = {}) => {
  return async (dispatch) => {
    dispatch({ type: RECENT_ORDERS_GET_ROWS + FETCH });

    const formattedUrl = getUrl(url, args);

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

export const changeDate = (value, field) => {
  return {
    type: RECENT_ORDERS_CHANGE_DATE,
    payload: {
      value,
      field
    }
  };
};
