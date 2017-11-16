import { CARD_VALIDATION_ERROR, SUBMIT_CARD, FETCH, FAILURE } from '../constants';

const defaultState = {
  errorField: '',
  errorMessage: '',
  isProceeded: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case CARD_VALIDATION_ERROR:
    return {
      ...state,
      errorField: payload.errorField,
      errorMessage: payload.errorMessage
    };

  case SUBMIT_CARD + FETCH:
    return {
      ...state,
      ...defaultState,
      isProceeded: true
    };

  case SUBMIT_CARD + FAILURE:
    return {
      ...state,
      ...defaultState,
      isProceeded: false
    };

  default:
    return state;
  }
};
