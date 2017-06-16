import { DIALOG_OPEN, DIALOG_CLOSE } from '../constants';

export const openDialog = ({ headerTitle, isCloseButton, body, footer }) => {
  return (dispatch) => {
    dispatch({
      type: DIALOG_OPEN,
      payload: {
        headerTitle,
        isCloseButton,
        body,
        footer
      }
    });
  };
};

export const closeDialog = () => dispatch => dispatch({ type: DIALOG_CLOSE });
