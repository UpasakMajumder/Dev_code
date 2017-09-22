const settings = require('../ws/settings');
const address = {};

address.ui = (req, res) => res.json(settings.address.ui);

address.modify = (req, res) => {
  res.json({
    success: true,
    errorMessage: null,
    payload: req.body
  });
};

module.exports.address = address;
