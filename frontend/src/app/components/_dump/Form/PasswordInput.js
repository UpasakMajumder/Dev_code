import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* utilities */
import { removeProps } from 'app.helpers/object';
import { LOGIN, STATIC_FIELDS } from 'app.globals';

class PasswordInput extends Component {
  state = {
    isShown: false
  };

  static defaultProps = {
    passwordHideText: STATIC_FIELDS.password.hide,
    passwordShowText: STATIC_FIELDS.password.show
  };

  static propTypes = {
    error: PropTypes.string,
    label: PropTypes.string,
    disabled: PropTypes.bool,
    placeholder: PropTypes.string,
    passwordHideText: PropTypes.string,
    passwordShowText: PropTypes.string
  };

  handleToggle = () => this.setState(({ isShown }) => ({ isShown: !isShown }));

  render() {
    const { isShown } = this.state;
    const { error, label, disabled, passwordHideText, passwordShowText } = this.props;

    const inputProps = removeProps(this.props, ['error', 'label', 'passwordHideText', 'passwordShowText']);

    const labelElement = label ? <span className="input__label">{label}</span> : null;
    const toggler = isShown ? passwordHideText : passwordShowText;
    const type = isShown ? 'text' : 'password';
    const className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
    const onClick = disabled ? undefined : this.handleToggle;
    const errorElement = error ? <span className="input__error visible input__error--noborder">{error}</span> : null;
    const errorClass = error ? 'input--error' : '';

    return (
      <div className={className}>
        {labelElement}
        <div className="input__inner">
          <input
            type={type}
            className={`input__password ${errorClass}`}
            {...inputProps} />
          <span onClick={onClick} className="input__toggler">{ toggler }</span>
        </div>
        {errorElement}
      </div>
    );
  }
}

export default PasswordInput;
