import React, { Component } from 'react';
import { connect } from 'react-redux';

import TextInput from '../form/TextInput';
import PasswordInput from '../form/PasswordInput';
import CheckboxInput from '../form/CheckboxInput';
import requestLogin from '../../AC/login';
import { LOGIN } from '../../globals';
import queryString from 'query-string';

class Login extends Component {
  constructor(props) {
    super(props);

    this.state = {
      loginEmail: '',
      password: '',
      isKeepMeLoggedIn: false
    };

    // from config
    this.emailText = LOGIN.email;
    this.emailPlaceholder = LOGIN.emailPlaceholder;
    this.keepMeLoggedInText = LOGIN.keepMeLoggedIn;
    this.loginText = LOGIN.login;
    this.passwordPlaceholder = LOGIN.passwordPlaceholder;
    this.passwordText = LOGIN.password;
  }

  handleLoginEmailChange(e) {
    this.setState({
      loginEmail: e.target.value
    });
  }

  handlePasswordChange(e) {
    this.setState({
      password: e.target.value
    });
  }

  handleIsKeepMeLoggedIn(e) {
    this.setState({
      isKeepMeLoggedIn: e.target.checked
    });
  }

  // eslint-disable-next-line
  getErrorMessage(propertyName, failureResponse) {
    let errorMessage = null;

    if (failureResponse != null && (failureResponse.errorPropertyName === propertyName)) {
      errorMessage = failureResponse.errorMessage;
    }

    return errorMessage;
  }

  render() {
    const { requestLogin: request, login: { response, isLoading } } = this.props;
    const { loginEmail, password, isKeepMeLoggedIn } = this.state;

    return (
      <div className="css-login">
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12">
            <TextInput label={this.emailText} placeholder={this.emailPlaceholder} value={loginEmail} onChange={e => this.handleLoginEmailChange(e)}
                error={this.getErrorMessage('loginEmail', response)} />
          </div>
        </div>
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12">
            <PasswordInput label={this.passwordText} placeholder={this.passwordPlaceholder} value={password} onChange={e => this.handlePasswordChange(e)}
                error={this.getErrorMessage('password', response)} />
          </div>
        </div>
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12">
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
        </div>
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12 text-center">
            <button type="button" className="btn-main css-login__login-button" disabled={isLoading}
             onClick={() => request(loginEmail, password, isKeepMeLoggedIn)}>{this.loginText}</button>
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
