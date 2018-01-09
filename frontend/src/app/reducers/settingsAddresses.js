import { SETTINGS_ADDRESSES, ADD_SHIPPING_ADDRESS, MODIFY_SHIPPING_ADDRESS, SUCCESS, INIT_UI,
  SET_ADDRESS_DEFAULT, UNSET_ADDRESS_DEFAULT, FETCH } from 'app.consts';

export default (state = {}, action) => {
  const { type, payload } = action;

  let mappedAddresses = [];
  let addresses = [];

  switch (type) {
  case SET_ADDRESS_DEFAULT + FETCH:
    return {
      ...state,
      [payload.type]: {
        ...state[payload.type],
        defaultAddress: {
          ...state[payload.type].defaultAddress,
          id: payload.id
        }
      }
    };

  case UNSET_ADDRESS_DEFAULT + FETCH:
    return {
      ...state,
      [payload.type]: {
        ...state[payload.type],
        defaultAddress: {
          ...state[payload.type].defaultAddress,
          id: null
        }
      }
    };


  case SETTINGS_ADDRESSES + INIT_UI + SUCCESS:
    return payload.ui;

  case ADD_SHIPPING_ADDRESS + SUCCESS:
    if (!Object.keys(state).length) return state;
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
