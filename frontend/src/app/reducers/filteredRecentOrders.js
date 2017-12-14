import {
  FILTERED_RECENT_ORDERS_GET_CAMPAIGNS,
  FILTERED_RECENT_ORDERS_GET_ORDERS,
  FETCH,
  FAILURE,
  SUCCESS
} from 'app.consts';

const defaultState = {
  orderType: {
    isFetching: false,
    isBlocked: false,
    selected: null
  },
  campaign: {
    isFetching: false,
    isBlocked: false,
    selected: null,
    placeholder: null,
    items: []
  },
  orders: {}
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + FETCH:
    return {
      orderType: {
        isFetching: true, // show spinned
        isBlocked: true, // block
        selected: payload.selected // set the value
      },
      campaign: {
        ...state.campaign,
        selected: null, // clear list
        items: [] // clear list
      },
      orders: {} // clear table
    };

  case FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + SUCCESS:
    return {
      ...state,
      orderType: {
        ...state.orderType,
        isFetching: false, // hide spinner
        isBlocked: false // unblock
      },
      campaign: {
        ...state.campaign,
        placeholder: payload.placeholder,
        items: payload.items // render list
      }
    };

  case FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + FAILURE:
    return {
      ...state,
      orderType: {
        isFetching: false,
        isBlocked: false,
        selected: null
      }
    };

  case FILTERED_RECENT_ORDERS_GET_ORDERS + FETCH:
    return {
      orderType: {
        ...state.orderType,
        isBlocked: true // block
      },
      campaign: {
        ...state.campaign,
        isFetching: true, // show spinner
        isBlocked: true, // block
        selected: payload.selected
      },
      orders: {} // clear table
    };

  case FILTERED_RECENT_ORDERS_GET_ORDERS + SUCCESS:
    return {
      orderType: {
        ...state.orderType,
        isBlocked: false // unblock
      },
      campaign: {
        ...state.campaign,
        isFetching: false, // hide spinner
        isBlocked: false // unblock
      },
      orders: payload // render table
    };

  case FILTERED_RECENT_ORDERS_GET_ORDERS + FAILURE:
    return {
      ...state,
      orderType: {
        ...state.orderType,
        isBlocked: false
      },
      campaign: {
        ...state.campaign,
        isFetching: false,
        isBlocked: false,
        selected: null
      }
    };

  default:
    return state;
  }
};
