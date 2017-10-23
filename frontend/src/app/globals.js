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
  checkoutButtons,
  manageProducts,
  modifyMailingList,
  cartPreview,
  addToCartUrl,
  products,
  notification,
  chiliSave,
  languages,
  pagination,
  staticFields,
  locale
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
export const PRODUCTS = products;
export const NOTIFICATION = notification;
export const CHILI_SAVE = chiliSave;
export const MANAGE_PRODUCTS = manageProducts;
export const MODIFY_MAILING_LIST_UI = modifyMailingList;
export const BUTTONS_UI = checkoutButtons;
export const LANGUAGES = languages;
export const PAGINATION = pagination;
export const STATIC_FIELDS = staticFields;
export const LOCALE = locale || 'en-gb';
