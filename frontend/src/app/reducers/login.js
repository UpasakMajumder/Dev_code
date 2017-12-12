import {
  LOG_IN,
  VALIDATION_ERROR,
  CREDENTINALS_CHANGE,
  FETCH,
  SUCCESS,
  FAILURE,
  TAC_CHECK,
  TAC_ACCEPT
} from 'app.consts';

const defaultState = {
  credentinals: {
    loginEmail: '',
    password: '',
    isKeepMeLoggedIn: false
  },
  checkTaC: {
    showTaC: false,
    isAsked: false,
    url: ''
  },
  acceptTaC: false,
  submit: {
    logonSuccess: false,
    errorPropertyName: null,
    errorMessage: null,
    isAsked: false
  },
  isLoading: false
};

export default function login(state = defaultState, action = {}) {
  const { type, payload } = action;

  switch (type) {
  case CREDENTINALS_CHANGE:
    return {
      ...state,
      credentinals: {
        ...state.credentinals,
        [payload.field]: payload.value
      }
    };

  case TAC_ACCEPT + SUCCESS:
    return {
      ...state,
      checkTaC: {
        ...state.checkTaC,
        showTaC: false
      },
      acceptTaC: true
    };

  case TAC_CHECK + SUCCESS:
    return {
      ...state,
      checkTaC: {
        ...payload.checkTaC,
        isAsked: true
      }
    };

  case TAC_CHECK + FETCH:
    return {
      ...state,
      isLoading: true,
      submit: {
        ...state.submit,
        isAsked: false
      }
    };

  case TAC_ACCEPT + FAILURE:
  case TAC_CHECK + FAILURE:
    return {
      ...state,
      isLoading: false
    };

  case LOG_IN + VALIDATION_ERROR:
  case LOG_IN + FAILURE:
    return {
      ...defaultState,
      submit: {
        ...payload,
        isAsked: false
      }
    };

  case LOG_IN + SUCCESS:
    return {
      ...state,
      isLoading: false,
      submit: {
        ...payload,
        isAsked: true
      }
    };

  default:
    return state;
  }
}
