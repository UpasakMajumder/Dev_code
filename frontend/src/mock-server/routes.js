const OrderDetail = require('./controllers/orderDetail');
const CartPreview = require('./controllers/cartPreview');

const apiRouter = require('express').Router();

apiRouter.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Headers', 'X-Requested-With');
  next();
});


apiRouter.get('/orderDetail', OrderDetail);
apiRouter.get('/cartPreview', CartPreview);

module.exports = apiRouter;
