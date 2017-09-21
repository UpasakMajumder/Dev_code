const cartPreviewUI = require('./ws/cartPreviewUI');
const orderDetail = require('./ws/orderDetail');
const searchPageUI = require('./ws/searchPageUI');
const manageProductsUI = require('./ws/manageProductsUI');
const settingsUI = require('./ws/settingsUI');
const getHeaders = require('./ws/recentOrders').headers;
const products = require('./ws/products');
const productsFavorite = require('./ws/productsFavorite');
const checkoutUI = require('./ws/checkoutUI').checkoutUI;
const checkoutPriceUI = require('./ws/checkoutUI').checkoutPriceUI;

module.exports = () => {
    const loginSuccess = {
      success: true
    };

    const loginError = {
      success: false,
      errorMessage: 'Email address is not exists.',
      errorPropertyName: 'loginEmail'
    };

    const mailingSuccess = {
      success: true
    };

    return {
      loginSuccess,
      loginError,
      mailingSuccess,
      cartPreviewUI,
      searchPageUI,
      manageProductsUI,
      settingsUI,
      getHeaders,
      products,
      productsFavorite,
      checkoutUI,
      checkoutPriceUI
    };
};
