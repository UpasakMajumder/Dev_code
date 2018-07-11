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
            value: payload.status
          }
        }
      }
    };

  case ORDER_DETAIL + EDIT_ORDERS:
    return {
      ui: {
        ...state.ui,
        commonInfo: {
          ...state.ui.commonInfo,
          totalCost: {
            ...state.ui.commonInfo.totalCost,
            value: payload.pricingInfo[payload.pricingInfo.length - 1].value // totalCost is always the last item
          }
        },
        pricingInfo: {
          ...state.ui.pricingInfo,
          items: payload.pricingInfo
        },
        orderedItems: {
          ...state.ui.orderedItems,
          items: state.ui.orderedItems.items.map((item) => {
            const orderedItem = payload.orderedItems.find(orderedItem => orderedItem.lineNumber === item.lineNumber);
            if (!orderedItem) return item;

            const priceItem = payload.ordersPrice.find(order => order.lineNumber === item.lineNumber);

            return {
              ...item,
              removed: orderedItem.removed,
              quantity: orderedItem.quantity,
              price: priceItem && priceItem.price
            };
          })
        }
      }
    };


  default:
    return state;
  }
};
