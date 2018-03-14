import Immutable, { Map, List } from 'immutable';

import {
  SUCCESS,
  CHECKOUT_INIT_ITEMS,
  CHECKOUT_REMOVE_PRODUCT,
  CHECKOUT_CHANGE_PRODUCT_QUANTITY
} from 'app.consts';

const defaultState = Immutable.fromJS({
  products: [],
  quantityText: ''
});

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case CHECKOUT_INIT_ITEMS:
    return state.merge(Immutable.fromJS(payload));

  case CHECKOUT_REMOVE_PRODUCT + SUCCESS:
    return state
      .update('products', products => products.filter(product => product.get('id') !== payload.id))
      .set('quantityText', payload.quantityText);

  case CHECKOUT_CHANGE_PRODUCT_QUANTITY + SUCCESS:
    return state.update('products', (products) => {
      return products.map(product => (product.get('id') === payload.id ? product.set('quantity', payload.quantity) : product));
    });

  default:
    return state;
  }
};
