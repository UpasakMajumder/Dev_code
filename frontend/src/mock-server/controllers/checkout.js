const {
  emptyCart,
  products,
  deliveryAddresses,
  paymentMethods,
  submit,
  emailConfirmation,
  validationMessage,
  wrapper,
  deliveryMethods,
  totals,
  redirectURL
 } = require('../ws/checkout');

module.exports.ui = (req, res) => {
  const response = Object.assign({}, wrapper);
  response.payload = {
    emptyCart,
    products,
    deliveryAddresses,
    paymentMethods,
    submit,
    emailConfirmation,
    validationMessage
  };
  res.json(response);
};

module.exports.totalPrice = (req, res) => {
  const response = Object.assign({}, wrapper);
  response.payload = {
    deliveryMethods,
    totals
  };
  res.json(response);
};

module.exports.changeQuantity = (req, res) => {
  const response = Object.assign({}, wrapper);
  const { body: { id, quantity } } = req;
  const items = products.items.map((product) => {
    return {
      ...product,
      quantity: product.id == id ? quantity : product.quantity
    };
  });

  response.payload = {
    products: { ...products, items }
  };
  res.json(response);
};

module.exports.removeProducs = (req, res) => {
  const response = Object.assign({}, wrapper);
  const { body: { id } } = req;
  const items = products.items.filter(product => product.id != id);

  response.payload = {
    products: { ...products, items }
  };
  res.json(response);
};

module.exports.changeAddress = (req, res) => {
  const response = Object.assign({}, wrapper);
  const { body: { id } } = req;

  const items = deliveryAddresses.items.map((deliveryAddress) => {
    return {
      ...deliveryAddress,
      checked: deliveryAddress.id == id
    };
  });

  response.payload = {
    deliveryAddresses: { ...deliveryAddresses, items }
  };
  res.json(response);
};

module.exports.changeDeliveryMethod = (req, res) => {
  const response = Object.assign({}, wrapper);
  const { body: { id } } = req;

  const items = deliveryMethods.items.map((deliveryMethod) => {
    return {
      ...deliveryMethods,
      checked: deliveryMethod.id == id
    };
  });

  response.payload = {
    deliveryMethods: { ...deliveryMethods, items },
    totals
  };
  res.json(response);
};

module.exports.submit = (req, res) => {
  const response = Object.assign({}, wrapper);
  response.payload = {
    redirectURL
  };
  res.json(response);
};

module.exports.addNewAddress = (req, res) => {
  const response = Object.assign({}, wrapper);
  const { body } = req;
  response.payload = body;
  res.json(response);
};
