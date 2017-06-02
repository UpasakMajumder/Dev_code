import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import shoppingCart from './shoppingCart';

const rootReducer = combineReducers({
  login,
  shoppingCart
});

export default rootReducer;
