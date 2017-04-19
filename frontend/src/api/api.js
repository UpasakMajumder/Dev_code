module.exports = () => {
    const loginSuccess = {
      success: false,
      errorMessage: 'Email address is not exists.',
      errorPropertyName: 'loginEmail'
    }

    const loginError = {
      success: false,
      errorMessage: 'Email address is not exists.',
      errorPropertyName: 'loginEmail'
    }

    return { loginSuccess, loginError };
};
