import React from 'react';
/* utilities */
import { removeProps } from 'app.helpers/object';
/* globals */
import { STATIC_FIELDS } from 'app.globals';

const Select = (props) => {
  const { label, error, disabled, isOptional, options, onChange, value, placeholder } = props;

  const labelElement = label ? <span className="input__label">{label}</span> : null;
  const className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
  const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
  const errorClass = error ? 'input--error' : '';
  const optionalLabel = isOptional ? <span className="input__right-label">{STATIC_FIELDS.validation.optionalLabel}</span> : null;

  let placeholderElement = null;

  if (placeholder) {
    placeholderElement = <option disabled={true} selected={!value}>{placeholder}</option>;
  } else {
    placeholderElement = label ? <option disabled={true} selected={!value}>{label}</option> : null;
  }


  const optionList = options.map((option) => {
    return typeof option === 'string'
      ? <option key={option} value={option}>{option}</option>
      : <option key={option.id} value={option.id}>{option.name}</option>;
  });

  return (
    <div className={className}>
      {labelElement}
      {optionalLabel}
      <div className={`input__select ${errorClass}`}>
        <select
          value={value}
          className={errorClass}
          required={!isOptional}
          onChange={onChange}
          disabled={disabled}
        >
          {placeholderElement}
          {optionList}
        </select>
      </div>
      {errorElement}
    </div>
  );
};

export default Select;
