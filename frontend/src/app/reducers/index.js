import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import shoppingCart from './shoppingCart';
import isLoading from './isLoading';
import settingsAddresses from './settingsAddresses';
import dialog from './dialog';
import orderDetail from './orderDetail';
import searchPage from './searchPage';
import search from './search';
import isShownHeaderShadow from './headerShadow';

const rootReducer = combineReducers({
  login,
  shoppingCart,
  isLoading,
  settingsAddresses,
  dialog,
  searchPage,
  search,
  isShownHeaderShadow,
  orderDetail
});

export default rootReducer;
