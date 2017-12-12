import axios from 'axios';
/* constants */
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
/* globals */
import { LOGIN } from 'app.globals';
/* helpers */
import { emailRegExp } from 'app.helpers/regexp';

const isValid = (body, dispatch) => {
  const { loginEmail, password } = body;

  if (!loginEmail.match(emailRegExp)) {
    dispatch({
      type: LOG_IN + VALIDATION_ERROR,
      isLoading: false,
      payload: {
        logonSuccess: false,
        errorMessage: LOGIN.emailValidationMessage,
        errorPropertyName: 'loginEmail'
      }
    });
    return false;
  }

  if (password.length === 0) {
    dispatch({
      type: LOG_IN + VALIDATION_ERROR,
      isLoading: false,
      payload: {
        logonSuccess: false,
        errorMessage: LOGIN.passwordValidationMessage,
        errorPropertyName: 'password'
      }
    });

    return false;
  }

  return true;
};

export const changeCredentinals = (field, value) => {
  return {
    type: CREDENTINALS_CHANGE,
    payload: {
      field,
      value
    }
  };
};

export const checkTaC = (url, body) => {
  return (dispatch) => {
    if (!isValid(body, dispatch)) return;

    dispatch({ type: TAC_CHECK + FETCH });

    axios.post(url, body)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success && payload) {
          if (payload.logonSuccess) {
            dispatch({
              type: TAC_CHECK + SUCCESS,
              payload
            });
          } else {
            dispatch({
              type: LOG_IN + FAILURE,
              alert: false,
              payload
            });
          }
        } else {
          dispatch({
            type: TAC_CHECK + FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((error) => {
        dispatch({ type: TAC_CHECK + FAILURE });
      });
  };
};

export const acceptTaC = (url, body) => {
  return (dispatch) => {
    if (!isValid(body, dispatch)) return;

    axios.post(url, body)
      .then((response) => {
        const { success, errorMessage } = response.data;
        if (success) {
          dispatch({ type: TAC_ACCEPT + SUCCESS });
        } else {
          dispatch({
            type: TAC_ACCEPT + FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((error) => {
        dispatch({ type: TAC_ACCEPT + FAILURE });
      });
  };
};

export const loginSubmit = (url, body) => {
  return (dispatch) => {
    if (!isValid(body, dispatch)) return;

    axios.post(url, body)
      .then((response) => {
        const { success, payload, errorMessage } = response.data;
        if (success && payload.logonSuccess) {
          dispatch({
            type: LOG_IN + SUCCESS,
            payload
          });
        } else if (!payload.logonSuccess) {
          dispatch({
            type: LOG_IN + FAILURE,
            alert: false,
            payload
          });
        } else {
          dispatch({
            type: LOG_IN + FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((error) => {
        dispatch({ type: LOG_IN + FAILURE });
      });
  };
};
