import axios from 'axios';
import validator from 'validator';
import { LOGIN_CLIENT_FETCH, LOGIN_CLIENT_SUCCESS, LOGIN_CLIENT_FAILURE, LOGIN_CLIENT_VALIDATION_ERROR } from '../constants';
import { LOGIN } from '../globals';

export default (loginEmail, password, isKeepMeLoggedIn) => {
  return (dispatch) => {
    if (!validator.isEmail(loginEmail)) {
      dispatch({
        type: LOGIN_CLIENT_VALIDATION_ERROR,
        data: {
          isLoading: false,
          response: {
            success: false,
            errorMessage: LOGIN.emailValidationMessage,
            errorPropertyName: 'loginEmail'
          }
        }
      });

      return;
    }

    if (password.length === 0) {
      dispatch({
        type: LOGIN_CLIENT_VALIDATION_ERROR,
        data: {
          isLoading: false,
          response: {
            success: false,
            errorMessage: LOGIN.passwordValidationMessage,
            errorPropertyName: 'password'
          }
        }
      });

      return;
    }

    dispatch({
      type: LOGIN_CLIENT_FETCH,
      isLoading: true
    });

    // ToDo: Change to POST and URL
    axios.post('/KadenaWebService.asmx/LogonUser', { loginEmail, password, isKeepMeLoggedIn })
      .then((response) => {
        const data = response.data.d ? response.data.d : response.data; // d prop is because of .NET
        dispatch({
          type: LOGIN_CLIENT_SUCCESS,
          data,
          isLoading: false
        });
      })
      .catch((error) => {
        dispatch({
          type: LOGIN_CLIENT_FAILURE,
          isLoading: false
        });
        alert(error); // eslint-disable-line no-alert
      });
  };
};
