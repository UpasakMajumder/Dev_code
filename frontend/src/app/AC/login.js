import axios from 'axios';
import validator from 'validator';
import * as constants from '../constants';

export default function requestLogin(loginEmail, password, isKeepMeLoggedIn) {
  return (dispatch, state) => {

    if (!validator.isEmail(loginEmail)) {
      dispatch({
        type: constants.LOGIN_CLIENT_VALIDATION_ERROR,
        data: {
          isLoading: false,
          response: {
            success: false,
            errorMessage: 'Please check your email format.',
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
            errorMessage: 'Pleokase enter password.',
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

        dispatch({
          type: constants.LOGIN_RESPONSE_SUCCESS,
          data: response.data
        });
      })
      .catch(() => {
        dispatch({
          type: constants.FETCH_SERVERS_FAILURE
        });
      });
  };
}
