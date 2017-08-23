const { localization } = config; // eslint-disable-line no-undef

const {
  login,
  checkout,
  spotfire,
  userSettings,
  searchPage,
  search,
  orderDetail,
  recentOrders,
  ui,
  cartPreview,
  addToCartUrl,
  notification
} = localization;


export const LOGIN = login;
export const CHECKOUT = checkout;
export const SPOTFIRE = spotfire;
export const USER_SETTINGS = userSettings;
export const ORDER_DETAIL = orderDetail;
export const SEARCH_PAGE = searchPage;
export const SEARCH = search;
export const RECENT_ORDERS = recentOrders;
export const CART_PREVIEW = cartPreview;
export const ADD_TO_CART_URL = addToCartUrl;
export const NOTIFICATION = notification;

/* UI */
export const MODIFY_MAILING_LIST_UI = ui ? ui.modifyMailingList : {};
