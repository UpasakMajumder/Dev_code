module.exports = () => {
    const loginSuccess = {
      success: true
    }

    const loginError = {
      success: false,
      errorMessage: 'Email address is not exists.',
      errorPropertyName: 'loginEmail'
    }

    return { loginSuccess, loginError };
};
