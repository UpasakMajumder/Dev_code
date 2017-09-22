const settings = require('../ws/settings');
const address = {};

address.ui = (req, res) => res.json(settings.address.ui);

address.modify = (req, res) => {
  const { body } = req;

  if (body.id === -1) body.id = Date.now();

  res.json({
    success: true,
    errorMessage: null,
    payload: req.body
  });
};

module.exports.address = address;
