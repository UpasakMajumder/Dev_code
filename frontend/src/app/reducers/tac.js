import {
  TAC_OPEN,
  TAC_CLOSE
} from 'app.consts';

const defaultState = {
  show: false,
  redirect: false,
  returnurl: ''
};

export default function login(state = defaultState, action = {}) {
  const { type, payload } = action;

  switch (type) {
  case TAC_OPEN: {
    return {
      ...state,
      show: true,
      redirect: payload.redirect,
      returnurl: payload.returnurl
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
