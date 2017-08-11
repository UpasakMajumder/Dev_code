import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import login from './login';
import checkout from './checkout';
import isLoading from './isLoading';
import settingsAddresses from './settingsAddresses';
import orderDetail from './orderDetail';
import searchPage from './searchPage';
import search from './search';
import isShownHeaderShadow from './headerShadow';
import recentOrders from './recentOrders';
import modifyMailingList from './modifyMailingList';
import cartPreview from './cartPreview';

const rootReducer = combineReducers({
  login,
  checkout,
  isLoading,
  settingsAddresses,
  searchPage,
  search,
  isShownHeaderShadow,
  orderDetail,
  recentOrders,
  modifyMailingList,
  cartPreview
});

export default rootReducer;
