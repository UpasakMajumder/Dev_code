import { HEADER_SHADOW_SHOW, HEADER_SHADOW_HIDE } from '../constants';

export default (state = false, action) => {
  const { type } = action;

  switch (type) {
  case HEADER_SHADOW_SHOW:
    return true;

  case HEADER_SHADOW_HIDE:
    return false;

  default:
    return state;
  }
};
