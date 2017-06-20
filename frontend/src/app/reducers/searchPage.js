import { SEARCH_PAGE_UI_SUCCESS } from '../constants';

const defaultState = {
  products: [],
  pages: []
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case SEARCH_PAGE_UI_SUCCESS:
    return {
      ...state,
      ...payload
    };

  default:
    return state;
  }
};
