import { SETTINGS_ADDRESSES, MODIFY_SHIPPING_ADDRESS, SUCCESS } from 'app.consts';

export default (state = {}, action) => {
  const { type, payload } = action;

  let mappedAddresses = [];
  let addresses = [];

  switch (type) {
  case SETTINGS_ADDRESSES + SUCCESS:
    return payload.ui;

  case MODIFY_SHIPPING_ADDRESS + SUCCESS:
    mappedAddresses = state.shipping.addresses.map((address) => {
      if (address.id === payload.data.id) {
        return {
          ...address,
          ...payload.data
        };
      }

      return address;
    });

    addresses = state.shipping.addresses.length
      ? mappedAddresses
      : [{
        ...payload.data,
        isEditButton: true,
        isRemoveButton: false
      }];

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
