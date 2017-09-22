const settings = require('../ws/settings');

const address = {
  ui: (req, res) => res.json(settings.address.ui)
};

module.exports.address = address;
