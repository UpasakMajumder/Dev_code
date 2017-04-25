import * as constants from '../constants';

const defaultState = {
  response: null,
  isLoading: false
};

export default function login(state = defaultState, action = {}) {
  switch (action.type) {
  case constants.FETCH_SERVERS:
    return {
      ...state,
      isLoading: action.isLoading
    };

  case constants.LOGIN_CLIENT_VALIDATION_ERROR:
    return {
      ...state,
      ...action.data
    };

  case constants.LOGIN_RESPONSE_SUCCESS:
    return {
      ...state,
      response: action.data,
      isLoading: false
    };
  default:
    return state;
  }
}
