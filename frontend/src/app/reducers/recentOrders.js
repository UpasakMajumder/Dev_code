import {
  RECENT_ORDERS_CHANGE_PAGE,
  FETCH,
  SUCCESS,
  RECENT_ORDERS,
  INIT_UI,
  RECENT_ORDERS_GET_ROWS,
  RECENT_ORDERS_SORT
} from 'app.consts';

const defaultState = {
  pagination: {
    currentPage: 0
  },
  rows: [],
  sort: {
    sortOrderAsc: false,
    sortBy: null
  }
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case RECENT_ORDERS_GET_ROWS + FETCH:
    return {
      ...state,
      rows: defaultState.rows
    };

  case RECENT_ORDERS_GET_ROWS + SUCCESS:
    return {
      ...state,
      pagination: {
        ...state.pagination,
        ...payload.pagination
      },
      rows: [
        ...state.rows,
        ...payload.rows
      ],
      sort: {
        ...state.sort,
        sortOrderAsc: typeof payload.sortOrderAsc === 'undefined' ? state.sort.sortOrderAsc : payload.sortOrderAsc,
        sortBy: typeof payload.sortBy === 'undefined' ? state.sort.sortBy : payload.sortBy
      }
    };


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
