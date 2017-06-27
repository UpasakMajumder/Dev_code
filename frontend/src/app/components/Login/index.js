import React, { Component } from 'react';
import { connect } from 'react-redux';
import queryString from 'query-string';

import TextInput from '../form/TextInput';
import PasswordInput from '../form/PasswordInput';
import CheckboxInput from '../form/CheckboxInput';
import requestLogin from '../../AC/login';
import { LOGIN } from '../../globals';
import Button from '../Button';

class Login extends Component {
  constructor(props) {
    super(props);

    // from config
    this.emailText = LOGIN.email;
    this.emailPlaceholder = LOGIN.emailPlaceholder;
    this.keepMeLoggedInText = LOGIN.keepMeLoggedIn;
    this.loginText = LOGIN.login;
    this.passwordPlaceholder = LOGIN.passwordPlaceholder;
    this.passwordText = LOGIN.password;

    document.querySelector('body').addEventListener('keypress', (event) => {
      if (event.keyCode === 13) {
        this.props.requestLogin(this.state.loginEmail, this.state.password, this.state.isKeepMeLoggedIn);
      }
    });
  }

  state = {
    loginEmail: '',
    password: '',
    isKeepMeLoggedIn: false
  };

  handleLoginEmailChange(e) {
    this.setState({ loginEmail: e.target.value });
  }

  handlePasswordChange(e) {
    this.setState({ password: e.target.value });
  }

  handleIsKeepMeLoggedIn(e) {
    this.setState({ isKeepMeLoggedIn: e.target.checked });
  }

  static getErrorMessage(propertyName, failureResponse) {
    let errorMessage = null;

    if (failureResponse !== null && (failureResponse.errorPropertyName === propertyName)) {
      errorMessage = failureResponse.errorMessage;
    }

    return errorMessage;
  }

  render() {
    const { requestLogin: request, login: { response, isLoading } } = this.props;
    const { loginEmail, password, isKeepMeLoggedIn } = this.state;

    return (
      <div className="css-login">
        <div className="mb-2">
          <TextInput label={this.emailText}
                     placeholder={this.emailPlaceholder}
                     value={loginEmail}
                     onChange={e => this.handleLoginEmailChange(e)}
                     error={Login.getErrorMessage('loginEmail', response)} />
        </div>
        <div className="mb-2">
          <PasswordInput label={this.passwordText}
                         placeholder={this.passwordPlaceholder}
                         value={password}
                         onChange={e => this.handlePasswordChange(e)}
                         error={Login.getErrorMessage('password', response)} />
        </div>
        <div className="mb-3">
          <div className="input__wrapper">
            <CheckboxInput
              id="dom-1"
              type="checkbox"
              label={this.keepMeLoggedInText}
              value={isKeepMeLoggedIn}
              onChange={e => this.handleIsKeepMeLoggedIn(e)}
            />
          </div>
        </div>
        <div className="mb-3">
          <div className="text-center">
            <Button text={this.loginText}
                    type="action"
                    btnClass="login__login-button btn--no-shadow"
                    isLoading={isLoading}
                    onClick={() => request(loginEmail, password, isKeepMeLoggedIn)}/>
          </div>
        </div>
      </div>
    );
  }
}

export default connect(
  (state) => {
    const { login } = state;
    if (login.response && login.response.success) {
      const query = queryString.parse(location.search);
      if (query.returnurl) {
        location.assign(query.returnurl);
      } else {
        location.assign('/');
      }
    }
    return { login };
  },
  {
    requestLogin
  }
)(Login);
