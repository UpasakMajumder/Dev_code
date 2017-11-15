import { SHOW, HIDE, HEADER_SHADOW, LANGUAGES } from 'app.consts';

export const showLanguages = (headerShadow) => {
  return (dispatch) => {
    dispatch({ type: LANGUAGES + SHOW });
    if (headerShadow) dispatch({ type: HEADER_SHADOW + SHOW });
  };
};

export const hideLanguages = (headerShadow) => {
  return (dispatch) => {
    dispatch({ type: LANGUAGES + HIDE });
    if (headerShadow) dispatch({ type: HEADER_SHADOW + HIDE });
  };
};
