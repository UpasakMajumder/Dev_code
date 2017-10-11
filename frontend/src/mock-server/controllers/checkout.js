const checkout = require('../ws/checkout');

module.exports.ui = (req, res) => res.json(checkout.ui);

module.exports.totalPrice = (req, res) => res.json(checkout.totalPrice);
