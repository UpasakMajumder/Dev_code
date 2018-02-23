const products = require('../ws/products');

module.exports.all = (req, res) => res.json(products.all);
module.exports.favourites = (req, res) => res.json(products.favourites);
module.exports.managed = (req, res) => res.json(products.managed);
module.exports.setFavourite = (req, res) => res.json({ success: true });
module.exports.addToCart = (req, res) => res.json(products.addToCart);
module.exports.options = (req, res) => {
  const payload = {
    priceValue: Math.random(),
    pricePrefix: '$'
  };

  res.json({
    success: true,
    errorMessage: null,
    payload
  });
};
