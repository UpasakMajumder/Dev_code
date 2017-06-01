import { SHOPPING_CART_UI_FETCH, SHOPPING_CART_UI_SUCCESS, SHOPPING_CART_UI_FAILURE, CHANGE_SHOPPING_DATA,
  INIT_CHECKED_SHOPPING_DATA, RECALCULATE_SHOPPING_PRICE_SUCCESS, SEND_SHOPPING_DATA_SUCCESS,
  RECALCULATE_SHOPPING_PRICE_FETCH, SEND_SHOPPING_DATA_FETCH, ERROR_SHOPPING_VALIDATION, RECALCULATE_SHOPPING_PRICE_FAILURE,
  SEND_SHOPPING_DATA_FAILURE } from '../constants';

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
  isSending: false,
  validation: {
    field: ''
  }
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case ERROR_SHOPPING_VALIDATION:
    return {
      ...state,
      isSending: false,
      validation: {
        field: payload.field
      }
    };

  case RECALCULATE_SHOPPING_PRICE_FETCH:
  case SEND_SHOPPING_DATA_FETCH:
    return {
      ...state,
      validation: {
        field: ''
      },
      isSending: true
    };

  case SHOPPING_CART_UI_SUCCESS:
  case RECALCULATE_SHOPPING_PRICE_SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isSending: false
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
