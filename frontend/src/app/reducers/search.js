import { SEARCH_RESULTS_HIDE, SEARCH_RESULTS_SHOW, SEARCH_RESULT_GET_SUCCESS, SEARCH_QUERY_CHANGE } from '../constants';

const defaultState = {
  products: {},
  pages: {},
  message: '',
  query: '',
  areResultsShown: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case SEARCH_QUERY_CHANGE:
    return {
      ...state,
      query: payload.query
    };

  case SEARCH_RESULTS_HIDE:
    return defaultState;

  case SEARCH_RESULTS_SHOW:
    return {
      ...state,
      areResultsShown: true
    };

  case SEARCH_RESULT_GET_SUCCESS:
    return {
      ...state,
      ...payload
    };

  default:
    return state;
  }
};
