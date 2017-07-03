import { Map } from 'immutable';
import { ORDER_DETAIL_GET_UI_SUCCESS } from 'app.consts';

const defaultState = Map({ ui: {} });

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case ORDER_DETAIL_GET_UI_SUCCESS:
    return state.set('ui', payload.ui);

  default:
    return state;
  }
};
