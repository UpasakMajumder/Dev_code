const order = require('../ws/order');
const campaign = require('../ws/filtered-recent-orders/campaigns');
const filteredOrders = require('../ws/filtered-recent-orders/orders');

module.exports.detail = (req, res) => res.json(order.detail);

module.exports.reports = {
  rows: (req, res) => {
    const { page } = req.params;
    const { sort } = req.query;

    if (page) {
      if (page % 2 === 0 || page === undefined) {
        res.json(order.reports.rows1);
      } else {
        res.json(order.reports.rows2);
      }
    } else if (sort) {
      if (sort.includes('ASC')) {
        res.json(order.reports.rows2);
      } else {
        res.json(order.reports.rows1);
      }
    } else {
      res.json(order.reports.rows1);
    }
  }
};

module.exports.recent = {
  filtered: {
    campaigns: (req, res) => res.json(campaign),
    orders: (req, res) => res.json(filteredOrders)
  }
};
