import { LOGIN_CLIENT_FETCH, LOGIN_CLIENT_VALIDATION_ERROR, LOGIN_CLIENT_SUCCESS, LOGIN_CLIENT_FAILURE } from 'app.consts';

const defaultState = {
  response: null,
  isLoading: false
};

export default function login(state = defaultState, action = {}) {
  switch (action.type) {
  case LOGIN_CLIENT_FETCH:
    return {
      ...state,
      isLoading: action.isLoading
    };

  case LOGIN_CLIENT_VALIDATION_ERROR:
    return {
      ...state,
      ...action.data
    };

  case LOGIN_CLIENT_FAILURE:
    return {
      ...state,
      isLoading: action.isLoading
    };

  case LOGIN_CLIENT_SUCCESS:
    return {
      ...state,
      response: action.data,
      isLoading: action.isLoading
    };
  default:
    return state;
  }
}
