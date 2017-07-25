import { FETCH, SUCCESS, FAILURE, INIT_UI, CHECKOUT, CHANGE_CHECKOUT_DATA, INIT_CHECKED_CHECKOUT_DATA,
  RECALCULATE_CHECKOUT_PRICE, SUBMIT_CHECKOUT, REMOVE_PRODUCT, CHANGE_PRODUCT_QUANTITY, CHECKOUT_ASK_PDF } from 'app.consts';

const defaultState = {
  ui: {},
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
  case CHANGE_PRODUCT_QUANTITY + SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isWaitingPDF: payload.isWaitingPDF,
      isAskingPDF: false
    };

  case REMOVE_PRODUCT + SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isWaitingPDF: payload.isWaitingPDF,
      isAskingPDF: false
    };

  case CHECKOUT + INIT_UI + SUCCESS:
  case RECALCULATE_CHECKOUT_PRICE + SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isSending: false,
      isWaitingPDF: payload.isWaitingPDF,
      isAskingPDF: false
    };

  case INIT_CHECKED_CHECKOUT_DATA:
    return {
      ...state,
      checkedData: {
        deliveryAddress: payload.deliveryAddress,
        deliveryMethod: payload.deliveryMethod,
        paymentMethod: payload.paymentMethod
      }
    };

  case CHANGE_CHECKOUT_DATA:
    return {
      ...state,
      checkedData: {
        ...state.checkedData,
        [payload.field]: payload.field === 'paymentMethod'
                          ? { id: payload.id,
                            invoice: payload.invoice
                          }
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
