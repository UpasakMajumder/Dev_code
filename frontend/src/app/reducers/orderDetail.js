import { SUCCESS, INIT_UI, ORDER_DETAIL } from 'app.consts';

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

  default:
    return state;
  }
};
