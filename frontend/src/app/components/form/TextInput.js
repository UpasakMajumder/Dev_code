import React, { Component } from 'react';
import PropTypes from 'prop-types';
import removeProps from '../../helpers/object';

class TextInput extends Component {
  constructor() {
    super();

    this.handleFocus = this.handleFocus.bind(this);
    this.handleClick = this.handleClick.bind(this);
  }

  handleFocus(e) {
    console.log('hi');
    const target = e.target;
    if (target.value.length) return;
    this.refs.label.classList.add('input__label--active');
  }

  handleBlur(e) {
    const target = e.target;
    if (target.value.length) return;
    this.refs.label.classList.remove('input__label--active');
  }

  handleClick(e) {
    const target = e.target;
    const input = this.refs.input;
    if (input.value.length) return;
    target.classList.add('input__label--active');
    input.focus();
  }

  render() {
    const { label, error, disabled, labelAnimation, innerElement } = this.props;

    const inputProps = removeProps(this.props, ['label', 'error', 'labelAnimation', 'innerElement']);


    let className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
    const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
    const errorClass = error ? 'input--error' : '';

    let input, labelElement;
    if (labelAnimation) {
      className += ' input__wrapper--label-animation';
      input = <input onBlur={(e) => this.handleBlur(e)}
                     onFocus={(e) => this.handleFocus(e)}
                     ref="input"
                     type="text"
                     className={`input__text ${errorClass}`}
                     {...inputProps} />;

      labelElement = label ? <span onClick={(e) => this.handleClick(e)}
                                         ref="label"
                                         className="input__label">{label}</span> : null;
    } else {
      input = <input type="text"
                           className={`input__text ${errorClass}`}
                           {...inputProps} />;

      labelElement = label ? <span className="input__label">{label}</span> : null;
    }

    return (
      <div className={className}>
        {labelElement}
        <div className="input__inner">
          {innerElement || null}
          {input}
        </div>
        {errorElement}
      </div>
    );
  }
}

TextInput.propTypes = {
  label: PropTypes.string,
  placeholder: PropTypes.string,
  error: PropTypes.string,
  disabled: PropTypes.bool,
  labelAnimation: PropTypes.bool
};

export default TextInput;
