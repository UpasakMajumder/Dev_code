import { START, FINISH, APP_LOADING } from 'app.consts';

export default function mailing(state = false, action) {
  const { type } = action;

  switch (type) {
  case APP_LOADING + START:
    return true;

  case APP_LOADING + FINISH:
    return false;

  default:
    return state;
  }
}
