const CartPreview = require('./controllers/cartPreview');
const Search = require('./controllers/search');
const Checkout = require('./controllers/checkout');
const Settings = require('./controllers/settings');
const Products = require('./controllers/products');
const Orders = require('./controllers/order');

const apiRouter = require('express').Router();

apiRouter.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Headers', 'X-Requested-With');
  next();
});


apiRouter.get('/cartPreview', CartPreview);

apiRouter.get('/search/page', Search.searchPage);

apiRouter.get('/checkout/ui', Checkout.ui);
apiRouter.get('/checkout/total-price', Checkout.totalPrice);

apiRouter.get('/settings/address/ui', Settings.address.ui);
apiRouter.post('/settings/address/modify', Settings.address.modify);

apiRouter.get('/products/all', Products.all);
apiRouter.get('/products/favourites', Products.favourites);
apiRouter.get('/products/managed', Products.managed);

apiRouter.get('/order/recent/ui', Orders.recent.ui);
apiRouter.get('/order/recent/page/:page', Orders.recent.page);
apiRouter.get('/order/detail', Orders.detail);

module.exports = apiRouter;
