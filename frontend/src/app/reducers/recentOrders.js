import { RECENT_ORDERS_ROWS, RECENT_ORDERS_HEADINGS, SUCCESS } from 'app.consts';

const defaultState = {
  headings: [],
  pageInfo: {},
  rows: {},
  noOrdersMessage: ''
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case RECENT_ORDERS_HEADINGS + SUCCESS:
    return {
      ...state,
      headings: payload.headings,
      pageInfo: payload.pageInfo,
      noOrdersMessage: payload.noOrdersMessage
    };

  case RECENT_ORDERS_ROWS + SUCCESS:
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
