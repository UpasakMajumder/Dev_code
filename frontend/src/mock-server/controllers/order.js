const order = require('../ws/order');

module.exports.detail = (req, res) => res.json(order.detail);

module.exports.recent = {
  header: (req, res) => res.json(order.recent)
};
