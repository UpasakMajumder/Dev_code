import { GET_RECENT_ORDERS_HEADINGS_SUCCESS, GET_RECENT_ORDERS_ROWS_SUCCESS } from '../constants';

const defaultState = {
  headings: [],
  pageInfo: {},
  rows: {},
  noOrdersMessage: ''
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case GET_RECENT_ORDERS_HEADINGS_SUCCESS:
    return {
      ...state,
      headings: payload.headings,
      pageInfo: payload.pageInfo,
      noOrdersMessage: payload.noOrdersMessage
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
