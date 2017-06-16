import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import shoppingCart from './shoppingCart';
import isLoading from './isLoading';

const rootReducer = combineReducers({
  login,
  shoppingCart,
  isLoading
});

export default rootReducer;
