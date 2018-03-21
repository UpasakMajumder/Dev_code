import axios from 'axios';
import {
  FAILURE,
  TAC_CHECK,
  TAC_OPEN,
  TAC_CLOSE
} from 'app.consts';

export const closeTAC = () => {
  return { type: TAC_CLOSE };
};

export const openTAC = () => {
  return { type: TAC_OPEN };
};

export const checkTaC = ({
  url,
  token,
  redirect,
  returnurl
}) => {
  return (dispatch) => {
    axios.post(url, { token })
      .then((response) => {
        const { success, payload, errorMessage } = response.data;
        if (success) {
          dispatch({
            type: TAC_CHECK,
            payload: {
              show: payload.show,
              redirect,
              returnurl,
              token
            }
          });
        } else {
          dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((e) => {
        dispatch({
          type: FAILURE,
          alert: false
        });
      });
  };
};
