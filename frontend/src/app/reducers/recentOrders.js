import { GET_RECENT_ORDERS_HEADINGS_SUCCESS, GET_RECENT_ORDERS_ROWS_SUCCESS } from '../constants';

const defaultState = {
  headings: [],
  pagination: {},
  rows: {}
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case GET_RECENT_ORDERS_HEADINGS_SUCCESS:
    return {
      ...state,
      headings: payload.headings,
      pagination: payload.pagination
    };

  case GET_RECENT_ORDERS_ROWS_SUCCESS:
    return {
      ...state,
      rows: {
        ...state.rows,
        ...payload.rows
      }
    };

  default:
    return state;
  }
};
