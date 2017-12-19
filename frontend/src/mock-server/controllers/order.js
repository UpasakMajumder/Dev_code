const order = require('../ws/order');
const campaign = require('../ws/filtered-recent-orders/campaigns');
const filteredOrders = require('../ws/filtered-recent-orders/orders');

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
  },
  filtered: {
    campaigns: (req, res) => res.json(campaign),
    orders: (req, res) => res.json(filteredOrders)
  }
};
