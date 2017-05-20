import { CARD_VALIDATION_ERROR, SUBMIT_CARD } from '../constants';

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

  case SUBMIT_CARD:
    return {
      ...state,
      ...defaultState,
      isProceeded: true
    };

  default:
    return state;
  }
};
