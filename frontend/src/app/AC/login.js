import axios from 'axios';
/* constants */
import { LOG_IN, VALIDATION_ERROR, FETCH, SUCCESS, FAILURE } from 'app.consts';
/* globals */
import { LOGIN } from 'app.globals';
/* helpers */
import { emailRegExp } from 'app.helpers/regexp';

export default (loginEmail, password, isKeepMeLoggedIn) => {
  return (dispatch) => {
    if (!loginEmail.match(emailRegExp)) {
      dispatch({
        type: LOG_IN + VALIDATION_ERROR,
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
        type: LOG_IN + VALIDATION_ERROR,
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
      type: LOG_IN + FETCH,
      isLoading: true
    });

    // ToDo: Change to POST and URL
    axios.post('/KadenaWebService.asmx/LogonUser', { loginEmail, password, isKeepMeLoggedIn })
      .then((response) => {
        const data = response.data.d ? response.data.d : response.data; // d prop is because of .NET
        if (data.success) {
          dispatch({
            type: LOG_IN + SUCCESS,
            data
          });
        } else {
          dispatch({
            type: LOG_IN + FAILURE,
            isLoading: false,
            data
          });
        }
      })
      .catch((error) => {
        dispatch({
          type: LOG_IN + FAILURE,
          isLoading: false
        });
        alert(error); // eslint-disable-line no-alert
      });
  };
};
