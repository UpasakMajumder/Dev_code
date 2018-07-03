import React, { Component } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
/* components */
import TextInput from 'app.dump/Form/TextInput';
import PasswordInput from 'app.dump/Form/PasswordInput';
import Button from 'app.dump/Button';
/* utilities */
import { emailRegExp } from 'app.helpers/regexp';
import defineStrength from 'app.helpers/password';
/* constants */
import { FAILURE } from 'app.consts';
/* globals */
import { REGISTRATION } from 'app.globals';
/* local components */
import LoginModal from './LoginModal';

class Registration extends Component {
  static defaultProps = {
    config: REGISTRATION ? {
      title: REGISTRATION.title || 'Sign Up2',
      labels: {
        firstName: REGISTRATION.labels.firstName || 'First Name1',
        lastName: REGISTRATION.labels.lastName || 'Last Name1',
        email: REGISTRATION.labels.email || 'Email1',
        password: REGISTRATION.labels.password || 'Password1',
        confirmPassword: REGISTRATION.labels.confirmPassword || 'Confirm Password1',
        button: REGISTRATION.labels.button || 'Sign Up1'
      },
      errors: {
        required: REGISTRATION.errors.required || 'Required!',
        email: REGISTRATION.errors.email || 'Invalid Email!',
        confirmPassword: REGISTRATION.errors.confirmPassword || 'Passwords are not equal'
      },
      url: REGISTRATION.url || '',
      toLogin: REGISTRATION.toLogin || {
        url: '#',
        text: 'Sign Inn'
      },
      strength: REGISTRATION.strength || null,
      loginModal: REGISTRATION.loginModal || {
        body: 'Your account was successfully created. Click the login link below to access the storefront.',
        btnLabel: 'Login',
        title: 'Registration successful'
      }
    } : {
      title: 'Sign Up',
      labels: {
        firstName: 'First Name',
        lastName: 'Last Name',
        email: 'Email',
        password: 'Password',
        confirmPassword: 'Confirm Password',
        button: 'Sign Up'
      },
      errors: {
        required: 'Required!',
        email: 'Invalid Email!',
        confirmPassword: 'Passwords are not equal'
      },
      url: '',
      toLogin: {
        url: '#',
        text: 'Sign In'
      },
      strength: null,
      loginModal: {
        body: 'Your account was successfully created. Click the login link below to access the storefront.',
        btnLabel: 'Login',
        title: 'Registration successful'
      }
    }
  }

  static propTypes = {
    config: PropTypes.shape({
      title: PropTypes.string.isRequired,
      labels: PropTypes.shape({
        firstName: PropTypes.string.isRequired,
        lastName: PropTypes.string.isRequired,
        email: PropTypes.string.isRequired,
        password: PropTypes.string.isRequired,
        confirmPassword: PropTypes.string.isRequired,
        button: PropTypes.string.isRequired
      }).isRequired,
      errors: PropTypes.shape({
        required: PropTypes.string.isRequired,
        email: PropTypes.string.isRequired,
        confirmPassword: PropTypes.string.isRequired
      }).isRequired,
      url: PropTypes.string.isRequired,
      toLogin: {
        url: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired
      },
      strength: PropTypes.shape({
        minLength: PropTypes.number,
        preferedLength: PropTypes.number,
        minNonAlphaNumChars: PropTypes.number,
        preferedNonAlphaNumChars: PropTypes.number,
        regularExpression: PropTypes.string,
        policyStrings: PropTypes.arrayOf(PropTypes.string.isRequired).isRequired,
        usePolicy: PropTypes.bool,
        info: PropTypes.string.isRequire
      }),
      loginModal: PropTypes.shape({
        btnLabel: PropTypes.string.isRequired,
        body: PropTypes.string.isRequired,
        title: PropTypes.string.isRequired
      }).isRequired
    }).isRequired
  }

  state = {
    fields: {
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: ''
    },
    isPending: false,
    invalids: [{ field: 'password' }],
    strength: {
      open: false,
      level: 0,
      message: this.props.config.strength.policyStrings[0],
      valid: false
    },
    showLoginModal: false
  };

  getErrorMessage = (name) => {
    if (!this.state.invalids.length) return '';
    const invalid = this.state.invalids.find(invalid => invalid.field === name);
    if (!invalid) return '';
    return invalid.errorMessage;
  }

  toggleStrength = (open) => {
    this.setState({
      strength: {
        ...this.state.strength,
        open
      }
    });
  }

  handleChangeInput = (event) => {
    const { target } = event;
    const field = target.name;
    const { value } = target;

    this.setState({
      fields: {
        ...this.state.fields,
        [field]: value
      },
      invalids: this.state.invalids.filter((invalid) => {
        return invalid.field !== field && field !== 'password';
      })
    });

    if (field === 'password') {
      this.getStrength(value);
    }
  };

