/**
 * https://github.com/Keyamoon/svgxuse
 * If you do not use SVG <use xlink:href="…"> elements, remove svgxuse module
 */
import 'svgxuse';
import { toastr } from 'react-redux-toastr';
import configureStore from './store';
import { init, render } from './init';

const app = {
  run() {
    this.react();
    this.static();
  },

  /* Static JavaScript classes */
  static() {
    init('confirmation', document.getElementsByClassName('js-confirmation'));
    init('storage', document.getElementsByClassName('js-storage'));
    init('spotfire', document.getElementsByClassName('js-spotfire'));
    // init('money-format', document.getElementsByClassName('js-money-format'));
    init('tabs', document.getElementsByClassName('js-tabs'));
    init('collapse', document.getElementsByClassName('js-collapse'));
    init('tooltip', document.getElementsByClassName('js-tooltip'));
    init('sidebar', document.getElementsByClassName('js-sidebar'));
    init('drop-zone', document.getElementsByClassName('js-drop-zone'));
    init('dialog', document.getElementsByClassName('js-dialog'));
    init('add-tr', document.getElementsByClassName('js-add-tr'));
    init('redirection', document.getElementsByClassName('js-redirection'));
    init('password', document.getElementsByClassName('js-password'));
    init('closer', document.getElementsByClassName('js-close-this'));
    init('datepicker', document.getElementsByClassName('js-datepicker'));
    init('replace-value', document.getElementsByClassName('js-replace-value'));
    init('table-paginator', document.getElementsByClassName('js-table-paginator'));
    init('add-to-cart', document.getElementsByClassName('js-add-to-cart'));
    init('chili-editor', document.getElementsByClassName('js-chili-editor'));
    init('product-view', document.getElementsByClassName('js-product-view'));
    init('cart-preview', document.getElementsByClassName('js-cart-preview'));
    init('product-options', document.getElementsByClassName('js-product-options'));
  },

  /* React */
  react() {
    /* Configure Redux store */
    window.store = configureStore();
    render('StyleguideInput', document.querySelectorAll('.styleguide-input'), { store: false });
    render('Login', document.querySelectorAll('.js-login'));
    render('Payment', document.querySelectorAll('.js-credit-card-payment'));
    render('Checkout', document.querySelectorAll('#r-shopping-cart'));
    render('GlobalSpinner', document.querySelectorAll('.r-spinner'));
    render('Settings/Addresses', document.querySelectorAll('.r-settings-addresses'));
    // render('Settings/Languages', document.querySelectorAll('.r-settings-languages'));
    render('OrderDetail', document.querySelectorAll('.r-order-detail'));
    render('SearchPage/Products', document.querySelectorAll('.r-search-page-products'));
    render('SearchPage/Pages', document.querySelectorAll('.r-search-page-pages'));
    render('Search', document.querySelectorAll('.r-search'));
    render('ManageProducts', document.querySelectorAll('.r-manage-products'));
    render('HeaderShadow', document.querySelectorAll('.r-header-shadow'));
    render('OrdersReports', document.querySelectorAll('.r-orders-reports'));
    render('RecentOrders', document.querySelectorAll('.r-recent-orders'));
    render('ModifyMailingList', document.querySelectorAll('.r-modify-mlist'));
    render('CartPreview', document.querySelectorAll('.r-cart-preview'));
    render('CartItems', document.querySelectorAll('.r-cart-items'));
    render('Toastr', document.querySelectorAll('.r-toastr'));
    render('ToastrTest', document.querySelectorAll('.r-toastr-test'));
    render('Products/All', document.querySelectorAll('.r-products'));
    render('Products/Favorites', document.querySelectorAll('.r-products-favorites'));
    render('LanguageSelector', document.querySelectorAll('.r-language-selector'));
    render('TaC', document.querySelectorAll('.r-tac'));
    render('FilteredRecentOrders', document.querySelectorAll('.r-filtered-recent-orders'));
    render('Registration', document.querySelectorAll('.r-registration'));
  }
};

/* Global scope reference */
window.app = app;
window.toastr = toastr;

/* Run */
app.run();
