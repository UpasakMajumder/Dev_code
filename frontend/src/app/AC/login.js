import axios from 'axios';
import validator from 'validator';
import * as constants from '../constants';
import { LOGIN } from '../globals';

export default function requestLogin(loginEmail, password, isKeepMeLoggedIn) {
  return (dispatch) => {

    if (!validator.isEmail(loginEmail)) {
      dispatch({
        type: constants.LOGIN_CLIENT_VALIDATION_ERROR,
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
        type: constants.LOGIN_CLIENT_VALIDATION_ERROR,
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
      type: constants.FETCH_SERVERS,
      isLoading: true
    });

    // ToDo: Change to POST and URL
    axios.get('http://localhost:3000/loginSuccess', { loginEmail, password, isKeepMeLoggedIn })
      .then((response) => {
        dispatch({
          type: constants.FETCH_SERVERS_SUCCESS
        });

        const data = response.data.d ? response.data.d : response.data; // d prop is because of .NET
        dispatch({
          type: constants.LOGIN_RESPONSE_SUCCESS,
          data
        });
      })
      .catch(() => {
        dispatch({
          type: constants.FETCH_SERVERS_FAILURE
        });
      });
  };
}
