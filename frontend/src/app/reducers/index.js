import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import mailing from './mailing';

const rootReducer = combineReducers({
  login,
  mailing,
  router: routerReducer
});

export default rootReducer;
