import { FETCH, SUCCESS, FAILURE, INIT_UI, CHANGE_CHECKOUT_DATA, INIT_CHECKED_CHECKOUT_DATA, CHECKOUT_STATIC,
  RECALCULATE_CHECKOUT_PRICE, SUBMIT_CHECKOUT, REMOVE_PRODUCT, CHANGE_PRODUCT_QUANTITY, CHECKOUT_PRICING, ADD_NEW_ADDRESS } from 'app.consts';

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
  case CHECKOUT_STATIC + INIT_UI + SUCCESS:
  case CHECKOUT_PRICING + INIT_UI + SUCCESS:
    return {
      ...state,
      ui: {
        ...state.ui,
        ...payload.ui
      }
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

  case CHANGE_PRODUCT_QUANTITY + SUCCESS:
    return {
      ...state,
      ui: payload.ui
    };

  case REMOVE_PRODUCT + SUCCESS:
    return {
      ...state,
      ui: payload.ui
    };

  case RECALCULATE_CHECKOUT_PRICE + SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isSending: false
    };

  case INIT_CHECKED_CHECKOUT_DATA:
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

  case CHANGE_CHECKOUT_DATA:
    return {
      ...state,
      checkedData: {
        ...state.checkedData,
        [payload.field]: payload.field === 'paymentMethod'
          ? { id: payload.id, invoice: payload.invoice, card: payload.card }
          : payload.id
      }
    };

  case RECALCULATE_CHECKOUT_PRICE + FETCH:
  case SUBMIT_CHECKOUT + FETCH:
    return {
      ...state,
      isSending: true
    };

  case SUBMIT_CHECKOUT + SUCCESS:
    return {
      ...state,
      sendData: {
        status: payload.status,
        redirectURL: payload.redirectURL
      },
      isSending: false
    };

  case RECALCULATE_CHECKOUT_PRICE + FAILURE:
  case SUBMIT_CHECKOUT + FAILURE:
    return {
      ...state,
      isSending: false
    };

  default:
    return state;
  }
};
