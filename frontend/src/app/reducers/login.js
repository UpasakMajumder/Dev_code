import { LOG_IN, VALIDATION_ERROR, FETCH, SUCCESS, FAILURE } from 'app.consts';

const defaultState = {
  response: null,
  isLoading: false
};

export default function login(state = defaultState, action = {}) {
  switch (action.type) {
  case LOG_IN + FETCH:
    return {
      ...state,
      isLoading: action.isLoading,
      response: null
    };

  case LOG_IN + VALIDATION_ERROR:
    return {
      ...state,
      ...action.data
    };

  case LOG_IN + FAILURE:
    return {
      ...state,
      response: action.data,
      isLoading: action.isLoading
    };

  case LOG_IN + SUCCESS:
    return {
      ...state,
      response: action.data
    };
  default:
    return state;
  }
}
