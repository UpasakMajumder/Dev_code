import { SHOPPING_CART_UI_FETCH, SHOPPING_CART_UI_SUCCESS, SHOPPING_CART_UI_FAILURE, CHANGE_SHOPPING_DATA,
  INIT_CHECKED_SHOPPING_DATA, RECALCULATE_SHIPPING_PRICE_SUCCESS, SEND_SHIPPING_DATA_SUCCESS,
  RECALCULATE_SHIPPING_PRICE_FETCH, SEND_SHIPPING_DATA_FETCH, ERROR_SHIPPING_VALIDATION } from '../constants';

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
  let deliveryAddress = 0;
  let deliveryMethod = 0;
  let paymentMethod = 0;

  switch (type) {
  case ERROR_SHIPPING_VALIDATION:
    return {
      ...state,
      isSending: false,
      validation: {
        field: payload.field
      }
    };

  case RECALCULATE_SHIPPING_PRICE_FETCH:
  case SEND_SHIPPING_DATA_FETCH:
    return {
      ...state,
      validation: {
        field: ''
      },
      isSending: true
    };

  case SHOPPING_CART_UI_SUCCESS:
  case RECALCULATE_SHIPPING_PRICE_SUCCESS:
    return {
      ...state,
      ui: payload.ui,
      isSending: false
    };

  case INIT_CHECKED_SHOPPING_DATA:
    state.ui.deliveryAddresses.items.forEach((address) => {
      if (address.checked) deliveryAddress = address.id;
    });

    state.ui.deliveryMethod.items.forEach((methodGroup) => {
      methodGroup.items.forEach((method) => {
        if (method.checked && !deliveryMethod) deliveryMethod = method.id;
      });
    });

    state.ui.paymentMethod.items.forEach((method) => {
      if (method.checked) paymentMethod = { id: method.id, invoice: '' };
    });

    return {
      ...state,
      checkedData: {
        deliveryAddress,
        deliveryMethod,
        paymentMethod
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

  case SEND_SHIPPING_DATA_SUCCESS:
    return {
      ...state,
      sendData: {
        status: payload.status,
        redirectURL: payload.redirectURL
      },
      isSending: false
    };

  default:
    return state;
  }
};
