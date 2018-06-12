import { SUCCESS, INIT_UI, ORDER_DETAIL, CHANGE_STATUS, EDIT_ORDERS } from 'app.consts';

const defaultState = {
  ui: {}
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case ORDER_DETAIL + INIT_UI + SUCCESS:
    return {
      ui: payload.ui
    };

  case ORDER_DETAIL + CHANGE_STATUS:
    return {
      ui: {
        ...state.ui,
        commonInfo: {
          ...state.ui.commonInfo,
          status: {
            ...state.ui.commonInfo.status,
            value: payload.status,
            note: payload.note
          }
        }
      }
    };

  case ORDER_DETAIL + EDIT_ORDERS:
    return {
      ui: {
        ...state.ui,
        pricingInfo: {
          ...state.ui.pricingInfo,
          items: payload.pricingInfo
        },
        orderedItems: {
          ...state.ui.orderedItems,
          items: state.ui.orderedItems.map(item => ({ ...item, quantity: payload.orderedItems[item.id] }))
        }
      }
    };


  default:
    return state;
  }
};
