import React from 'react';
import { connect } from 'react-redux';

import TextInput from '../form/TextInput';
import PasswordInput from '../form/PasswordInput';
import CheckboxInput from '../form/CheckboxInput';
import requestLogin from '../../AC/login';

class Login extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      loginEmail: '',
      password: '',
      isKeepMeLoggedIn: false
    };
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
            <TextInput label="E-MAIL" placeholder="your@email.com" value={loginEmail} onChange={e => this.handleLoginEmailChange(e)}
                error={this.getErrorMessage('loginEmail', response)} />
          </div>
        </div>
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12">
            <PasswordInput label="Password" placeholder="password" value={password} onChange={e => this.handlePasswordChange(e)}
                error={this.getErrorMessage('password', response)} />
          </div>
        </div>
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12">
            <div className="input__wrapper">
              <CheckboxInput
                id="dom-1"
                type="checkbox"
                label="Keep me logged in"
                value={isKeepMeLoggedIn}
                onChange={e => this.handleIsKeepMeLoggedIn(e)}
              />
          </div>
          </div>
        </div>
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12 text-center">
            <button type="button" className="btn-main css-login__login-button" disabled={isLoading}
             onClick={() => request(loginEmail, password, isKeepMeLoggedIn)}>Log in</button>
          </div>
        </div>
        <div className="row justify-content-center css-login__row mb-4">
          <div className="col-12 text-center">
            <a href="/" className="css-login-link">Forgot password?</a>
          </div>
        </div>
        <div className="row justify-content-center css-login__row">
          <div className="col-12 text-center">
            <a href="/" className="css-login-link">Request access</a>
          </div>
        </div>
      </div>
    );
  }
}

export default connect(
  (state) => {
    const { login } = state;
    if (login.response && login.response.success) location.reload();
    return { login };
  },
  {
    requestLogin
  }
)(Login);
