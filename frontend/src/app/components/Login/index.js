import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import axios from 'axios';
/* components */
import Button from 'app.dump/Button';
import TextInput from 'app.dump/Form/TextInput';
import PasswordInput from 'app.dump/Form/PasswordInput';
import CheckboxInput from 'app.dump/Form/CheckboxInput';
/* ac */
import { checkTaC } from 'app.ac/tac';
/* utilities */
import { LOGIN, TAC } from 'app.globals';
import { getSearchObj } from 'app.helpers/location';
import { emailRegExp } from 'app.helpers/regexp';
/* constants */
import { FAILURE } from 'app.consts';

class Login extends Component {
  static defaultProps = {
    keepMeLoggedInText: LOGIN.keepMeLoggedIn,
    passwordText: LOGIN.password,
    emailText: LOGIN.email,
    loginText: LOGIN.login,
    passwordPlaceholder: LOGIN.passwordPlaceholder,
    emailPlaceholder: LOGIN.emailPlaceholder,
    loginUrl: LOGIN.loginUrl,
    emailValidationMessage: LOGIN.emailValidationMessage,
    passwordValidationMessage: LOGIN.passwordValidationMessage
  };

  static propTypes = {
    keepMeLoggedInText: PropTypes.string.isRequired,
    passwordText: PropTypes.string.isRequired,
    emailText: PropTypes.string.isRequired,
    loginText: PropTypes.string.isRequired,
    passwordPlaceholder: PropTypes.string,
    emailPlaceholder: PropTypes.string,
    loginUrl: PropTypes.string.isRequired,
    emailValidationMessage: PropTypes.string.isRequired,
    passwordValidationMessage: PropTypes.string.isRequired
  };

  state = {
    loginEmail: '',
    password: '',
    isKeepMeLoggedIn: false,
    isLoading: false,
    invalids: []
  };

  toggleKeepMeLoggedIn = () => this.setState(prevState => ({ isKeepMeLoggedIn: !prevState.isKeepMeLoggedIn }));

  handleChangeField = (field, value) => {
    this.setState({
      [field]: value,
      invalids: this.state.invalids.filter(invalid => invalid.field !== field)
    });
  };

  isValid = () => {
    const invalids = [];

    if (!this.state.loginEmail.match(emailRegExp)) {
      invalids.push({
        field: 'loginEmail',
        errorMessage: this.props.emailValidationMessage
      });
    }

    if (this.state.password.length === 0) {
      invalids.push({
        field: 'password',
        errorMessage: this.props.passwordValidationMessage
      });
    }
    this.setState({ invalids });

    return !invalids.length;
  };

  getToken = async () => {
    const { loginEmail, password, isKeepMeLoggedIn } = this.state;
    const body = { loginEmail, password, isKeepMeLoggedIn };
    const response = await axios.post(LOGIN.loginUrl, body);
    const { success, payload, errorMessage } = response.data;
    if (success) {
      if (payload.token) {
        return payload.token; // return token
      }

      const invalids = [{ field: payload.errorPropertyName, errorMessage: payload.errorMessage }];
      this.setState({ invalids });
      return null;
    }

    window.store.dispatch({
      type: FAILURE,
      alert: errorMessage
    });

    return null;
  };

  handleSubmit = async () => {
    this.setState(prevState => ({ isLoading: !prevState.isLoading }));
    // validation
    const isValid = this.isValid();
    if (isValid) {
      // get token
      const token = await this.getToken();
      if (token) {
        // define redirectUrl to redirect
        let returnurl;
        const query = getSearchObj();
        if (query.returnurl) {
          returnurl = decodeURIComponent(query.returnurl);
        } else {
          returnurl = '/';
        }

        // checkTaC
        // redirect
        this.props.checkTaC({
          url: TAC.checkTaCUrl,
          token,
          redirect: true,
          returnurl
        });
      }
    }

    this.setState(prevState => ({ isLoading: !prevState.isLoading }));
  };

  componentDidMount() {
    document.querySelector('body').addEventListener('keypress', (event) => {
      if (event.keyCode === 13) this.handleSubmit();
    });
  }

  getErrorMessage = (field) => {
    if (!this.state.invalids.length) return '';
    const invalid = this.state.invalids.find(invalid => invalid.field === field);
    if (!invalid) return '';
    return invalid.errorMessage;
  }

  render() {
    const {
      emailText,
      emailPlaceholder,
      keepMeLoggedInText,
      loginText,
      passwordPlaceholder,
      passwordText
    } = this.props;

    const {
      loginEmail,
      password,
      isKeepMeLoggedIn,
      isLoading
    } = this.state;

    return (
      <div>
        <div className="css-login">
          <div className="mb-2">
            <TextInput
              label={emailText}
              placeholder={emailPlaceholder}
              value={loginEmail}
              onChange={e => this.handleChangeField('loginEmail', e.target.value)}
              error={this.getErrorMessage('loginEmail')}
            />
          </div>

          <div className="mb-2">
            <PasswordInput
              label={passwordText}
              placeholder={passwordPlaceholder}
              value={password}
              onChange={e => this.handleChangeField('password', e.target.value)}
              error={this.getErrorMessage('password')}
            />
          </div>

          <div className="mb-3">
            <div className="input__wrapper">
              <CheckboxInput
                id="dom-1"
                type="checkbox"
                label={keepMeLoggedInText}
                value={isKeepMeLoggedIn}
                onChange={this.toggleKeepMeLoggedIn}
              />
            </div>
          </div>

          <div className="mb-3">
            <div className="text-center">
              <Button
                text={loginText}
                type="action"
                btnClass="login__login-button btn--no-shadow"
                isLoading={isLoading}
                onClick={this.handleSubmit}
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default connect(null, {
  checkTaC
})(Login);
