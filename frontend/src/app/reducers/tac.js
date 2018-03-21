import {
  TAC_CHECK,
  TAC_OPEN,
  TAC_CLOSE
} from 'app.consts';

const defaultState = {
  show: false,
  redirect: false,
  returnurl: '',
  isChecked: false,
  token: ''
};

export default function login(state = defaultState, action = {}) {
  const { type, payload } = action;

  switch (type) {
  case TAC_CHECK: {
    return {
      ...state,
      show: payload.show,
      redirect: payload.redirect,
      returnurl: payload.returnurl,
      token: payload.token,
      isChecked: true
    };
  }

  case TAC_OPEN: {
    return {
      ...state,
      show: true
    };
  }

  case TAC_CLOSE: {
    return {
      ...state,
      show: false
    };
  }

  default:
    return state;
  }
}
