const search = require('../ws/search');

module.exports.result = (req, res) => res.json(search.result);
module.exports.query = (req, res) => res.json(search.query);
