/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, MODIFY_MAILING_LIST } from 'app.consts';
/* globals */
import { MODIFY_MAILING_LIST_UI } from 'app.globals';

export const initUI = () => {
  return (dispatch) => {
    dispatch({ type: MODIFY_MAILING_LIST + INIT_UI + FETCH });

    if (MODIFY_MAILING_LIST_UI) {
      dispatch({
        type: MODIFY_MAILING_LIST + INIT_UI + SUCCESS,
        payload: MODIFY_MAILING_LIST_UI
      });
    } else {
      dispatch({ type: MODIFY_MAILING_LIST + INIT_UI + FAILURE });
    }
  };
};

export const x = 1;
