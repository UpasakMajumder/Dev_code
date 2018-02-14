import {
  FETCH,
  SUCCESS,
  FAILURE,
  INIT_UI,
  CHECKOUT,
  CHECKOUT_INIT_CHECKED_DATA,
  CHECKOUT_CHANGE_PAYMENT,
  CHECKOUT_CHANGE_QUANTITY,
  CHECKOUT_REMOVE_PRODUCT,
  CHECKOUT_CHANGE_ADDRESS,
  CHECKOUT_GET_TOTALS,
  CHECKOUT_PROCEED,
  CHECKOUT_CHANGE_DELIVERY,
  ADD_NEW_ADDRESS
} from 'app.consts';

const defaultState = {
  ui: {},
  newAddress: {},
  checkedData: {
    deliveryAddress: 0,
    deliveryMethod: 0,
    paymentMethod: {
      id: 0,
      invoice: ''
    }
  },
  sendData: {
    status: '',
    redirectUrl: ''
  },
  isSending: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case CHECKOUT + INIT_UI + SUCCESS:
    return {
      ...state,
      ui: payload
    };

  case CHECKOUT_INIT_CHECKED_DATA:
    return {
      ...state,
      checkedData: {
        // don't override New Address selection with Kentico state
        deliveryAddress: state.checkedData.deliveryAddress === -1 ? state.checkedData.deliveryAddress : payload.deliveryAddress,
        deliveryMethod: payload.deliveryMethod,
        paymentMethod: {
          ...state.checkedData.paymentMethod,
          ...payload.paymentMethod
        }
      }
    };

  case CHECKOUT_CHANGE_PAYMENT:
    return {
      ...state,
      ui: {
        ...state.ui,
        paymentMethods: {
          ...state.ui.paymentMethods,
          items: state.ui.paymentMethods.items.map(item => ({ ...item, checked: item.id === payload.id }))
        }
      },
      checkedData: {
        ...state.checkedData,
        paymentMethod: {
          id: payload.id,
          invoice: payload.invoice
        }
      }
    };

  case CHECKOUT_CHANGE_QUANTITY + SUCCESS:
  case CHECKOUT_REMOVE_PRODUCT + SUCCESS:
    return {
      ...state,
      ui: {
        ...state.ui,
        products: payload.products
      }
    };

  case CHECKOUT_CHANGE_ADDRESS + SUCCESS:
    return {
      ...state,
      ui: {
        ...state.ui,
        deliveryAddresses: payload.deliveryAddresses
      },
      checkedData: {
        ...state.checkedData,
        deliveryAddress: payload.id
      }
    };

  case CHECKOUT_GET_TOTALS + FETCH:
  case CHECKOUT_CHANGE_DELIVERY + FETCH:
    return {
      ...state,
      ui: state.ui.totals ? { ...state.ui, totals: null } : state.ui
    };

  case CHECKOUT_CHANGE_DELIVERY + SUCCESS:
    return {
      ...state,
      ui: {
        ...state.ui,
        totals: payload.totals,
        deliveryMethods: payload.deliveryMethods
      },
      checkedData: {
        ...state.checkedData,
        deliveryMethod: payload.id
      },
      isSending: false
    };

  case CHECKOUT_GET_TOTALS + SUCCESS:
    return {
      ...state,
      ui: {
        ...state.ui,
        totals: payload.totals,
        deliveryMethods: payload.deliveryMethods
      },
      isSending: false
    };

  case CHECKOUT_GET_TOTALS + FAILURE:
  case CHECKOUT_PROCEED + FAILURE:
    return {
      ...state,
      isSending: false
    };

  case CHECKOUT_PROCEED + FETCH:
    return {
      ...state,
      isSending: true
    };

  case CHECKOUT_PROCEED + SUCCESS:
    return {
      ...state,
      sendData: {
        status: payload.status,
        redirectURL: payload.redirectURL
      },
      isSending: false
    };

  case ADD_NEW_ADDRESS + SUCCESS:
    return {
      ...state,
      newAddress: payload,
      checkedData: {
        ...state.checkedData,
        deliveryAddress: -1
      }
    };

  default:
    return state;
  }
};
