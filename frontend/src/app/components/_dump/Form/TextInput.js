import React from 'react';
import PropTypes from 'prop-types';
/* utilities */
import { removeProps } from 'app.helpers/object';

const TextInput = (props) => {
  const { label, error, disabled, isOptional } = props;

  const inputProps = removeProps(props, ['label', 'error', 'isOptional', 'isSelect', 'options']);

  const labelElement = label ? <span className="input__label">{label}</span> : null;
  const className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
  const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
  const errorClass = error ? 'input--error' : '';
  const optionalLabel = isOptional ? <span className="input__right-label">optional</span> : null;

  return (
    <div className={className}>
      {labelElement}
      {optionalLabel}
      <input
        type="text"
        className={`input__text ${errorClass}`}
        {...inputProps} />
      {errorElement}
    </div>
  );
};

TextInput.propTypes = {
  label: PropTypes.string,
  placeholder: PropTypes.string,
  error: PropTypes.string,
  disabled: PropTypes.bool,
  isOptional: PropTypes.bool
};

export default TextInput;
