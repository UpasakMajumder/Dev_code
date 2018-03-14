const generatePrice = () => '$ ' + Math.random().toString().substr(0, 5);

module.exports.getTotals = () => ({
  success: true,
  errorMessage: null,
  payload: {
    totals: [
      {
        id: "summary",
        "value": generatePrice()
      },
      {
        id: "shipping",
        "value": generatePrice()
      },
      {
        id: "subtotal",
        "value": generatePrice()
      },
      {
        id: "tax",
        "value": generatePrice()
      },
      {
        id: "totals",
        "value": generatePrice()
      }
    ]
  }
});
