import { CARD_VALIDATION_ERROR, SUBMIT_CARD } from '../constants';

const defaultState = {
  errorField: '',
  errorMessage: ''
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
    case CARD_VALIDATION_ERROR:
    const { errorField, errorMessage } = payload;
      return {
        ...state,
        errorField,
        errorMessage
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
