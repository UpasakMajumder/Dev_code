const CartPreview = require('./controllers/cartPreview');
const Search = require('./controllers/search');
const Checkout = require('./controllers/checkout');
const Settings = require('./controllers/settings');
const Products = require('./controllers/products');
const Orders = require('./controllers/order');
const MailingList = require('./controllers/mailingList');
const Login = require('./controllers/login');
const CheckTaC = require('./controllers/checkTaC');
const AcceptTaC = require('./controllers/acceptTaC');

const apiRouter = require('express').Router();

apiRouter.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Headers', 'X-Requested-With');
  next();
});

apiRouter.get('/order/recent/filtered/campaigns/:selectedOrderType', Orders.recent.filtered.campaigns);
apiRouter.get('/order/recent/filtered/orders/:selectedOrderType/:selectedCampaign*?', Orders.recent.filtered.orders);

apiRouter.post('/login', Login);
apiRouter.post('/accepttac', AcceptTaC);
apiRouter.post('/checktac', CheckTaC);

apiRouter.get('/cartPreview', CartPreview);

apiRouter.get('/search/result', Search.result);
apiRouter.post('/search/query', Search.query);

apiRouter.get('/checkout/ui', Checkout.ui);
apiRouter.get('/checkout/total-price', Checkout.totalPrice);
apiRouter.post('/checkout/total-price', Checkout.totalPrice);
apiRouter.post('/checkout/change-quantity', Checkout.changeQuantity);
apiRouter.post('/checkout/remove-producs', Checkout.removeProducs);
apiRouter.post('/checkout/change-address', Checkout.changeAddress);
apiRouter.post('/checkout/change-delivery-method', Checkout.changeDeliveryMethod);
apiRouter.post('/checkout/submit', Checkout.submit);
apiRouter.post('/checkout/add-new-address', Checkout.addNewAddress);

apiRouter.get('/settings/address/ui', Settings.address.ui);
apiRouter.post('/settings/address/modify', Settings.address.modify);
apiRouter.put('/settings/address/default/set', Settings.address.defaultSet);
apiRouter.put('/settings/address/default/unset', Settings.address.defaultSet);

apiRouter.get('/products/all', Products.all);
apiRouter.get('/products/favourites', Products.favourites);
apiRouter.get('/products/managed', Products.managed);
apiRouter.put('/products/favourite/:id', Products.setFavourite);
apiRouter.put('/products/unfavourite/:id', Products.setFavourite);
apiRouter.post('/products/add-to-cart', Products.addToCart);
apiRouter.post('/products/options', Products.options);

apiRouter.get('/order/recent/ui', Orders.recent.ui);
apiRouter.get('/order/recent/page/:page', Orders.recent.page);
apiRouter.get('/order/detail', Orders.detail);

apiRouter.post('/mailing-list/use-correct/:containerId', MailingList.useCorrect);
apiRouter.post('/mailing-list/reprocess/:containerId', MailingList.reprocess);

module.exports = apiRouter;
