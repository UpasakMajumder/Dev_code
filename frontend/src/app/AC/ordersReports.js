import axios from 'axios';
/* constants */
import {
  FETCH,
  SUCCESS,
  FAILURE,
  ORDERS_REPORTS_GET_ROWS,
  ORDERS_REPORTS_CHANGE_DATE
} from 'app.consts';

export const getRows = (url, args = {}) => {
  return async (dispatch) => {
    dispatch({ type: ORDERS_REPORTS_GET_ROWS + FETCH });

    try {
      const { data: { payload, success, errorMessage } } = await axios.get(url);

      if (!success) {
        dispatch({
          type: ORDERS_REPORTS_GET_ROWS + FAILURE,
          alert: errorMessage
        });
      } else {
        let pagination = payload.pagination;

        if (typeof args.page !== 'undefined') {
          pagination = { ...pagination, currentPage: args.page };
        }

        dispatch({
          type: ORDERS_REPORTS_GET_ROWS + SUCCESS,
          payload: {
            rows: payload.rows || [],
            pagination,
            sortOrderAsc: args.sortOrderAsc,
            sortBy: args.sortBy
          }
        });
      }
    } catch (error) {
      dispatch({
        type: ORDERS_REPORTS_GET_ROWS + FAILURE
      });
    }
  };
};

export const changeDate = (value, field) => {
  return {
    type: ORDERS_REPORTS_CHANGE_DATE,
    payload: {
      value,
      field
    }
  };
};