  handleRegistrationSuccess = () => {
    this.setState({ showLoginModal: true });
  }

  isValid = () => {
    const {
      firstName,
      lastName,
      email,
      password,
      confirmPassword
    } = this.state.fields;

    const {
      errors
    } = this.props.config;

    const invalids = [];

    if (firstName.length === 0) {
      invalids.push({
        field: 'firstName',
        errorMessage: errors.required
      });
    }

    if (lastName.length === 0) {
      invalids.push({
        field: 'lastName',
        errorMessage: errors.required
      });
    }

    if (!email.match(emailRegExp)) {
      invalids.push({
        field: 'email',
        errorMessage: errors.email
      });
    }

    if (this.state.invalids.some(invalid => invalid.field === 'password')) {
      invalids.push({
        field: 'password'
      });
      this.setState({
        strength: {
          ...this.state.strength,
          open: true
        }
      });
    }

    if (password !== confirmPassword) {
      invalids.push({
        field: 'confirmPassword',
        errorMessage: errors.confirmPassword
      });
    }

    this.setState({ invalids });

    return !invalids.length;
  };

  sendRequest = () => {
    const { url } = this.props.config;

    if (!url) {
      window.store.dispatch({
        type: FAILURE,
        alert: 'Config is not specified!'
      });
      return;
    }

    axios.post(this.props.config.url, {
      firstName: this.state.fields.firstName,
      lastName: this.state.fields.lastName,
      email: this.state.fields.email,
      password: this.state.fields.password
    })
      .then((response) => {
        const { success, payload, errorMessage } = response.data;
        if (success) {
          if (payload) {
            if (payload.errorPropertyName) {
              const invalids = [{ field: payload.errorPropertyName, errorMessage: payload.errorMessage }];
              this.setState({ invalids });
            }
          } else {
            this.handleRegistrationSuccess();
          }
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((e) => {
        // eslint-disable-next-line
        console.error(e);
        window.store.dispatch({ type: FAILURE });
      });
  };

  getStrength = (password) => {
    const { strength } = this.props.config;
    const strengthResult = defineStrength({
      password,
      ...strength
    });

    const invalids = this.state.invalids.filter(invalid => invalid.field !== 'password');
    if (!strengthResult.valid) {
      invalids.push({ field: 'password' });
    }

    this.setState({
      invalids,
      strength: {
        ...this.state.strength,
        message: strengthResult.message,
        level: strengthResult.level,
        valid: strengthResult.valid
      }
    });
  }

  handleSubmit = () => {
    this.setState({ isPending: true });
    try {
      if (this.isValid()) this.sendRequest();
    } catch (e) {
      console.error(e); // eslint-disable-line
    }
    this.setState({ isPending: false });
  };

  renderLoginModal = () => {
    const { toLogin, loginModal } = this.props.config;
    const { btnLabel, body, title } = loginModal;

    return (
      <LoginModal
        body={body}
        btnLabel={btnLabel}
        btnLink={toLogin.url}
        title={title}
      />
    );
  }

  render() {
    const { fields, showLoginModal } = this.state;
    const { config } = this.props;

    return (
      <div>
        <h1 className="mb-3 text-center">{config.title}</h1>

        <div className="mb-2">
          <TextInput
            label={config.labels.firstName}
            name="firstName"
            value={fields.firstName}
            onChange={this.handleChangeInput}
            error={this.getErrorMessage('firstName')}
            optional
          />
        </div>

        <div className="mb-2">
          <TextInput
            label={config.labels.lastName}
            name="lastName"
            value={fields.lastName}
            onChange={this.handleChangeInput}
            error={this.getErrorMessage('lastName')}
          />
        </div>

        <div className="mb-2">
          <TextInput
            label={config.labels.email}
            name="email"
            value={fields.email}
            onChange={this.handleChangeInput}
            error={this.getErrorMessage('email')}
          />
        </div>

        <div className="mb-2">
          <PasswordInput
            label={config.labels.password}
            name="password"
            value={fields.password}
            onChange={this.handleChangeInput}
            strength={this.state.strength}
            info={config.strength.info}
            onFocus={() => this.toggleStrength(true)}
            onBlur={() => this.toggleStrength(false)}
          />
        </div>

        <div className="mb-3">
          <PasswordInput
            label={config.labels.confirmPassword}
            name="confirmPassword"
            value={fields.confirmPassword}
            onChange={this.handleChangeInput}
            error={this.getErrorMessage('confirmPassword')}
          />
        </div>

        <div className="mb-3">
          <Button
            text={config.labels.button}
            type="action"
            btnClass="login__login-button btn--no-shadow"
            onClick={this.handleSubmit}
            isLoading={this.state.isPending}
          />
        </div>

        <div>
          <a className="link" href={config.toLogin.url}>{config.toLogin.text}</a>
        </div>

        {showLoginModal && this.renderLoginModal()}
      </div>
    );
  }
}

export default Registration;
