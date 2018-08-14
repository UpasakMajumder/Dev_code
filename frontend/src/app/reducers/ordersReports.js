import {
  FETCH,
  SUCCESS,
  FAILURE,
  ORDERS_REPORTS_GET_ROWS,
  ORDERS_REPORTS_CHANGE_DATE,
  ORDERS_REPORTS_MANAGE
} from 'app.consts';

const defaultState = {
  rowsAreAsked: false,
  pagination: {
    currentPage: 0  // BE requires to start the pages from 1
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

const getNewRows = (stateRows, payloadRows) => {
  const newRows = JSON.parse(JSON.stringify(stateRows));

  payloadRows.forEach((row, index) => {
    const keys = Object.keys(row);
    keys.forEach((key) => {
      if (newRows[index].items[key]) {
        newRows[index].items[key].value = row[key];
      } else {
        newRows[index].items[key] = { value: row[key] };
      }
    });
  });

  return newRows;
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
      rows: defaultState.rows,
      rowsAreAsked: false
    };

  case ORDERS_REPORTS_GET_ROWS + SUCCESS:
    return {
      ...state,
      rowsAreAsked: true,
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

  case ORDERS_REPORTS_GET_ROWS + FAILURE:
    return {
      ...state,
      rowsAreAsked: true
    };

  case ORDERS_REPORTS_MANAGE:
    return {
      ...state,
      rows: getNewRows(state.rows, payload.rows)
    };

  default:
    return state;
  }
};
