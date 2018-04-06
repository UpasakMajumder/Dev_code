import axios from 'axios';
import {
  TAC_OPEN,
  TAC_CLOSE
} from 'app.consts';

export const closeTAC = () => {
  return { type: TAC_CLOSE };
};

export const openTaC = ({
  redirect,
  returnurl
}) => {
  return {
    type: TAC_OPEN,
    payload: {
      redirect,
      returnurl
    }
  };
};
