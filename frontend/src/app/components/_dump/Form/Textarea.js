import React from 'react';
import PropTypes from 'prop-types';
/* globals */
import { STATIC_FIELDS } from 'app.globals';

const Textarea = ({
  label,
  disabled,
  error,
  value,
  isOptional,
  name,
  onChange,
  inputClass,
  rows
}) => {
  const labelElement = label ? <span className="input__label">{label}</span> : null;
  const selector = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
  const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
  const errorClass = error ? 'input--error' : '';
  const optionalLabel = isOptional ? <span className="input__right-label">{STATIC_FIELDS.validation.optionalLabel}</span> : null;
  const inputSelector = `${errorClass} ${inputClass}`;

  return (
    <div className={selector}>
      {labelElement}
      {optionalLabel}
      <textarea
        onChange={onChange}
        className={inputSelector}
        value={value}
        rows={rows}
      />
      {errorElement}
    </div>
  );
};

Textarea.defaultProps = {
  label: '',
  disabled: false,
  error: '',
  isOptional: false,
  name: '',
  inputClass: '',
  rows: 1
};

Textarea.propTypes = {
  value: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired,
  label: PropTypes.string,
  disabled: PropTypes.bool,
  error: PropTypes.string,
  isOptional: PropTypes.bool,
  name: PropTypes.string,
  inputClass: PropTypes.string,
  rows: PropTypes.number
};

export default Textarea;
