import { ORDER_DETAIL_GET_UI_SUCCESS } from '../constants';

const defaultState = {
  ui: {}
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case ORDER_DETAIL_GET_UI_SUCCESS:
    return {
      ...state,
      ui: payload.ui
    };

  default:
    return state;
  }
};
