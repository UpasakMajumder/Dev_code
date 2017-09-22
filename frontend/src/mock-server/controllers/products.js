const products = require('../ws/products');

module.exports.all = (req, res) => res.json(products.all);
module.exports.favourites = (req, res) => res.json(products.favourites);
module.exports.managed = (req, res) => res.json(products.managed);
