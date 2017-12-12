import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
/* components */
import Button from 'app.dump/Button';
import TextInput from 'app.dump/Form/TextInput';
import PasswordInput from 'app.dump/Form/PasswordInput';
import CheckboxInput from 'app.dump/Form/CheckboxInput';
/* ac */
import {
  checkTaC,
  loginSubmit,
  changeCredentinals
} from 'app.ac/login';
/* utilities */
import { LOGIN } from 'app.globals';
import { getSearchObj } from 'app.helpers/location';

class Login extends Component {
  static defaultProps = {
    emailText: LOGIN.email,
    emailPlaceholder: LOGIN.emailPlaceholder,
    keepMeLoggedInText: LOGIN.keepMeLoggedIn,
    loginText: LOGIN.login,
    passwordPlaceholder: LOGIN.passwordPlaceholder,
    passwordText: LOGIN.password,
    loginUrl: LOGIN.loginUrl,
    acceptTaCUrl: LOGIN.acceptTaCUrl,
    checkTaCUrl: LOGIN.checkTaCUrl
  };

  static propTypes = {
    keepMeLoggedInText: PropTypes.string.isRequired,
    passwordText: PropTypes.string.isRequired,
    checkTaC: PropTypes.func.isRequired,
    loginSubmit: PropTypes.func.isRequired,
    emailText: PropTypes.string.isRequired,
    loginText: PropTypes.string.isRequired,
    passwordPlaceholder: PropTypes.string,
    emailPlaceholder: PropTypes.string,
    login: PropTypes.shape({
      credentinals: PropTypes.shape({
        loginEmail: PropTypes.string.isRequired,
        password: PropTypes.string.isRequired,
        isKeepMeLoggedIn: PropTypes.bool.isRequired
      }).isRequired,
      checkTaC: PropTypes.shape({
        showTaC: PropTypes.bool.isRequired,
        url: PropTypes.string.isRequired,
        isAsked: PropTypes.bool.isRequired
      }).isRequired,
      acceptTaC: PropTypes.bool.isRequired,
      submit: PropTypes.shape({
        logonSuccess: PropTypes.bool.isRequired,
        errorPropertyName: PropTypes.string,
        errorMessage: PropTypes.string,
        isAsked: PropTypes.bool.isRequired
      }).isRequired,
      isLoading: PropTypes.bool.isRequired
    }).isRequired,
    loginUrl: PropTypes.string.isRequired
  };

  checkTaCSubmit = () => {
    const { checkTaCUrl } = LOGIN;
    const { loginEmail, password } = this.props.login.credentinals;
    this.props.checkTaC(checkTaCUrl, { loginEmail, password });
  };

  loginSubmit = () => {
    const { loginUrl } = LOGIN;
    const { loginEmail, password, isKeepMeLoggedIn } = this.props.login.credentinals;
    this.props.loginSubmit(loginUrl, { loginEmail, password, isKeepMeLoggedIn });
  };

  componentWillReceiveProps(nextProps) {
    const { login: loginProps } = this.props;
    const { login: loginNextProps } = nextProps;

    // if necessary not to show TaC
    if (loginNextProps.checkTaC.isAsked && !loginNextProps.submit.isAsked) {
      if (!loginNextProps.checkTaC.showTaC) {
        this.loginSubmit();
      }
    }

    // if TaC accepted
    if (loginProps.acceptTaC !== loginNextProps.acceptTaC && loginNextProps.acceptTaC) {
      this.loginSubmit();
    }

    // if logged in
    if (loginProps.submit.logonSuccess !== loginNextProps.submit.logonSuccess && loginNextProps.submit.logonSuccess) {
      const query = getSearchObj();
      if (query.returnurl) {
        location.assign(decodeURIComponent(query.returnurl));
      } else {
        location.assign('/');
      }
    }
  }

  componentDidMount() {
    document.querySelector('body').addEventListener('keypress', (event) => {
      if (event.keyCode === 13) this.checkTaCSubmit();
    });
  }

  changeCredentinals = (field, value) => {
    this.props.changeCredentinals(field, value);
  };

  static getErrorMessage(propertyName, invalids) {
    if (!invalids) return '';
    if (invalids.errorPropertyName === propertyName) return invalids.errorMessage;
    return '';
  }

  render() {
    const {
      login,
      emailText,
      emailPlaceholder,
      keepMeLoggedInText,
      loginText,
      passwordPlaceholder,
      passwordText
    } = this.props;

    const { credentinals } = login;

    return (
      <div>
        <div className="css-login">
          <div className="mb-2">
            <TextInput
              label={emailText}
              placeholder={emailPlaceholder}
              value={credentinals.loginEmail}
              onChange={e => this.changeCredentinals('loginEmail', e.target.value)}
              error={Login.getErrorMessage('loginEmail', login.submit)}
            />
          </div>

          <div className="mb-2">
            <PasswordInput
              label={passwordText}
              placeholder={passwordPlaceholder}
              value={credentinals.password}
              onChange={e => this.changeCredentinals('password', e.target.value)}
              error={Login.getErrorMessage('password', login.submit)}
            />
          </div>

          <div className="mb-3">
            <div className="input__wrapper">
              <CheckboxInput
                id="dom-1"
                type="checkbox"
                label={keepMeLoggedInText}
                value={credentinals.isKeepMeLoggedIn}
                onChange={e => this.changeCredentinals('isKeepMeLoggedIn', !credentinals.isKeepMeLoggedIn)}
              />
            </div>
          </div>

          <div className="mb-3">
            <div className="text-center">
              <Button text={loginText}
                      type="action"
                      btnClass="login__login-button btn--no-shadow"
                      isLoading={login.isLoading}
                      onClick={this.checkTaCSubmit}
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default connect((state) => {
  const { login } = state;
  return { login };
}, {
  checkTaC,
  loginSubmit,
  changeCredentinals
})(Login);
