/* constants */
import { SUCCESS, FAILURE, INIT_UI, MODIFY_MAILING_LIST, MODIFY_MAILING_LIST_USE_CORRECT } from 'app.consts';

const defaultState = {
  errorUI: null,
  successUI: null,
  errorList: null,
  successList: null,
  formInfo: null,
  uiFail: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case MODIFY_MAILING_LIST + INIT_UI + SUCCESS:
    return {
      ...state,
      errorUI: payload.errorUI,
      successUI: payload.successUI,
      errorList: payload.errorList,
      successList: payload.successList,
      formInfo: payload.formInfo
    };

  case MODIFY_MAILING_LIST + INIT_UI + FAILURE:
    return {
      ...state,
      uiFail: true
    };

  case MODIFY_MAILING_LIST_USE_CORRECT + SUCCESS:
    return {
      ...state,
      errorList: null
    };

  default:
    return state;
  }
};
