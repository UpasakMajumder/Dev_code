import { toastr } from 'react-redux-toastr';
/* consts */
import { FAILURE, SUCCESS } from 'app.consts';
/* globals */
import { NOTIFICATION } from 'app.globals';

export default store => next => action => { // eslint-disable-line arrow-parens
  if (action.type.includes(FAILURE) && action.alert !== false) {
    const title = action.alert || NOTIFICATION.serverNotAvailable.title;
    const text = action.alert ? '' : NOTIFICATION.serverNotAvailable.text;

    toastr.error(title, text);
  }

  if (action.type.includes(SUCCESS) && action.alert) {
    toastr.success(action.alert);
  }

  action.error && console.error(action.error); // eslint-disable-line

  next(action);
};
