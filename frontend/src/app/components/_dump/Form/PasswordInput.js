import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* 3rd part */
import { Tooltip } from 'react-tippy';
/* components */
import Tooler from 'app.dump/Tooler';
/* utilities */
import { removeProps } from 'app.helpers/object';
import { LOGIN, STATIC_FIELDS } from 'app.globals';

class PasswordInput extends Component {
  state = {
    isShown: false,
    focused: false
  };

  static defaultProps = {
    passwordHideText: STATIC_FIELDS.password.hide,
    passwordShowText: STATIC_FIELDS.password.show,
    strength: null,
    onFocus: () => {},
    onBlur: () => {}
  };

  static propTypes = {
    error: PropTypes.string,
    label: PropTypes.string,
    disabled: PropTypes.bool,
    placeholder: PropTypes.string,
    passwordHideText: PropTypes.string,
    passwordShowText: PropTypes.string,
    strength: PropTypes.shape({
      message: PropTypes.string.isRequired,
      open: PropTypes.bool.isRequired,
      level: PropTypes.number.isRequire
    }),
    info: PropTypes.string,
    // func
    onFocus: PropTypes.func,
    onBlur: PropTypes.func
  };

  handleFocus = () => {
    this.props.onFocus();
    this.setState({ focused: true });
  };

  handleBlur = () => {
    this.props.onBlur();
    this.setState({ focused: false });
  };

  handleToggle = () => this.setState(({ isShown }) => ({ isShown: !isShown }));

  getDefineStrengthComponent = () => {
    const { strength } = this.props;
    if (!strength) return null;

    return (
      <Tooltip
        title={strength.message}
        position="left"
        animation="perspective"
        open={strength.open}
        arrow
        unmountHTMLWhenHide
      >
        <div className={`psw-strength ${this.state.focused ? 'psw-strength--focused' : ''} psw-strength--level-${strength.level}`}/>
      </Tooltip>
    );
  };

  render() {
    const { isShown } = this.state;
    const {
      error,
      label,
      disabled,
      passwordHideText,
      passwordShowText,
      info
    } = this.props;

    const inputProps = removeProps(this.props, ['error', 'label', 'passwordHideText', 'passwordShowText', 'onFocus', 'onBlur']);

    const labelElement = label ? <span className="input__label">{label}</span> : null;
    const infoLabel = info ? <div className="input__right-label"><Tooler type="info" html={info} /></div> : null;
    const toggler = isShown ? passwordHideText : passwordShowText;
    const type = isShown ? 'text' : 'password';
    const className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
    const onClick = disabled ? undefined : this.handleToggle;
    const errorElement = error ? <span className="input__error visible input__error--noborder">{error}</span> : null;
    const errorClass = error ? 'input--error' : '';

    return (
      <div className={className}>
        {labelElement}
        {infoLabel}
        <div className="input__inner">
          <input
            type={type}
            className={`input__password ${errorClass}`}
            onFocus={this.handleFocus}
            onBlur={this.handleBlur}
            {...inputProps} />
          <span onClick={onClick} className="input__toggler">{ toggler }</span>
        </div>
        {this.getDefineStrengthComponent()}
        {errorElement}
      </div>
    );
  }
}

export default PasswordInput;
