import { SHOW, HIDE, SUCCESS, SEARCH_RESULTS, CHANGE_SEARCH_QUERY } from 'app.consts';

const defaultState = {
  products: {},
  pages: {},
  message: '',
  query: '',
  areResultsShown: false,
  pressedEnter: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case CHANGE_SEARCH_QUERY:
    return {
      ...state,
      query: payload.query
    };

  case SEARCH_RESULTS + HIDE:
    return defaultState;

  case SEARCH_RESULTS + SHOW:
    return {
      ...state,
      areResultsShown: true
    };

  case SEARCH_RESULTS + SUCCESS:
    return {
      ...state,
      ...payload
    };

  default:
    return state;
  }
};
