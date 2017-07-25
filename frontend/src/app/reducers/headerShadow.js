import { HEADER_SHADOW, SHOW, HIDE } from 'app.consts';

export default (state = false, action) => {
  const { type } = action;

  switch (type) {
  case HEADER_SHADOW + SHOW:
    return true;

  case HEADER_SHADOW + HIDE:
    return false;

  default:
    return state;
  }
};
