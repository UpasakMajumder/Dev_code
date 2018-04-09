import React from 'react';
import PropTypes from 'prop-types';
/* utilities */
import { removeProps } from 'app.helpers/object';
/* globals */
import { STATIC_FIELDS } from 'app.globals';

const TextInput = (props) => {
  const { label, error, disabled, isOptional, className, maxLength } = props;

  const inputProps = removeProps(props, ['label', 'error', 'isOptional', 'isSelect', 'options', 'className']);

  const labelElement = label ? <span className="input__label">{label}</span> : null;
  const selector = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
  const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
  const errorClass = error ? 'input--error' : '';
  const optionalLabel = isOptional ? <span className="input__right-label">{STATIC_FIELDS.validation.optionalLabel}</span> : null;
  const inputSelector = `input__text ${errorClass} ${className}`;

  return (
    <div className={selector}>
      {labelElement}
      {optionalLabel}
      <input
        type="text"
        className={inputSelector}
        maxLength={maxLength}
        {...inputProps}
      />
      {errorElement}
    </div>
  );
};

TextInput.defaultProps = {
  maxLength: 50
};

TextInput.propTypes = {
  label: PropTypes.string,
  placeholder: PropTypes.string,
  error: PropTypes.string,
  disabled: PropTypes.bool,
  isOptional: PropTypes.bool,
  maxLength: PropTypes.number
};

export default TextInput;
