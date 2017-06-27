import React, { Component } from 'react';
import { connect } from 'react-redux';
import { getSearchObj } from '../../helpers/location';

import TextInput from '../form/TextInput';
import PasswordInput from '../form/PasswordInput';
import CheckboxInput from '../form/CheckboxInput';
import requestLogin from '../../AC/login';
import { LOGIN } from '../../globals';
import Button from '../Button';

class Login extends Component {
  state = {
    loginEmail: '',
    password: '',
    isKeepMeLoggedIn: false
  };

  static defaultProps = {
    emailText: LOGIN.email,
    emailPlaceholder: LOGIN.emailPlaceholder,
    keepMeLoggedInText: LOGIN.keepMeLoggedIn,
    loginText: LOGIN.login,
    passwordPlaceholder: LOGIN.passwordPlaceholder,
    passwordText: LOGIN.password
  };

  componentDidMount() {
    const { loginEmail, password, isKeepMeLoggedIn } = this.state;
    const { requestLogin } = this.props;

    document.querySelector('body').addEventListener('keypress', (event) => {
      if (event.keyCode === 13) requestLogin(loginEmail, password, isKeepMeLoggedIn);
    });
  }

  handleLoginEmailChange = e => this.setState({ loginEmail: e.target.value });
  handlePasswordChange = e => this.setState({ password: e.target.value });
  handleIsKeepMeLoggedIn = e => this.setState({ isKeepMeLoggedIn: e.target.checked });

  static getErrorMessage(propertyName, failureResponse) {
    let errorMessage = null;

    if (failureResponse !== null && failureResponse.errorPropertyName === propertyName) {
      errorMessage = failureResponse.errorMessage;
    }

    return errorMessage;
  }

  render() {
    const { login, emailText, emailPlaceholder, keepMeLoggedInText,
      loginText, passwordPlaceholder, passwordText, requestLogin } = this.props;
    const { loginEmail, password, isKeepMeLoggedIn } = this.state;
    const { response, isLoading } = login;

    return (
      <div className="css-login">
        <div className="mb-2">
          <TextInput label={emailText}
                     placeholder={emailPlaceholder}
                     value={loginEmail}
                     onChange={e => this.handleLoginEmailChange(e)}
                     error={Login.getErrorMessage('loginEmail', response)}
          />
        </div>

        <div className="mb-2">
          <PasswordInput label={passwordText}
                         placeholder={passwordPlaceholder}
                         value={password}
                         onChange={e => this.handlePasswordChange(e)}
                         error={Login.getErrorMessage('password', response)}
          />
        </div>

        <div className="mb-3">
          <div className="input__wrapper">
            <CheckboxInput
              id="dom-1"
              type="checkbox"
              label={keepMeLoggedInText}
              value={isKeepMeLoggedIn}
              onChange={e => this.handleIsKeepMeLoggedIn(e)}
            />
          </div>
        </div>

        <div className="mb-3">
          <div className="text-center">
            <Button text={loginText}
                    type="action"
                    btnClass="login__login-button btn--no-shadow"
                    isLoading={isLoading}
                    onClick={() => requestLogin(loginEmail, password, isKeepMeLoggedIn)}
            />
          </div>
        </div>
      </div>
    );
  }
}

export default connect((state) => {
  const { login } = state;
  if (login.response && login.response.success) {
    const query = getSearchObj();
    if (query.returnurl) {
      location.assign(query.returnurl);
    } else {
      location.assign('/');
    }
  }
  return { login };
}, {
  requestLogin
})(Login);
