import * as constants from 'app.consts';

const defaultState = {
  response: null,
  isLoading: false
};

export default function mailing(state = defaultState, action = {}) {
  switch (action.type) {
  case constants.FETCH_SERVERS:
  case constants.FETCH_SERVERS_SUCCESS:
  case constants.FETCH_SERVERS_FAILURE:
    return {
      ...state,
      isLoading: action.isLoading
    };

  case constants.MAILING_RESPONSE_SUCCESS:
    return {
      ...state,
      response: action.data,
      isLoading: false
    };

  default:
    return state;
  }
}
