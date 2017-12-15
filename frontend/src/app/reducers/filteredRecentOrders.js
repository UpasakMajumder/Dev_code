import {
  FILTERED_RECENT_ORDERS_GET_CAMPAIGNS,
  FILTERED_RECENT_ORDERS_GET_ORDERS,
  FETCH,
  FAILURE,
  SUCCESS
} from 'app.consts';

const defaultState = {
  isFetching: false,
  orderType: null,
  campaign: {
    value: null,
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
      isFetching: true, // show spinner/block
      orderType: payload.value,
      campaign: {
        ...state.campaign,
        value: null, // clear list
        items: [] // clear list
      },
      orders: {} // clear table
    };

  case FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + SUCCESS:
    return {
      ...state,
      isFetching: false, // hide spinner/unblock
      orderType: state.orderType,
      campaign: {
        ...state.campaign,
        placeholder: payload.placeholder,
        items: payload.items // render list
      }
    };

  case FILTERED_RECENT_ORDERS_GET_CAMPAIGNS + FAILURE:
    return {
      ...state,
      isFetching: false,
      orderType: null
    };

  case FILTERED_RECENT_ORDERS_GET_ORDERS + FETCH:
    return {
      isFetching: true, // show spinner/block
      orderType: payload.selectedOrderType,
      campaign: {
        ...state.campaign,
        items: payload.selectedCampaign ? state.campaign.items : [],
        value: payload.selectedCampaign
      },
      orders: {} // clear table
    };

  case FILTERED_RECENT_ORDERS_GET_ORDERS + SUCCESS:
    return {
      isFetching: false,
      orderType: state.orderType,
      campaign: state.campaign,
      orders: payload // render table
    };

  case FILTERED_RECENT_ORDERS_GET_ORDERS + FAILURE:
    return {
      ...state,
      isFetching: false,
      orderType: state.orderType,
      campaign: {
        ...state.campaign,
        value: null
      }
    };

  default:
    return state;
  }
};
