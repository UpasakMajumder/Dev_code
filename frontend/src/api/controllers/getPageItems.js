const rows1 = require('../ws/recentOrders').rows1;
const rows2 = require('../ws/recentOrders').rows2;

module.exports = [
  '/getPageItems/:page',
  (req, res) => {
    res.header('Access-Control-Allow-Origin', '*');
    const { page } = req.params;
    if (page % 2 === 0) {
      res.json(rows1)
    } else {
      res.json(rows2)
    }
  }
];
