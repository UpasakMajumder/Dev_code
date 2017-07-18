/* libraries */
import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, MODIFY_MAILING_LIST, MODIFY_MAILING_LIST_USE_CORRECT, APP_LOADING, START, FINISH } from 'app.consts';
/* globals */
import { MODIFY_MAILING_LIST_UI, MODIFY_MAILING_LIST as MODIFY_MAILING_LIST_URL } from 'app.globals';
/* helpers */
import removeProps from 'app.helpers/object';

export const initUI = () => {
  return (dispatch) => {
    dispatch({ type: MODIFY_MAILING_LIST + INIT_UI + FETCH });

    if (MODIFY_MAILING_LIST_UI) {
      const errorUI = removeProps(MODIFY_MAILING_LIST_UI.errorList, ['items']);
      const successUI = removeProps(MODIFY_MAILING_LIST_UI.successList, ['items']);
      const errorList = MODIFY_MAILING_LIST_UI.errorList.items;
      const successList = MODIFY_MAILING_LIST_UI.successList.items;
      const formInfo = MODIFY_MAILING_LIST_UI.formInfo;

      dispatch({
        type: MODIFY_MAILING_LIST + INIT_UI + SUCCESS,
        payload: {
          errorUI,
          successUI,
          errorList,
          successList,
          formInfo
        }
      });
    } else {
      dispatch({ type: MODIFY_MAILING_LIST + INIT_UI + FAILURE });
    }
  };
};

export const useCorrect = (id) => {
  return (dispatch) => {
    dispatch({ type: MODIFY_MAILING_LIST_USE_CORRECT + FETCH });
    dispatch({ type: APP_LOADING + START });

    const prod = () => {
      axios({
        method: 'get',
        url: `${MODIFY_MAILING_LIST_URL.useCorrectUrl}/${id}`
      }).then((response) => {
        const { success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: MODIFY_MAILING_LIST_USE_CORRECT + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
          dispatch({ type: APP_LOADING + FINISH });
        } else {
          dispatch({ type: MODIFY_MAILING_LIST_USE_CORRECT + SUCCESS });
          dispatch({ type: APP_LOADING + FINISH });
        }
      })
        .catch((error) => {
          dispatch({ type: MODIFY_MAILING_LIST_USE_CORRECT + FAILURE });
          dispatch({ type: APP_LOADING + FINISH });
          alert(error); // eslint-disable-line no-alert
        });
    };

    const dev = () => setTimeout(() => {
      dispatch({ type: MODIFY_MAILING_LIST_USE_CORRECT + SUCCESS });
      dispatch({ type: APP_LOADING + FINISH });
    }, 2000);

    // dev();
    prod();
  };
};
