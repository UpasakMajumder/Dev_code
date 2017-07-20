/* libraries */
import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, MODIFY_MAILING_LIST, MODIFY_MAILING_LIST_USE_CORRECT, APP_LOADING,
  START, FINISH, MODIFY_MAILING_LIST_REPROCESS, MODIFY_MAILING_LIST_SHOW_VALIDATION_ERRORS } from 'app.consts';
/* globals */
import { MODIFY_MAILING_LIST_UI } from 'app.globals';
/* helpers */
import removeProps from 'app.helpers/object';

export const initUI = (containerId) => {
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
          formInfo,
          containerId
        }
      });
    } else {
      dispatch({ type: MODIFY_MAILING_LIST + INIT_UI + FAILURE });
    }
  };
};

export const useCorrect = (id, url) => {
  return (dispatch) => {
    dispatch({ type: MODIFY_MAILING_LIST_USE_CORRECT + FETCH });
    dispatch({ type: APP_LOADING + START });

    const prod = () => {
      axios({
        method: 'post',
        url: `${url}/${id}`
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

    dev();
    // prod();
  };
};

export const reprocessAddresses = (id, url, data) => {
  return (dispatch) => {
    dispatch({ type: APP_LOADING + START });
    dispatch({ type: MODIFY_MAILING_LIST_REPROCESS + FETCH });

    const prod = () => {
      axios({
        method: 'post',
        url: `${url}/${id}`,
        data: { data }
      }).then((response) => {
        const { success, errorMessage } = response.data;
        if (!success) {
          dispatch({ type: MODIFY_MAILING_LIST_REPROCESS + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
          dispatch({ type: APP_LOADING + FINISH });
        } else {
          dispatch({ type: MODIFY_MAILING_LIST_REPROCESS + SUCCESS });
          dispatch({ type: APP_LOADING + FINISH });
        }
      })
        .catch((error) => {
          dispatch({ type: MODIFY_MAILING_LIST_REPROCESS + FAILURE });
          dispatch({ type: APP_LOADING + FINISH });
          alert(error); // eslint-disable-line no-alert
        });
    };

    const dev = () => setTimeout(() => {
      dispatch({ type: MODIFY_MAILING_LIST_REPROCESS + SUCCESS });
      dispatch({ type: APP_LOADING + FINISH });
    }, 2000);

    dev();
    // prod();
  };
};

export const validationErrors = (emptyFields) => {
  return (dispatch) => {
    dispatch({
      type: MODIFY_MAILING_LIST_SHOW_VALIDATION_ERRORS,
      payload: { emptyFields }
    });
  };
};
