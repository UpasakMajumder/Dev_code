const {
  getTotals
} = require('../ws/checkoutRefactor');

module.exports.removeProduct = (req, res) => {
  res.json({
    success: true,
    payload: {
      quantityText: 'You have 1 item in your shopping cart'
    },
    errorMessage: ''
  });
};

module.exports.changeProductQuantity = (req, res) => {
  res.json({
    success: true,
    errorMessage: ''
  });
};

module.exports.getTotals = (req, res) => {
  setTimeout(() => {
    res.json(getTotals()); // func to generate response every request
  }, 2000);
};
