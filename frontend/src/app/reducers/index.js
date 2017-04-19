import { combineReducers } from 'redux';
import login from './login';
import mailing from './mailing';

const rootReducer = combineReducers({
  login,
  mailing
});

export default rootReducer;
