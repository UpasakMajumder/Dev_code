import {
  APP_LOADING,
  START,
  FINISH
} from 'app.consts';

export const start = () => ({ type: APP_LOADING + START });
export const finish = () => ({ type: APP_LOADING + FINISH });
