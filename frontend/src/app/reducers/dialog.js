import { DIALOG_OPEN, DIALOG_CLOSE } from '../constants';

const defaultState = {
  isOpen: false,
  payload: {}
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case DIALOG_CLOSE:
    return defaultState;

  case DIALOG_OPEN:
    return {
      isOpen: true,
      ...payload
    };

  default:
    return state;
  }
};
