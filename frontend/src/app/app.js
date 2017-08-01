/**
 * https://github.com/Keyamoon/svgxuse
 * If you do not use SVG <use xlink:href="…"> elements, remove svgxuse module
 */
import 'svgxuse';
import configureStore from './store';
import { init, render } from './init';

const app = {
  run() {
    this.static();
    this.react();
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
    init('chili-editor', document.getElementsByClassName('js-chili-editor'));
  },

  /* React */
  react() {
    /* Configure Redux store */
    window.store = configureStore();
    render('StyleguideInput', document.querySelectorAll('.styleguide-input'), { store: false });
    render('Login', document.querySelectorAll('.js-login'));
    render('Checkout', document.querySelectorAll('#r-shopping-cart'));
    render('GlobalSpinner', document.querySelectorAll('.r-spinner'));
    render('Settings/Addresses', document.querySelectorAll('.r-settings-addresses'));
    render('OrderDetail', document.querySelectorAll('.r-order-detail'));
    render('SearchPage/Products', document.querySelectorAll('.r-search-page-products'));
    render('SearchPage/Pages', document.querySelectorAll('.r-search-page-pages'));
    render('Search', document.querySelectorAll('.r-search'));
    render('HeaderShadow', document.querySelectorAll('.r-header-shadow'));
    render('RecentOrders', document.querySelectorAll('.r-recent-orders'));
    render('ModifyMailingList', document.querySelectorAll('.r-modify-mlist'));
    render('CartPreview', document.querySelectorAll('.r-cart-preview'));
    render('CartItems', document.querySelectorAll('.r-cart-items'));
  }
};

/* Global scope reference */
window.app = app;

/* Run */
app.run();
