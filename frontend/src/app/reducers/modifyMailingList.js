/* constants */
import { SUCCESS, FAILURE, INIT_UI, MODIFY_MAILING_LIST, MODIFY_MAILING_LIST_SHOW_VALIDATION_ERRORS,
  MODIFY_MAILING_LIST_USE_CORRECT, MODIFY_MAILING_LIST_REPROCESS } from 'app.consts';

const defaultState = {
  containerId: null,
  errorUI: null,
  successUI: null,
  errorList: null,
  successList: null,
  formInfo: null,
  uiFail: false,
  canReprocess: false,
  emptyFields: {}
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
      formInfo: payload.formInfo,
      containerId: payload.containerId
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

  case MODIFY_MAILING_LIST_REPROCESS + SUCCESS:
    return {
      ...state,
      canReprocess: true
    };

  case MODIFY_MAILING_LIST_SHOW_VALIDATION_ERRORS:
    return {
      ...state,
      emptyFields: payload.emptyFields
    };


  default:
    return state;
  }
};
