import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import shoppingCart from './shoppingCart';
import isLoading from './isLoading';
import settingsAddresses from './settingsAddresses';
import dialog from './dialog';

const rootReducer = combineReducers({
  login,
  shoppingCart,
  isLoading,
  settingsAddresses,
  dialog
});

export default rootReducer;
