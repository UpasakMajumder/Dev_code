import React, { PropTypes } from 'react';
import removeProps from '../../helpers/object';

export default function TextInput(props) {
  const { label, error, disabled } = props;

  const inputProps = removeProps(props, ['label', 'error']);

  const labelElement = label ? <span className="input__label">{label}</span> : null;
  const className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
  const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
  const errorClass = error ? 'input--error' : '';

  return (
    <div className={className}>
      {labelElement}
      <input
        type="text"
        className={`input__text ${errorClass}`}
        {...inputProps} />
      {errorElement}
    </div>
  );
}

TextInput.propTypes = {
  label: PropTypes.string,
  placeholder: PropTypes.string,
  error: PropTypes.string,
  disabled: PropTypes.bool
};
