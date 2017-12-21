import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import { reducer as toastr } from 'react-redux-toastr';
import login from './login';
import checkout from './checkout';
import isLoading from './isLoading';
import settingsAddresses from './settingsAddresses';
import orderDetail from './orderDetail';
import searchPage from './searchPage';
import search from './search';
import isShownHeaderShadow from './headerShadow';
import recentOrders from './recentOrders';
import manageProducts from './manageProducts';
import modifyMailingList from './modifyMailingList';
import cartPreview from './cartPreview';
import products from './products';
import dialogAlert from './dialogAlert';
import cardPayment from './card-payment';
import languageSelector from './languageSelector';
import filteredRecentOrders from './filteredRecentOrders';

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
  manageProducts,
  modifyMailingList,
  cartPreview,
  products,
  toastr,
  dialogAlert,
  cardPayment,
  languageSelector,
  filteredRecentOrders
});

export default rootReducer;
