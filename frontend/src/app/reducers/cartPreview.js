import { CART_PREVIEW, INIT_UI, SUCCESS, TOGGLE } from 'app.consts';

const defaultState = {
  emptyCartMessage: '',
  cart: {},
  items: [],
  isVisible: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case CART_PREVIEW + INIT_UI + SUCCESS:
    return {
      ...state,
      emptyCartMessage: payload.emptyCartMessage,
      cart: payload.cart,
      items: payload.items
    };

  case CART_PREVIEW + TOGGLE:
    return {
      ...state,
      isVisible: payload.isVisible
    };

  default:
    return state;
  }
};
