module.exports = () => {
    const loginSuccess = {
      success: true
    }

    const loginError = {
      success: false,
      errorMessage: 'Email address is not exists.',
      errorPropertyName: 'loginEmail'
    }

    const mailingSuccess = {
      success: true
    }

    return { loginSuccess, loginError, mailingSuccess };
};
