import { DIALOG_ALERT, TOGGLE } from 'app.consts';

const defaultState = {
  visible: false,
  message: '',
  closeDialog: null,
  btns: []
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case DIALOG_ALERT + TOGGLE:
    return {
      ...state,
      visible: !!payload.visible,
      message: payload.message || '',
      closeDialog: payload.closeDialog || null,
      btns: payload.btns || []
    };

  default:
    return state;
  }
};
