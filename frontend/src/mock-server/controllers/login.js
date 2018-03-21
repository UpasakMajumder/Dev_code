module.exports = (req, res) => {
  const { body } = req;
  if (body.password === '111') {
    res.json({
      success: true,
      payload: {
        token: null,
        errorPropertyName: 'password',
        errorMessage: 'Password is incorrect'
      },
      errorMessage: null
    });
  } else {
    res.json({
      success: true,
      payload: {
        token: `${+new Date}`,
        errorPropertyName: null,
        errorMessage: null
      },
      errorMessage: null
    });
  }
};
