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
