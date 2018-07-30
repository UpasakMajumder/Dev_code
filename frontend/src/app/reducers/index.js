import { combineReducers } from 'redux';
import { reducer as toastr } from 'react-redux-toastr';
import tac from './tac';
import checkout from './checkout';
import isLoading from './isLoading';
import settingsAddresses from './settingsAddresses';
import searchPage from './searchPage';
import search from './search';
import isShownHeaderShadow from './headerShadow';
import ordersReports from './ordersReports';
import manageProducts from './manageProducts';
import modifyMailingList from './modifyMailingList';
import cartPreview from './cartPreview';
import products from './products';
import recentOrders from './recentOrders';
import dialogAlert from './dialogAlert';
import cardPayment from './card-payment';
import languageSelector from './languageSelector';
import filteredRecentOrders from './filteredRecentOrders';
import thumbnailPreview from './thumbnailPreview';

const rootReducer = combineReducers({
  tac,
  checkout,
  isLoading,
  settingsAddresses,
  searchPage,
  search,
  isShownHeaderShadow,
  ordersReports,
  manageProducts,
  modifyMailingList,
  cartPreview,
  products,
  toastr,
  dialogAlert,
  cardPayment,
  languageSelector,
  filteredRecentOrders,
  recentOrders,
  thumbnailPreview
});

export default rootReducer;
