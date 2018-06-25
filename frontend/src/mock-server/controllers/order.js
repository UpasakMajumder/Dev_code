const order = require('../ws/order');
const campaign = require('../ws/filtered-recent-orders/campaigns');
const filteredOrders = require('../ws/filtered-recent-orders/orders');

module.exports.edit = (req, res) => {
  res.json(order.edit);
}

module.exports.detail = {
  ui: (req, res) => res.json(order.detail.ui),
  accept: (req, res) => res.json(order.detail.accept),
  reject: (req, res) => res.json(order.detail.reject),
};

module.exports.reports = {
  rows: (req, res) => {
    const { sort, page } = req.query;

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
    campaigns: (req, res) => setTimeout(() => res.json(campaign), 2000),
    orders: (req, res) => {
      filteredOrders.payload.rows[0].items[1].value = req.params.id + (req.params.campaign || 0);
      setTimeout(() => res.json(filteredOrders), 2000);
    }
  },
  requiringApproval: (req, res) => res.json(order.recent.requiringApproval),
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
