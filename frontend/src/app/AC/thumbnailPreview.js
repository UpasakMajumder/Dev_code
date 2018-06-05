import { THUMBNAIL_PREVIEW_TOGGLE } from 'app.consts';

export default (image = '') => {
  return {
    type: THUMBNAIL_PREVIEW_TOGGLE,
    payload: {
      image
    }
  };
};
