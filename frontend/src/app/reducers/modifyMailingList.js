/* constants */
import { SUCCESS, FAILURE, INIT_UI, MODIFY_MAILING_LIST } from 'app.consts';

const defaultState = {
  ui: null,
  uiFail: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case MODIFY_MAILING_LIST + INIT_UI + SUCCESS:
    return {
      ...state,
      ui: payload
    };

  case MODIFY_MAILING_LIST + INIT_UI + FAILURE:
    return {
      ...state,
      uiFail: true
    };

  default:
    return state;
  }
};
