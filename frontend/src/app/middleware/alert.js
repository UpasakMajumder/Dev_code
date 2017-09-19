import { toastr } from 'react-redux-toastr';
/* consts */
import { FAILURE } from 'app.consts';
/* globals */
import { NOTIFICATION } from 'app.globals';

export default store => next => action => { // eslint-disable-line arrow-parens
  if (action.type.includes(FAILURE) && action.alert) {
    const title = action.alert;
    const text = '';

    // let title = NOTIFICATION.serverNotAvailable.title;
    // let text = NOTIFICATION.serverNotAvailable.text;

    toastr.error(title, text);
  }

  next(action);
};
