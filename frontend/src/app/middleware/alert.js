/* consts */
import { FAILURE } from 'app.consts';
/* globals */
import { NOTIFICATION } from 'app.globals';

export default store => next => action => { // eslint-disable-line arrow-parens
  if (action.type.includes(FAILURE) && action.alert !== false) {
    let title = NOTIFICATION.serverNotAvailable.title;
    let text = NOTIFICATION.serverNotAvailable.text;

    if (action.alert) {
      title = action.alert;
      text = '';
    }

    window.toastr.error(title, text);
  }

  next(action);
};
