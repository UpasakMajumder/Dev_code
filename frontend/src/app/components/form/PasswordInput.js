import React, { Component } from 'react';
import PropTypes from 'prop-types';
import removeProps from '../../helpers/object';
import { LOGIN } from '../../globals';

export default class PasswordInput extends Component {
  constructor() {
    super();

    this.state = {
      isShown: false
    };

    this.handleToggle = this.handleToggle.bind(this);
    this.passwordHideText = LOGIN.passwordHide;
    this.passwordShowText = LOGIN.passwordShow;
  }

  handleToggle() {
    this.setState((prevState) => {
      return {
        isShown: !prevState.isShown
      };
    });
  }

  render() {
    const { isShown } = this.state;
    const { error, label, disabled } = this.props;

    const inputProps = removeProps(this.props, ['error', 'label']);

    const labelElement = label ? <span className="input__label">{label}</span> : null;
    const toggler = isShown ? this.passwordHideText : this.passwordShowText;
    const type = isShown ? 'text' : 'password';
    const className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
    const onClick = disabled ? undefined : this.handleToggle;
    const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
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

PasswordInput.propTypes = {
  error: PropTypes.string,
  label: PropTypes.string,
  disabled: PropTypes.bool,
  placeholder: PropTypes.string
};
