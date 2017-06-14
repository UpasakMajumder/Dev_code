import { APP_LOADING_START, APP_LOADING_FINISH } from '../constants';

export default function mailing(state = false, action) {
  const { type } = action;

  switch (type) {
  case APP_LOADING_START:
    return true;

  case APP_LOADING_FINISH:
    return false;

  default:
    return state;
  }
}
