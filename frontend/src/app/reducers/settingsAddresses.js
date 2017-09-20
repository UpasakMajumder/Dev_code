import { SETTINGS_ADDRESSES, ADD_SHIPPING_ADDRESS, MODIFY_SHIPPING_ADDRESS, SUCCESS, INIT_UI } from 'app.consts';

export default (state = {}, action) => {
  const { type, payload } = action;

  let mappedAddresses = [];
  let addresses = [];

  switch (type) {
  case SETTINGS_ADDRESSES + INIT_UI + SUCCESS:
    return payload.ui;

  case ADD_SHIPPING_ADDRESS + SUCCESS:
    return {
      ...state,
      shipping: {
        ...state.shipping,
        addresses: [
          ...state.shipping.addresses,
          payload
        ]
      }
    };

  case MODIFY_SHIPPING_ADDRESS + SUCCESS:
    mappedAddresses = state.shipping.addresses.map((address) => {
      if (address.id === payload.id) {
        return {
          ...address,
          ...payload
        };
      }

      return address;
    });

    addresses = state.shipping.addresses.length
      ? mappedAddresses
      : [payload];

    return {
      ...state,
      shipping: {
        ...state.shipping,
        addresses
      }
    };

  default:
    return state;
  }
};
