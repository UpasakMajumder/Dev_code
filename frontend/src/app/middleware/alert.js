import { toastr } from 'react-redux-toastr';
/* consts */
import { FAILURE } from 'app.consts';
/* globals */
import { NOTIFICATION } from 'app.globals';

export default store => next => action => { // eslint-disable-line arrow-parens
  if (action.type.includes(FAILURE) && action.alert !== false) {
    const title = action.alert || NOTIFICATION.serverNotAvailable.title;
    const text = action.alert ? '' : NOTIFICATION.serverNotAvailable.text;

    toastr.error(title, text);
  }

  next(action);
};
