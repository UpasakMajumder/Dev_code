import { DIALOG_ALERT, TOGGLE, HEADER_SHADOW, SHOW, HIDE, isDevelopment } from 'app.consts';

export const callAC = (dev, prod) => {
  if (isDevelopment) {
    dev();
  } else {
    prod();
  }
};

export const toggleDialogAlert = (visible, message, closeDialog, btns) => {
  const dispatch = window.store.dispatch;

  dispatch({
    type: DIALOG_ALERT + TOGGLE,
    payload: {
      visible, message, closeDialog, btns
    }
  });

  if (visible) {
    dispatch({ type: HEADER_SHADOW + SHOW });
  } else {
    dispatch({ type: HEADER_SHADOW + HIDE });
  }
};

export const bla = 1;
