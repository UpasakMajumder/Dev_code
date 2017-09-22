const order = require('../ws/order');

module.exports.detail = (req, res) => res.json(order.detail);

module.exports.recent = {
  ui: (req, res) => res.json(order.recent.ui),
  page: (req, res) => {
    const { page } = req.params;
    if (page % 2 === 0) {
      res.json(order.recent.page1);
    } else {
      res.json(order.recent.page2);
    }
  }
};
