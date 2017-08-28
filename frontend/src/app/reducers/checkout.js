import { FETCH, SUCCESS, FAILURE, INIT_UI, CHANGE_CHECKOUT_DATA, INIT_CHECKED_CHECKOUT_DATA, CHECKOUT_STATIC,
  RECALCULATE_CHECKOUT_PRICE, SUBMIT_CHECKOUT, REMOVE_PRODUCT, CHANGE_PRODUCT_QUANTITY, CHECKOUT_PRICING, ADD_NEW_ADDRESS } from 'app.consts';

const defaultState = {
  ui: {},
  addedDataId: null,
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
  const items = []; // for ADD_NEW_ADDRESS + SUCCESS

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
    state.ui.deliveryAddresses.items.forEach((item) => {
      if (item.id !== payload.oldId) {
        items.push({
          ...item,
          checked: false
        });
      }
    });

    items.push({
      ...payload.data,
      checked: true
    });

    return {
      ...state,
      addedAddressId: payload.data.id,
      ui: {
        ...state.ui,
        deliveryAddresses: {
          ...state.ui.deliveryAddresses,
          items
        }
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
