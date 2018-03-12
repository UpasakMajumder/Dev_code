import {
  FETCH,
  SUCCESS,
  ORDERS_REPORTS_GET_ROWS,
  ORDERS_REPORTS_CHANGE_DATE
} from 'app.consts';

const defaultState = {
  pagination: {
    currentPage: 0
  },
  rows: [],
  sort: {
    sortOrderAsc: false,
    sortBy: undefined
  },
  filter: {
    orderDate: {
      dateFrom: null,
      dateTo: null
    }
  }
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case ORDERS_REPORTS_CHANGE_DATE:
    return {
      ...state,
      filter: {
        ...state.filter,
        orderDate: {
          ...state.filter.orderDate,
          [payload.field]: payload.value
        }
      }
    };

  case ORDERS_REPORTS_GET_ROWS + FETCH:
    return {
      ...state,
      rows: defaultState.rows
    };

  case ORDERS_REPORTS_GET_ROWS + SUCCESS:
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

  default:
    return state;
  }
};
