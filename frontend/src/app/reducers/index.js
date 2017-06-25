import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import shoppingCart from './shoppingCart';
import isLoading from './isLoading';
import settingsAddresses from './settingsAddresses';
import orderDetail from './orderDetail';
import searchPage from './searchPage';
import search from './search';
import isShownHeaderShadow from './headerShadow';
import recentOrders from './recentOrders';

const rootReducer = combineReducers({
  login,
  shoppingCart,
  isLoading,
  settingsAddresses,
  searchPage,
  search,
  isShownHeaderShadow,
  orderDetail,
  recentOrders
});

export default rootReducer;
