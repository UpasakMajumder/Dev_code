import { THUMBNAIL_PREVIEW_TOGGLE } from 'app.consts';

const defaultState = {
  image: '',
  show: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  if (type !== THUMBNAIL_PREVIEW_TOGGLE) return state;

  return {
    show: !state.show,
    image: payload.image
  };
};
