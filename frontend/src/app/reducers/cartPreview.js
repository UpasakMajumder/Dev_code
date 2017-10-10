import { CART_PREVIEW, CART_PREVIEW_CHANGE_ITEMS, INIT_UI, SUCCESS, TOGGLE } from 'app.consts';

const defaultState = {
  emptyCartMessage: '',
  cart: {},
  items: [],
  summaryPrice: {},
  isVisible: false,
  isLoaded: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case CART_PREVIEW + INIT_UI + SUCCESS:
    return {
      ...state,
      emptyCartMessage: payload.emptyCartMessage,
      cart: payload.cart,
      items: payload.items,
      summaryPrice: payload.summaryPrice,
      isLoaded: true
    };

  case CART_PREVIEW + TOGGLE:
    return {
      ...state,
      isVisible: payload.isVisible
    };

  case CART_PREVIEW_CHANGE_ITEMS: {
    return {
      ...state,
      items: payload.items,
      summaryPrice: payload.summaryPrice
    };
  }

  default:
    return state;
  }
};
