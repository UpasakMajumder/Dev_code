import { SUCCESS, INIT_UI, ORDER_DETAIL, CHANGE_STATUS } from 'app.consts';

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

  default:
    return state;
  }
};
