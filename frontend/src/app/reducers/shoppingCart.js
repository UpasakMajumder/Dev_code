import { SHOPPING_CART_UI_SUCCESS, CHANGE_SHOPPING_DATA,
  INIT_CHECKED_SHOPPING_DATA, RECALCULATE_SHOPPING_PRICE_SUCCESS, SEND_SHOPPING_DATA_SUCCESS,
  RECALCULATE_SHOPPING_PRICE_FETCH, SEND_SHOPPING_DATA_FETCH, RECALCULATE_SHOPPING_PRICE_FAILURE,
  SEND_SHOPPING_DATA_FAILURE, REMOVE_PRODUCT_SUCCESS, CHANGE_PRODUCT_QUANTITY_SUCCESS,
  CHECKOUT_ASK_PDF_SUCCESS, CHECKOUT_ASK_PDF_FETCH, CHECKOUT_ASK_PDF_FAILURE } from '../constants';

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
  isWaitingPDF: false,
  isAskingPDF: false,
  isSending: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case CHECKOUT_ASK_PDF_FAILURE:
    return {
      ...state,
      isAskingPDF: false
    };

  case CHECKOUT_ASK_PDF_FETCH:
    return {
      ...state,
      isAskingPDF: true
    };


  case CHECKOUT_ASK_PDF_SUCCESS:
    return {
      ...state,
      isWaitingPDF: payload.isWaitingPDF,
      isAskingPDF: false
    };

  case CHANGE_PRODUCT_QUANTITY_SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isWaitingPDF: payload.isWaitingPDF,
      isAskingPDF: false
    };

  case REMOVE_PRODUCT_SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isWaitingPDF: payload.isWaitingPDF,
      isAskingPDF: false
    };

  case SHOPPING_CART_UI_SUCCESS:
  case RECALCULATE_SHOPPING_PRICE_SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isSending: false,
      isWaitingPDF: payload.isWaitingPDF,
      isAskingPDF: false
    };

  case INIT_CHECKED_SHOPPING_DATA:
    return {
      ...state,
      checkedData: {
        deliveryAddress: payload.deliveryAddress,
        deliveryMethod: payload.deliveryMethod,
        paymentMethod: payload.paymentMethod
      }
    };

  case CHANGE_SHOPPING_DATA:
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

  case RECALCULATE_SHOPPING_PRICE_FETCH:
  case SEND_SHOPPING_DATA_FETCH:
    return {
      ...state,
      isSending: true
    };

  case SEND_SHOPPING_DATA_SUCCESS:
    return {
      ...state,
      sendData: {
        status: payload.status,
        redirectURL: payload.redirectURL
      },
      isSending: false
    };

  case RECALCULATE_SHOPPING_PRICE_FAILURE:
  case SEND_SHOPPING_DATA_FAILURE:
    return {
      ...state,
      isSending: false
    };

  default:
    return state;
  }
};
