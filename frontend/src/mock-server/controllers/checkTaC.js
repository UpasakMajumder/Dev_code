module.exports = (req, res) => {
  const { body } = req;
  if (body.password === '222') {
    res.json({
      success: true,
      payload: {
        showTaC: false,
        url: 'http://localhost:5002/terms-conditions.html',
        logonSuccess: true,
        errorPropertyName: null,
        errorMessage: null
      },
      errorMessage: null
    });
  } else if (body.password === '111') {
    res.json({
      success: true,
      payload: {
        showTaC: false,
        url: 'http://localhost:5002/terms-conditions.html',
        logonSuccess: false,
        errorPropertyName: 'password',
        errorMessage: 'Password is bad'
      },
      errorMessage: null
    });
  } else if (body.password === '333') { // fix
    res.json({
      success: false,
      payload: null,
      errorMessage: 'Hello'
    });
  } else {
    res.json({
      success: true,
      payload: {
        showTaC: true,
        url: 'http://localhost:5002/terms-conditions.html',
        logonSuccess: true,
        errorPropertyName: null,
        errorMessage: null
      },
      errorMessage: null
    });
  }
};
