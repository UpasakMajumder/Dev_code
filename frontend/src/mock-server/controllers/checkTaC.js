module.exports = (req, res) => {
  const { body } = req;
  if (body.password === '222') {
    res.json({
      success: true,
      payload: {
        showTaC: false,
        url: 'http://localhost:5002/terms-conditions.html'
      },
      errorMessage: null
    });
  } else {
    res.json({
      success: true,
      payload: {
        showTaC: true,
        url: 'http://localhost:5002/terms-conditions.html'
      },
      errorMessage: null
    });
  }
};
