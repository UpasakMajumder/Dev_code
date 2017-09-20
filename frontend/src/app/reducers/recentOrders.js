import { RECENT_ORDERS_CHANGE_PAGE, SUCCESS, RECENT_ORDERS, INIT_UI } from 'app.consts';

const defaultState = {
  headings: [],
  pageInfo: {},
  rows: {},
  noOrdersMessage: ''
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case RECENT_ORDERS + INIT_UI + SUCCESS:
    return {
      ...state,
      headings: payload.headings,
      pageInfo: payload.pageInfo,
      noOrdersMessage: payload.noOrdersMessage,
      rows: {
        ...state.rows,
        ...payload.rows
      }
    };

  case RECENT_ORDERS_CHANGE_PAGE + SUCCESS:
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
