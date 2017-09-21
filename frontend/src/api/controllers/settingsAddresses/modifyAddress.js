module.exports = [
  '/settings/address/modify',
  (req, res) => {
    res.header('Access-Control-Allow-Origin', '*');

    const json = {
      success: true,
      errorMessage: null,
      payload: req.body
    };

    res.json(json);
  }
];
