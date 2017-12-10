module.exports = (req, res) => {
  const { body } = req;
  if (body.password === '111') {
    res.json({
      success: true,
      payload: {
        logonSuccess: false,
        errorPropertyName: 'password',
        errorMessage: 'Password is bad'
      },
      errorMessage: null
    });
  } else {
    res.json({
      success: true,
      payload: {
        logonSuccess: true,
        errorPropertyName: null,
        errorMessage: null
      },
      errorMessage: null
    });
  }
};
