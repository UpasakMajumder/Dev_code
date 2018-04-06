module.exports = (req, res) => {
  if (false) {
    res.json({
      success: true,
      errorMessage: '',
      payload: {
        show: false
      }
    });
  } else {
    res.json({
      success: true,
      errorMessage: '',
      payload: {
        show: true
      }
    });
  }
};
