import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import mailing from './mailing';
import cardPayment from './card-payment';

const rootReducer = combineReducers({
  login,
  mailing,
  cardPayment
});

export default rootReducer;
