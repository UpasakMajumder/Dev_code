import { SHOW, HIDE, LANGUAGES } from 'app.consts';

export default (state = false, action) => {
  const { type } = action;

  switch (type) {

  case LANGUAGES + HIDE:
    return false;

  case LANGUAGES + SHOW:
    return true;

  default:
    return state;
  }
};
